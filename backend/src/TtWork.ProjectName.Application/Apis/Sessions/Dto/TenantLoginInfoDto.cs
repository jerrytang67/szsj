using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TtWork.ProjectName.MultiTenancy;
using TTWork.Abp.Core.MultiTenancy;

namespace TtWork.ProjectName.Apis.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
