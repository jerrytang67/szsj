using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Organizations;
using Abp.UI;
using TtWork.ProjectName.Apis.Dtos;
using TtWork.ProjectName.Apis.Organizations.Dtos;
using TtWork.ProjectName.Definitions;
using TtWork.ProjectName.Entities.Organizations;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.AuditManagement.Applications;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Definitions;

namespace TtWork.ProjectName.Apis.Organizations
{
    public class OrganizationApplyAppService : AuditAsyncCrudAppService<OrganizationApply, OrganizationApplyDto, long, AppResultRequestDto, CreatorOrUpdateOrganizationApplyDto,
        CreatorOrUpdateOrganizationApplyDto>, IOrganizationApplyAppService
    {
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;

        public OrganizationApplyAppService(
            IRepository<OrganizationApply, long> repository, IocManager iocManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository
        ) : base(repository, iocManager)
        {
            _organizationUnitRepository = organizationUnitRepository;
            base.GetPermissionName = AppPermissions.Pages.Default;
            base.CreatePermissionName = AppPermissions.Pages.Default;
            base.UpdatePermissionName = AppPermissions.Pages.Default;
            base.GetAllPermissionName = AppPermissions.Pages.Administration.Default;
            base.DeletePermissionName = AppPermissions.Pages.Administration.Default;

            base.AuditName = OrganizationAudit.ApplyAprove;
        }

        /// <summary>
        /// 检查是否有同名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task CheckSameName(CreatorOrUpdateOrganizationApplyDto input)
        {
            if (await Repository.GetAll().AnyAsync(x => x.DisplayName == input.DisplayName &&
                                                        x.Id != input.Id))
            {
                throw new UserFriendlyException("相同商户名已存在");
            }
        }

        public override Task<PagedResultDto<OrganizationApplyDto>> GetAllAsync(AppResultRequestDto input)
        {
            return base.GetAllAsync(input);
        }


        public override async Task<OrganizationApplyDto> CreateAsync(CreatorOrUpdateOrganizationApplyDto input)
        {
            await CheckSameName(input);

            var dto = await base.CreateAsync(input);

            if (dto.AuditFlowId.HasValue)
                await base.StartAudit(dto);

            return dto;
        }

        public override async Task<OrganizationApplyDto> UpdateAsync(CreatorOrUpdateOrganizationApplyDto input)
        {
            await CheckSameName(input);

            var dto = await base.UpdateAsync(input);

            // 如果已经审核通过过了,这里会有机构ID,然后就是走编辑审核
            if (dto.OrganizationUnitId.HasValue)
                base.AuditName = OrganizationAudit.EditAprove;

            await StartAudit(dto);

            return dto;
        }

        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<List<OrganizationApplyDto>> GetMyApplyList()
        {
            var query = Repository.GetAll();

            var result = await query
                .Where(x => x.CreatorUserId == AbpSession.UserId
                ).ToListAsync();

            return ObjectMapper.Map<List<OrganizationApplyDto>>(result.Where(x => !x.IsAudited));
        }
    }
}