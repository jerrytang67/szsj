using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TtWork.ProjectName.Apis.Dtos;
using TtWork.ProjectName.Apis.Organizations.Dtos;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;

namespace TtWork.ProjectName.Apis.Organizations
{
    public interface IOrganizationUnitAppService : IApplicationService
    {
        Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnits(AppResultRequestDto input);

        Task<ListResultDto<OrganizationUnitDto>> GetAllOrganizationUnits(AppResultRequestDto input);

        Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input);
        Task<OrganizationUnitDto> GetCurrent();
        Task<GetForEditOutput<CreateOrganizationUnitInput>> GetForEdit(EntityDto input);

        Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input);

        Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input);

        Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input);

        Task DeleteOrganizationUnit(EntityDto<long> input);

        Task RemoveUserFromOrganizationUnit(UserToOrganizationUnitInput input);

        Task AddUsersToOrganizationUnit(UsersToOrganizationUnitInput input);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindOrganizationUnitUsersInput input);

        Task<string> PostEvent(EntityEventDto<int> input);
    }
}