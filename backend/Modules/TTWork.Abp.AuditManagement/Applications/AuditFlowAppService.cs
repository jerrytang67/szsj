using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.UI;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TTWork.Abp.AuditManagement.Applications.Dtos;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.AuditManagement.Events.Queries;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Extensions;

namespace TTWork.Abp.AuditManagement.Applications
{
    public interface IAuditFlowAppService : IAsyncCrudAppService<AuditFlowDto, Guid, AuditRequestDto,
        AuditFlowCreateOrEditDto, AuditFlowCreateOrEditDto>
    {
        Task<GetForEditOutput<AuditFlowCreateOrEditDto>> GetForEdit(EntityDto<Guid> input);
        Task<AuditFlowDto> GetByName(string auditName);
    }

    /// <summary>
    /// 审核流程
    /// </summary>
    public class AuditFlowAppService : AbpAsyncCrudAppService<AuditFlow, AuditFlowDto, Guid, AuditRequestDto,
        AuditFlowCreateOrEditDto, AuditFlowCreateOrEditDto>, IAuditFlowAppService
    {
        private readonly IMediator _mediator;
        private readonly IocManager _iocManager;
        private readonly AuditDefinitionManager _auditDefinitionManager;
        private readonly IRepository<AuditNode, Guid> _auditNodeRepository;
        private readonly IAuditProvider _auditProvider;

        public AuditFlowAppService(
            IRepository<AuditNode, Guid> auditNodeRepository,
            IRepository<AuditFlow, Guid> repository,
            IAuditProvider auditProvider,
            IMediator mediator,
            IocManager iocManager,
            AuditDefinitionManager auditDefinitionManager
        ) : base(repository, iocManager)
        {
            _mediator = mediator;
            _iocManager = iocManager;
            _auditDefinitionManager = auditDefinitionManager;
            _auditNodeRepository = auditNodeRepository;
            _auditProvider = auditProvider;

            base.EnableGetEdit = true;
        }

        public override async Task<AuditFlowDto> GetAsync(EntityDto<Guid> input)
        {
            var result = await base.GetAsync(input);
            var nodes = _auditNodeRepository.GetAllList(s => s.AuditFlowId == input.Id);
            result.AuditNodes = ObjectMapper.Map<List<AuditNodeDto>>(nodes);
            return result;
        }

        public override async Task<GetForEditOutput<AuditFlowCreateOrEditDto>> GetForEdit(EntityDto<Guid> input)
        {
            var find = await Repository
                .GetAllIncluding(x => x.AuditNodes)
                .FirstOrDefaultAsync(z => z.Id == input.Id);

            var schema = JToken.FromObject(new { });

            var auditDefinitions = await _mediator.Send(new QueryAuditDefinition());

            schema["auditDefinitions"] = auditDefinitions.GetSelection("string", "DisplayName", "Name", _iocManager);

            return new GetForEditOutput<AuditFlowCreateOrEditDto>(
                find != null
                    ? ObjectMapper.Map<AuditFlowCreateOrEditDto>(find)
                    : new AuditFlowCreateOrEditDto()
                    {
                        Enable = true,
                        ProviderName = "T",
                        ProviderKey = AbpSession.TenantId.ToString()
                    },
                schema);
        }

        public override async Task<PagedResultDto<AuditFlowDto>> GetAllAsync(AuditRequestDto input)
        {
            var query = Repository.GetAll()
                    .Include(x => x.AuditNodes)
                    .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.AuditName.Contains(input.Keyword))
                ;
            var total = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var result = new PagedResultDto<AuditFlowDto>(total, ObjectMapper.Map<List<AuditFlowDto>>(items));

            foreach (var item in result.Items)
            {
                var auditFlowDefinitiion = _auditDefinitionManager.Get(item.AuditName);
                using var localizationContext = _iocManager.ResolveAsDisposable<ILocalizationContext>();
                item.AuditDisplayName = auditFlowDefinitiion.DisplayName.Localize(localizationContext.Object);
            }

            return result;
        }

        public override async Task<AuditFlowDto> CreateAsync(AuditFlowCreateOrEditDto input)
        {
            await CheckSameName(input);

            CheckSameIndexSameUser(input);

            return await base.CreateAsync(input);
        }

        public override async Task<AuditFlowDto> UpdateAsync(AuditFlowCreateOrEditDto input)
        {
            await CheckSameName(input);

            CheckSameIndexSameUser(input);

            CheckUpdatePermission();

            var entity = await base.Repository
                .GetAllIncluding(x => x.AuditNodes)
                .FirstOrDefaultAsync(z => z.Id == input.Id);

            MapToEntity(input, entity);

            await CurrentUnitOfWork.SaveChangesAsync();


            // TODO: 重要!!!!如果更新了流程,需要同步所有用到这个非审核的项目重新开始审核

            return MapToEntityDto(entity);
        }


        public async Task<AuditFlowDto> GetByName(string auditName)
        {
            var flowId = await _auditProvider.GetOrNullAsync(auditName);
            if (!flowId.HasValue) return null;

            var auditFlow = await Repository
                .GetAllIncluding(x => x.AuditNodes)
                .FirstOrDefaultAsync(z => z.Id == flowId);
            return MapToEntityDto(auditFlow);
        }

        /// <summary>
        /// 检查同级别是否有同用户
        /// </summary>
        private void CheckSameIndexSameUser(AuditFlowCreateOrEditDto input)
        {
            if (input.AuditNodes == null) return;

            var group = input.AuditNodes.GroupBy(x => new {x.UserId, x.Index})
                .Select(x => new {key = x.Key, UserNanme = x.FirstOrDefault()?.UserName, items = x.ToList()});

            foreach (var g in @group)
            {
                if (g.items.Count > 1)
                    throw new UserFriendlyException(
                        $"用户 {g.UserNanme} 在 {g.key.Index} 层审核出现了 {g.items.Count}次,每层只能允许一次");
            }
        }

        /// <summary>
        /// 检查是否有同名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task CheckSameName(AuditFlowCreateOrEditDto input)
        {
            if (await Repository.GetAll().AnyAsync(x => x.AuditName == input.AuditName &&
                                                        x.ProviderName == input.ProviderName &&
                                                        x.ProviderKey == input.ProviderKey &&
                                                        x.Id != input.Id))
            {
                throw new UserFriendlyException("相同内容已存在");
            }
        }
    }
}