using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;

namespace TTWork.Abp.Timeline.Domains
{
    public class TimelineCategory : CreationAuditedEntity<long>, IMustHaveTenant, IMayHaveOrganizationUnit, IExtendableObject
    {
        [StringLength(64)] public string Name { get; set; }

        [StringLength(256)] public string ImageUrl { get; set; }

        public int TenantId { get; set; }
        
        public long? OrganizationUnitId { get; set; }
        
        public string ExtensionData { get; set; }
    }
}