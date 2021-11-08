using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace TTWork.Abp.Activity.Domains
{
    public class UserPointLog : CreationAuditedEntity<long>, IMustHaveTenant
    {
        public long UserId { get; set; }

        public EnumClass.UserPointEventType EventType { get; set; }

        public string EventId { get; set; }
        public int AfterPoints { get; set; }
        public int BeforePoints { get; set; }

        [NotMapped] public int ChangePoints => AfterPoints - BeforePoints;

        public string Desc { get; set; }

        public int TenantId { get; set; }
    }
}