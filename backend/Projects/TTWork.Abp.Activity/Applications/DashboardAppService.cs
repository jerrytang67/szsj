using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Definitions;

namespace TTWork.Abp.Activity.Applications
{
    public class DashboardAppService : AbpAppServiceBase
    {
        private readonly IRepository<User, long> _userRepository;

        public DashboardAppService(
            IRepository<User, long> userRepository
        )
        {
            _userRepository = userRepository;
        }

        [AbpAuthorize(AppPermissions.Pages.Dashboard.Default)]
        public async Task<object> GetDashboard()
        {
            var userCount = await _userRepository.GetAll().AsNoTracking().CountAsync();

            return new { userCount };
        }
    }
}