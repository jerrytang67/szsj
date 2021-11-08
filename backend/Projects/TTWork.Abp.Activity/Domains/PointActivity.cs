using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

namespace TTWork.Abp.Activity.Domains
{
    public class PointActivity : FullAuditedAggregateRoot<long>, IMustHaveTenant
    {
        [StringLength(256)] public string TitleImageUrl { get; set; }
        [StringLength(64)] public string Title { get; set; }

        [StringLength(512)] public string SubTitle { get; set; }

        public int State { get; set; }

        public string HtmlContext { get; set; }

        public int ViewCount { get; init; }

        // 一天可以帮助几次
        public int HelpPerDay { get; set; }

        public string Settings { get; set; }

        public DateTime DatetimeStart { get; set; }

        public DateTime DatetimeEnd { get; set; }

        [StringLength(16)] [CanBeNull] public string TimeStart { get; set; }

        [StringLength(16)] [CanBeNull] public string TimeEnd { get; set; }

        public int MinPoint { get; set; }

        public int MaxPoint { get; set; }

        public int TenantId { get; set; }
        
    }
}