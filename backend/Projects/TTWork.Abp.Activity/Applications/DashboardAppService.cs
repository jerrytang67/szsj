using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.LaborUnion.Applications.Dtos;
using TTWork.Abp.LaborUnion.Domains;

namespace TTWork.Abp.LaborUnion.Applications
{
    public class DashboardAppService : AbpAppServiceBase
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Craftsman, long> _craftsmanRepository;
        private readonly IRepository<CraftsmanRecommend, long> _craftsmanRecommendRepository;

        public DashboardAppService(
            IRepository<User, long> userRepository,
            IRepository<Craftsman, long> craftsmanRepository,
            IRepository<CraftsmanRecommend, long> craftsmanRecommendRepository
        )
        {
            _userRepository = userRepository;
            _craftsmanRepository = craftsmanRepository;
            _craftsmanRecommendRepository = craftsmanRecommendRepository;
        }

        [AbpAuthorize(AppPermissions.Pages.Dashboard.Default)]
        public async Task<object> GetDashboard()
        {
            var userCount = await _userRepository.GetAll().AsNoTracking().CountAsync();

            var craftsmanCount = await _craftsmanRepository.GetAll().AsNoTracking().CountAsync();

            var craftsmanRecommendCount = await _craftsmanRecommendRepository.GetAll().AsNoTracking().CountAsync();

            return new {userCount, craftsmanCount, craftsmanRecommendCount};
        }
    }
}