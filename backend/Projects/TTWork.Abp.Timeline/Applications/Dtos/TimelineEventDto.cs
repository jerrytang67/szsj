using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Newtonsoft.Json.Linq;
using TTWork.Abp.AuditManagement.Applications.Dtos;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.Timeline.Domains;

namespace TTWork.Abp.Timeline.Applications.Dtos
{
    /// <summary>
    /// <see cref="TimelineEvent"/>
    /// </summary>
    public class TimelineEventDto : FullAuditedEntityDto<long>, INeedAudit
    {
        public long? CategoryId { get; set; }

        public TimelineCategoryDto TimelineCategory { get; set; }

        public string TitleImageUrl { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public EnumClass.TimelineEventState State { get; set; }

        public int Type { get; set; }

        public string HtmlContext { get; set; }

        public DateTime? DatetimeStart { get; set; }

        public DateTime? DatetimeEnd { get; set; }

        public JObject Settings { get; set; }

        #region 审核相关

        public Guid? AuditFlowId { get; set; }
        public int? Audit { get; set; }
        public int? AuditStatus { get; set; }
        public bool IsAudited { get; set; }

        public AuditFlowDto AuditFlow { get; set; }
        public List<AuditNodeDto> CurrentAuditNodes { get; set; }

        #endregion
    }
}