using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Newtonsoft.Json.Linq;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.Timeline.Domains;

namespace TTWork.Abp.Timeline.Applications.Dtos
{
    /// <summary>
    /// <see cref="TimelineEvent"/>
    /// </summary>
    public class TimelineEventCreateOrUpdateDto : EntityDto<long>, INeedAuditBase
    {
        public long? CategoryId { get; set; }

        [StringLength(256)] public string TitleImageUrl { get; set; }

        [StringLength(256)] public string Title { get; set; }

        [StringLength(512)] public string SubTitle { get; set; }

        public EnumClass.TimelineEventState State { get; set; } = EnumClass.TimelineEventState.草稿;

        public int Type { get; set; }

        public string HtmlContext { get; set; }

        public DateTime? DatetimeStart { get; set; }

        public DateTime? DatetimeEnd { get; set; }

        public JObject Settings { get; set; } = new();

        public Guid? AuditFlowId { get; set; }
    }
}