using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Roles;
using TTWork.Abp.Core.Authorization.Roles;
using TtWork.ProjectName.Authorization.Roles;

namespace TtWork.ProjectName.Roles.Dto
{
    public class CreateRoleDto
    {
        [Required]
        [StringLength(AbpRoleBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpRoleBase.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        public string NormalizedName { get; set; }

        public bool IsDefault { get; set; } = false;

        public bool IsStatic { get; set; }


        [StringLength(Role.MaxDescriptionLength)]
        public string Description { get; set; }

        public List<string> GrantedPermissions { get; set; }
        
        public int Id { get; set; }

    }
}
