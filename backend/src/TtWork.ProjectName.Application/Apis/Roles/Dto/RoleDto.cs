using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.AutoMapper;
using TTWork.Abp.Core.Authorization.Roles;
using TtWork.ProjectName.Authorization.Roles;

namespace TtWork.ProjectName.Roles.Dto
{
    public class RoleDto : EntityDto<int>
    {
        [Required]
        [StringLength(AbpRoleBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpRoleBase.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        public bool IsDefault { get; set; }

        public bool IsStatic { get; set; }


        public string NormalizedName { get; set; }

        [StringLength(Role.MaxDescriptionLength)]
        public string Description { get; set; }

        public List<string> GrantedPermissions { get; set; }
    }
}