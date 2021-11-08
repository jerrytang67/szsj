using System.Collections.Generic;
using TtWork.ProjectName.Apis.Organizations.Dtos;
using TtWork.ProjectName.Sessions.Dto;
using JetBrains.Annotations;

namespace TtWork.ProjectName.Apis.Sessions.Dto
{
    public class GetCurrentLoginInformationsOutput
    {
        public ApplicationInfoDto Application { get; set; }
        public UserLoginInfoDto User { get; set; }
        public TenantLoginInfoDto Tenant { get; set; }
        
        public OrganizationUnitDto OrganizationUnit { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
    }
}