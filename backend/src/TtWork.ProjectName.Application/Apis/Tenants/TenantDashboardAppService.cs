using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using TtWork.ProjectName.Entities.Organizations;
using TtWork.ProjectName.Entities.Pay;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Definitions;
using TtWork.ProjectName.Apis.Tenants.Dto;
using TtWork.ProjectName.Entities.Cms;

namespace TtWork.ProjectName.Apis.Tenants
{
    [DisableAuditing]
    public class TenantDashboardAppService : AbpAppServiceBase, ITenantDashboardAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<PayOrder, long> _payOrdeRepository;
        private readonly IRepository<CmsContent> _contentRepository;
        private readonly IRepository<ProjectNameOrganizationUnit, long> _orgRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;


        public TenantDashboardAppService(
            IRepository<PayOrder, long> payOrdeRepository,
            IRepository<User, long> userRepository, IRepository<CmsContent> contentRepository,
            IRepository<ProjectNameOrganizationUnit, long> orgRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository
        )
        {
            _payOrdeRepository = payOrdeRepository;
            _userRepository = userRepository;
            _contentRepository = contentRepository;
            _orgRepository = orgRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
        }


        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<GetDashboardDataOutput> GetDashboardData(GetDashboardInput input)
        {
            var ouId = AbpSession.GetCurrentOu();

            var currentTime = DateTime.Today;

            DateTime date;
            switch (input.SalesSummaryDatePeriod)
            {
                case SalesSummaryDatePeriod.Daily:
                    date = currentTime;
                    break;
                case SalesSummaryDatePeriod.Weekly:
                    // 以周一为一周的开始
                    date = currentTime.AddDays(1 - currentTime.DayOfWeek == DayOfWeek.Sunday ? 7 : Convert.ToInt32(currentTime.DayOfWeek));
                    break;
                case SalesSummaryDatePeriod.Monthly:
                    date = new DateTime(currentTime.Year, currentTime.Month, 1);
                    break;
                default:
                    date = currentTime;
                    break;
            }

            var userId = (await GetCurrentUserAsync()).Id;

            var myOus = await _userOrganizationUnitRepository.GetAll().Where(x => x.UserId == userId)
                .Select(x => x.OrganizationUnitId).ToListAsync();

            var isAdmin = await IsAdmin();

            var output = new GetDashboardDataOutput
            {
                NewUsers = await _userRepository.GetAll().Where(z => z.CreationTime >= date).CountAsync(),
            };

            return output;
        }
    }
}