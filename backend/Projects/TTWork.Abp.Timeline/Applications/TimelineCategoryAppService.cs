using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Timeline.Applications.Dtos;
using TTWork.Abp.Timeline.Definitions;
using TTWork.Abp.Timeline.Domains;

namespace TTWork.Abp.Timeline.Applications
{
    public class TimelineCategoryAppService : AbpAsyncCrudAppService<TimelineCategory, TimelineCategoryDto, long, AppResultRequestDto, TimelineCategoryCreateOrUpdateDto,
        TimelineCategoryCreateOrUpdateDto>
    {
        public TimelineCategoryAppService(
            IRepository<TimelineCategory, long> repository,
            IocManager iocManager) : base(repository, iocManager)
        {
            EnableGetEdit = true;

            base.GetAllPermissionName = TimelinePermissions.Default;
            base.DeletePermissionName = TimelinePermissions.Admin;
            base.CreatePermissionName = TimelinePermissions.Admin;
            base.UpdatePermissionName = TimelinePermissions.Admin;
        }


        public override Task<TimelineCategoryDto> CreateAsync(TimelineCategoryCreateOrUpdateDto input)
        {
            return base.CreateAsync(input);
        }
    }
}