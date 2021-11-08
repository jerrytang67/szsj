using System;
using System.Collections.Generic;
using TtWork.ProjectName.Apis.Organizations.Dtos;

namespace TtWork.ProjectName.Users.Dto
{
    public class GetUserForEditOutput
    {
        public string HeadImgUrl { get; set; }

        public UserEditDto User { get; set; }

        public UserRoleDto[] Roles { get; set; }

        public List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        public List<string> MemberedOrganizationUnits { get; set; }

    }
}