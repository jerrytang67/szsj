using System.ComponentModel.DataAnnotations;
using Abp.Organizations;
using TtWork.ProjectName.Entities.Organizations;

namespace TtWork.ProjectName.Apis.Organizations.Dtos
{
    public class UpdateOrganizationUnitInput
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        public int Type { get; set; }


        public OrganizationUnitDetailCreateDto Detail { get; set; }
    }
}