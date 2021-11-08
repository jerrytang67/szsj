using System.Threading.Tasks;
using Abp.Application.Services;
using TtWork.ProjectName.Apis.Sessions.Dto;
using TtWork.ProjectName.Sessions.Dto;

namespace TtWork.ProjectName.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
