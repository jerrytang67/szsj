using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace TTWork.Abp.Activity.Domains
{
    public class PointActivityUserLog : FullAuditedAggregateRoot<long>, IMustHaveTenant
    {
        public PointActivityUserLog(long activityId, long? sharedFrom, int point)
        {
            ActivityId = activityId;
            SharedFrom = sharedFrom;
            Point = point;
        }

        public long ActivityId { get; init; }

        public int HelperNum { get; set; }

        public int Point { get; private set; }

        public EnumClass.PointActivityUserState State { get; set; }
        public long? SharedFrom { get; init; }

        [StringLength(64)] public string openid { get; init; }
        [StringLength(64)] public string nickname { get; init; }
        [StringLength(256)] public string headimgurl { get; init; }

        [ForeignKey("ActivityId")] public PointActivity PointActivity { get; set; }

        public int TenantId { get; set; }
    }
}