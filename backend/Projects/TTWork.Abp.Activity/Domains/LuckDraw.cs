using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace TTWork.Abp.Activity.Domains
{
    public class LuckDraw : FullAuditedAggregateRoot<long>, IMustHaveTenant, ILuckDrawBase
    {
        [StringLength(256)] public string TitleImageUrl { get; set; }
        [StringLength(256)] public string Title { get; set; }

        [StringLength(512)] public string SubTitle { get; set; }
        public EnumClass.LuckDrawType Type { get; set; }
        public EnumClass.PickupWay PickupWay { get; set; } = EnumClass.PickupWay.Qr;
        public int? Price { get; set; }

        public int MaxDrawTimes { get; set; } = -1; //不限制
        public int? MaxWinTimes { get; set; }
        public int PrizeTotal { get; set; }
        public int PrizeCount { get; set; }
        public int State { get; set; }

        public string HtmlContext { get; set; }
        public string Settings { get; set; }
        public DateTime DatetimeStart { get; set; }
        public DateTime DatetimeEnd { get; set; }

        public int TenantId { get; set; }

        public DateTime? PrizeExpiredTime { get; set; }

        public virtual ICollection<LuckDrawPrize> LuckDrawPrizes { get; set; } = new List<LuckDrawPrize>();

        [StringLength(512)] public string CheckCodes { get; set; }
    }
}