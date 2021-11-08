using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using TTWork.Abp.Core.Authorization.Users;

namespace TtWork.ProjectName.Entities.Organizations
{
    [Table("T_OrganizationEvents")]
    public class OrganizationEvent : Entity<long>, ICreationAudited<User>, IExtendableObject, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int EventType { get; set; } = 0;
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public User CreatorUser { get; set; }
        public int TenantId { get; set; }
        public long? OrganizationUnitId { get; set; }
        [StringLength(512)] public string ExtensionData { get; set; }
    }
    
    public enum OrganizationEventType
    {
        unknow = 0,
        follow = 2
    }
}