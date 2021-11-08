using System.Threading.Tasks;
using Abp.Application.Services;
using TtWork.ProjectName.Apis.Tenants.Dto;

namespace TtWork.ProjectName.Apis.Tenants
{
    public interface ITenantDashboardAppService : IApplicationService
    {
        Task<GetDashboardDataOutput> GetDashboardData(GetDashboardInput input);
    }
}