using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TTWork.Abp.Core.Applications.Dtos;
using TtWork.ProjectName.Apis.Users.Dto;
using TtWork.ProjectName.Roles.Dto;
using TtWork.ProjectName.Users.Dto;

namespace TtWork.ProjectName.Apis.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, AppResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}