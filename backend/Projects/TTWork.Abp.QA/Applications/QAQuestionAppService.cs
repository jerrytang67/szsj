using System;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TT.Extensions;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.QA.Definitions;
using TTWork.Abp.QA.Domains;

namespace TTWork.Abp.QA.Applications
{
    // ReSharper disable once InconsistentNaming
    public class QAQuestionAppService : AbpAsyncCrudAppService<QAQuestion, QAQuestionDto, Guid, AppResultRequestDto, QAQuestionCreateOrUpdateDto, QAQuestionCreateOrUpdateDto>
    {
        private readonly IMediator _mediator;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<QAPlan, long> _planRepository;

        public QAQuestionAppService(
            IMediator mediator,
            IGuidGenerator guidGenerator,
            IRepository<QAQuestion, Guid> repository,
            IRepository<QAPlan, long> planRepository,
            IocManager iocManager) : base(repository, iocManager)
        {
            _mediator = mediator;
            _guidGenerator = guidGenerator;
            _planRepository = planRepository;
            EnableGetEdit = true;

            base.GetAllPermissionName = QAPermissions.Default;
            base.DeletePermissionName = QAPermissions.Admin;
            base.CreatePermissionName = QAPermissions.Admin;
            base.UpdatePermissionName = QAPermissions.Admin;
        }

        public override async Task<GetForEditOutput<QAQuestionCreateOrUpdateDto>> GetForEdit(EntityDto<Guid> input)
        {
            var result = await base.GetForEdit(input);

            var list = await _planRepository.GetAll().AsNoTracking().OrderByDescending(x => x.Id).ToListAsync();
            result.Schema["planIds"] = list.GetSelection("number", "planId", @"{0}", new[] {"Title"}, "Id");
            return result;
        }

        public override Task<PagedResultDto<QAQuestionDto>> GetAllAsync(AppResultRequestDto input)
        {
            return base.GetAllAsync(input);
        }

        public override async Task<QAQuestionDto> CreateAsync(QAQuestionCreateOrUpdateDto input)
        {
            input.Id = _guidGenerator.Create();
            return await base.CreateAsync(input);
        }

        protected override IQueryable<QAQuestion> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), x => x.Title.Contains(input.Keyword));
        }
    }
}