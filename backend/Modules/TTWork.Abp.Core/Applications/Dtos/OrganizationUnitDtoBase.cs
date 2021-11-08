using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;

namespace TTWork.Abp.Core.Applications.Dtos
{
    [AutoMapFrom(typeof(OrganizationUnit))]
    public class OrganizationUnitDtoBase : EntityDto<long>
    {
        public string DisplayName { get; set; }
    }
}