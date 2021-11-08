using System.ComponentModel.DataAnnotations;
using Abp.Organizations;
using TtWork.ProjectName.Entities.Organizations;

namespace TtWork.ProjectName.Apis.Organizations.Dtos
{
    public class CreateOrganizationUnitInput
    {
        public int Id { get; set; }
        public long? ParentId { get; set; }

        public int Type { get; set; }

        [Required(ErrorMessage = "机构名称是必填的")]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        public OrganizationUnitDetailCreateDto Detail { get; set; } = new OrganizationUnitDetailCreateDto();

        public string formId { get; set; }

        public string openid { get; set; }
    }
}