using System.Collections.Generic;
using TtWork.ProjectName.Apis.Roles.Dto;

namespace TtWork.ProjectName.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}