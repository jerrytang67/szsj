using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace TTWork.Abp.Activity.Domains
{
    public class UserLuckTime : FullAuditedEntity<long>, IMustHaveTenant
    {
        [StringLength(128)] public string Host { get; set; }
        [StringLength(128)] public string HostId { get; set; }

        public int TenantId { get; set; }
        public long UserId { get; set; }
        public long LuckDrawId { get; set; }
        public EnumClass.UserLuckTimeStatus Status { get; set; }
        public long? ShareFrom { get; set; } //获取分享来自用户Id
        public DateTime? LuckDrawTime { get; set; } //抽奖时间
        public long? UserPrizeId { get; set; } //中奖信息
        public virtual UserPrize UserPrize { get; set; } = null;
    }

    public class UserLuckTimeDto : FullAuditedEntityDto<long>
    {
        public string Host { get; set; }
        public string HostId { get; set; }

        public long UserId { get; set; }
        public long LuckDrawId { get; set; }
        public EnumClass.UserLuckTimeStatus Status { get; set; }
        public long? ShareFrom { get; set; } //获取分享来自用户Id
        public long? UserPrizeId { get; set; } //中奖信息
        public virtual UserPrizeDto UserPrize { get; set; }
    }
}