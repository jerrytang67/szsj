using Abp.AutoMapper;
using TtWork.ProjectName.Entities.Organizations;

namespace TtWork.ProjectName.Apis.Organizations.Dtos
{
    [AutoMapFrom(typeof(OrganizationUnitDetail))]
    public class OrganizationUnitDetailDto : OrganizationUnitDetailDtoBase
    {
    }

    [AutoMapFrom(typeof(OrganizationUnitDetail))]
    public class OrganizationUnitDetailDtoBase
    {
        public string Desc { get; set; }

        public string LogoImgUrl { get; set; }

        public string Address { get; set; }

        public string HeadmanRealName { get; set; }

        public string HeadmanPhone { get; set; }

        public string Province { get; set; }


        public string City { get; set; }

        public int? CityId { get; set; }

        public string District { get; set; }

        public double? Lng { get; set; }

        public double? Lat { get; set; }
    }
}