using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using TT.Extensions;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.AuditManagement.Applications;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Timeline.Applications.Dtos;
using TTWork.Abp.Timeline.Definitions;
using TTWork.Abp.Timeline.Domains;

namespace TTWork.Abp.Timeline.Applications
{
    public class TimelineEventAppService : AuditAsyncCrudAppService<TimelineEvent, TimelineEventDto, long, AppResultRequestDto, TimelineEventCreateOrUpdateDto, TimelineEventCreateOrUpdateDto>
    {
        private readonly IRepository<TimelineCategory, long> _categoryRepository;

        public TimelineEventAppService(
            IRepository<TimelineEvent, long> repository,
            IRepository<TimelineCategory, long> categoryRepository,
            IocManager iocManager) : base(repository, iocManager)
        {
            _categoryRepository = categoryRepository;
            EnableGetEdit = true;

            base.AuditName = TimelineAudit.EventPublish;

            // base.GetAllPermissionName = appp.Default;
            base.DeletePermissionName = TimelinePermissions.Admin;
            base.CreatePermissionName = TimelinePermissions.Admin;
            base.UpdatePermissionName = TimelinePermissions.Admin;
        }
        
        public override async Task<GetForEditOutput<TimelineEventCreateOrUpdateDto>> GetForEdit(EntityDto<long> input)
        {
            var result = await base.GetForEdit(input);

            var categories = await _categoryRepository.GetAll().AsNoTracking().OrderBy(x => x.Id).ToListAsync();

            result.Schema["categoryId"] = categories.GetSelection("number", "categoryId", @"{0}", new[] {"Name"}, "Id");

            return result;
        }

        protected override IQueryable<TimelineEvent> ApplyPaging(IQueryable<TimelineEvent> query, AppResultRequestDto input)
        {
            return base.ApplyPaging(query, input);
        }

        protected override IQueryable<TimelineEvent> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                    .Include(x => x.TimelineCategory)
                    .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), x => x.Title.Contains(input.Keyword))
                    .WhereIf(input.Status.HasValue, x => (int) x.State == input.Status!.Value)
                    .WhereIf(input.Pid.HasValue, x => x.CategoryId == input.Pid)
                ;
        }
    }
}