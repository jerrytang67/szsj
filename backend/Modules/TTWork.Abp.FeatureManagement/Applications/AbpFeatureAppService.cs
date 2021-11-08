using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Organizations;
using Abp.UI;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.Core.Organizations;
using TTWork.Abp.FeatureManagement.Applications.Dtos;
using TTWork.Abp.FeatureManagement.Domain;
using TTWork.Abp.FeatureManagement.Events.Queries;
using TTWork.Abp.FeatureManagement.Features;

namespace TTWork.Abp.FeatureManagement.Applications
{
    public interface IAbpFeatureAppService : IAsyncCrudAppService<AbpFeatureDto, Guid, AbpFeatureRequestDto,
        AbpFeatureDto, AbpFeatureDto>
    {
        Task<List<FeatureDefinition>> GetAllFeatureDefinitionAsync();

        Task<GetForEditOutput<AbpFeatureDto>> GetForEdit(EntityDto<Guid> input);
    }

    public class AbpFeatureAppService : AbpAsyncCrudAppService<AbpFeature, AbpFeatureDto, Guid, AbpFeatureRequestDto,
        AbpFeatureDto, AbpFeatureDto>, IAbpFeatureAppService
    {
        private readonly IRepository<AbpFeature, Guid> _repository;
        private readonly IMediator _mediator;
        private readonly IocManager _iocManager;
        private readonly ICurrentOrganization _currentOrganization;
        private readonly IRepository<OrganizationUnit, long> _organizationRepository;
        private readonly IFeatureProvider _provider;
        private readonly IFeatureDefinitionManager _definitionManager;

        public AbpFeatureAppService(
            ICurrentOrganization currentOrganization,
            IRepository<OrganizationUnit, long> organizationRepository,
            IFeatureProvider provider,
            IFeatureDefinitionManager definitionManager,
            IRepository<AbpFeature, Guid> repository,
            IMediator mediator,
            IocManager iocManager
        ) : base(repository, iocManager)
        {
            _repository = repository;
            _mediator = mediator;
            _iocManager = iocManager;
            _currentOrganization = currentOrganization;
            _organizationRepository = organizationRepository;
            _provider = provider;
            _definitionManager = definitionManager;

            base.GetAllPermissionName = AppPermissions.Pages.Administration.Default;
            base.CreatePermissionName = AppPermissions.Pages.Administration.Default;
            base.UpdatePermissionName = AppPermissions.Pages.Administration.Default;
            base.DeletePermissionName = AppPermissions.Pages.Administration.Default;

            base.EnableGetEdit = true;
        }

        public virtual async Task<List<FeatureDefinition>> GetAllFeatureDefinitionAsync()
        {
            var definitions = await _mediator.Send(new QueryFeatureDefinition());
            return await Task.FromResult(definitions);
        }

        public override async Task<GetForEditOutput<AbpFeatureDto>> GetForEdit(EntityDto<Guid> input)
        {
            var find = await Repository
                .FirstOrDefaultAsync(z => z.Id == input.Id);

            var schema = JToken.FromObject(new { });

            var definitions = await _mediator.Send(new QueryFeatureDefinition());

            schema["definitions"] = definitions.GetSelection("string", "DisplayName", "Name", _iocManager);

            return new GetForEditOutput<AbpFeatureDto>(
                find != null
                    ? ObjectMapper.Map<AbpFeatureDto>(find)
                    : new AbpFeatureDto
                    {
                        Value = "true",
                        ProviderName = "T",
                        ProviderKey = AbpSession.TenantId.ToString(),
                        DateTimeExpired = DateTime.Today.AddYears(1).AddDays(1).AddMilliseconds(-1),
                        Enable = true
                    },
                schema);
        }

        public override async Task<AbpFeatureDto> CreateAsync(AbpFeatureDto input)
        {
            await CheckSameName(input);
            return await base.CreateAsync(input);
        }

        public override async Task<AbpFeatureDto> UpdateAsync(AbpFeatureDto input)
        {
            await CheckSameName(input);
            return await base.UpdateAsync(input);
        }

        public override async Task<PagedResultDto<AbpFeatureDto>> GetAllAsync(AbpFeatureRequestDto input)
        {
            var dtos = await base.GetAllAsync(input);

            var localCache = new Dictionary<long, string>();

            foreach (var dto in dtos.Items)
            {
                if (dto.ProviderName == "O")
                {
                    var outId = Convert.ToInt64(dto.ProviderKey);

                    if (localCache.ContainsKey(outId))
                    {
                        dto.OrganizationUnit = new BasicOrganizationInfo(
                            Convert.ToInt64(dto.ProviderKey)
                            , localCache[outId]
                        );
                    }
                    else
                    {
                        var ou = await _organizationRepository.FirstOrDefaultAsync(x => x.Id == outId);
                        if (ou != null)
                        {
                            localCache[outId] = ou.DisplayName;
                            dto.OrganizationUnit = new BasicOrganizationInfo(outId, localCache[outId]);
                        }
                    }
                }
            }

            return dtos;
        }

        /// <summary>
        /// 检查是否有同名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task CheckSameName(AbpFeatureDto input)
        {
            if (await Repository.GetAll().AnyAsync(x => x.Name == input.Name &&
                                                        x.ProviderName == input.ProviderName &&
                                                        x.ProviderKey == input.ProviderKey &&
                                                        x.Id != input.Id))
            {
                throw new UserFriendlyException("相同内容已存在");
            }
        }
    }
}