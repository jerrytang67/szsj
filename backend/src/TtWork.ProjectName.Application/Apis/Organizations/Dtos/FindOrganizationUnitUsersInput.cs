using TtWork.ProjectName.Apis.Dtos;
using TtWork.ProjectName.Dto;

namespace TtWork.ProjectName.Apis.Organizations.Dtos
{
    public class FindOrganizationUnitUsersInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}