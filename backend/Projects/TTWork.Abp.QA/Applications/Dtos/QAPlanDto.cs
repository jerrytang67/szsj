using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using TTWork.Abp.QA.Domains;

namespace TTWork.Abp.QA.Applications.Dtos
{
    // ReSharper disable once InconsistentNaming
    public class QAPlanDto : FullAuditedEntityDto<long>
    {
        [StringLength(256)] public string TitleImageUrl { get; set; }

        [StringLength(256)] public string Title { get; set; }

        [StringLength(512)] public string SubTitle { get; set; }

        public EnumClass.QAPlanState State { get; set; } = EnumClass.QAPlanState.开启;

        public EnumClass.QAPlanType Type { get; set; }
        public long? LuckDrawId { get; set; } //对应Type==2时

        public string HtmlContext { get; set; }

        public int ViewCount { get; init; }

        public int AnswerPerDay { get; set; }
        public int HelpPerDay { get; set; }

        public int SharePoints { get; set; }

        public JObject Settings { get; set; }

        public List<PointRule> PointRules { get; set; }

        public int QuestionCount { get; set; }

        public DateTime DatetimeStart { get; set; }

        public DateTime DatetimeEnd { get; set; }

        [StringLength(16)] [CanBeNull] public string TimeStart { get; set; }

        [StringLength(16)] [CanBeNull] public string TimeEnd { get; set; }
    }
}