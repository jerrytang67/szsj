using Abp.Application.Services.Dto;

namespace TtWork.ProjectName.Apis.Organizations.Dtos
{
    public class ProjectNameOrganizationUnitDtoBase : EntityDto<long>
    {
        public string DisplayName { get; set; }
        public string LogoImgUrl { get; set; }
        public string Address { get; set; }
        public string Desc { get; set; }

        public int Status { get; set; }

        public int Type { get; set; }
        public string Phone { get; set; }
    }
}