using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TtWork.ProjectName.Entities.Organizations;

namespace TtWork.ProjectName.Apis.Organizations.Dtos
{
    /// <summary>
    /// db entity is <see cref="ProjectNameOrganizationUnit"/>
    /// </summary>
    public class OrganizationUnitDto : AuditedEntityDto<long>
    {
        public long? ParentId { get; set; }

        public string Code { get; set; }

        public int Status { get; set; }

        public int Type { get; set; }

        public string DisplayName { get; set; }

        public int MemberCount { get; set; }

        public string RefuseContent { get; set; }

        public OrganizationUnitDetailDto Detail { get; set; }

        public List<int> UserEvents { get; set; } = new List<int>();
    }
}