using System.Threading.Tasks;
using Abp.Application.Services;
using TtWork.ProjectName.Apis.Authorization.Accounts.Dto;
using TtWork.ProjectName.Authorization.Accounts.Dto;

namespace TtWork.ProjectName.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
