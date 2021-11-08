using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace TTWork.Abp.QA.Domains
{
    // ReSharper disable once InconsistentNaming
    public class QAPlan : FullAuditedAggregateRoot<long>, IMustHaveTenant
    {
        [StringLength(256)] public string TitleImageUrl { get; set; }
        [StringLength(256)] public string Title { get; set; }

        [StringLength(512)] public string SubTitle { get; set; }

        public EnumClass.QAPlanState State { get; set; } = EnumClass.QAPlanState.开启;

        public EnumClass.QAPlanType Type { get; set; }
        public long? LuckDrawId { get; set; } //对应Type==2时

        public string HtmlContext { get; set; }

        public int ViewCount { get; private set; }

        public int HelpPerDay { get; set; } = 1; //每日助力次数

        public int AnswerPerDay { get; set; } = 1; //每日最多答题次数

        public int SharePoints { get; set; }

        public int QuestionCount { get; set; }

        public DateTime DatetimeStart { get; set; }

        public DateTime DatetimeEnd { get; set; }

        [StringLength(16)] [CanBeNull] public string TimeStart { get; set; }

        [StringLength(16)] [CanBeNull] public string TimeEnd { get; set; }
        public JObject Settings { get; set; }

        public List<PointRule> PointRules { get; set; }


        public int TenantId { get; set; }

        public virtual ICollection<QAQuestion> Questions { get; set; }

        public void Visit()
        {
            ViewCount++;
        }
    }


    public class PointRule
    {
        public int Count { get; set; }

        public int? Points { get; set; }

        public int? LuckTime { get; set; }
    }
}