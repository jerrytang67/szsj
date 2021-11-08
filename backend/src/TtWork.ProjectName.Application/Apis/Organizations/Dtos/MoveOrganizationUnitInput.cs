using System.ComponentModel.DataAnnotations;

namespace TtWork.ProjectName.Apis.Organizations.Dtos
{
    public class MoveOrganizationUnitInput
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        public long? NewParentId { get; set; }
    }
}