using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.Timeline.Applications;
using TTWork.Abp.Timeline.Applications.Dtos;

namespace TTWork.Abp.Timeline.Domains
{
    /// <summary>
    /// <see cref="TimelineEventDto"/>
    /// <see cref="TimelineEventCreateOrUpdateDto"/>
    /// </summary>
    public class TimelineEvent : FullAuditedAggregateRoot<long>, IMustHaveTenant, IMayHaveOrganizationUnit, IExtendableObject, INeedAudit
    {
        public long? CategoryId { get; set; }

        [ForeignKey("CategoryId")] [CanBeNull] public virtual TimelineCategory TimelineCategory { get; set; }

        [StringLength(256)] public string TitleImageUrl { get; set; }

        [StringLength(256)] public string Title { get; set; }

        [StringLength(512)] public string SubTitle { get; set; }

        public EnumClass.TimelineEventState State { get; set; } = EnumClass.TimelineEventState.草稿;

        public int Type { get; set; }

        public string HtmlContext { get; set; }

        public DateTime? DatetimeStart { get; set; }

        public DateTime? DatetimeEnd { get; set; }

        public JObject Settings { get; set; } = new();

        public int TenantId { get; set; }

        public long? OrganizationUnitId { get; set; }

        public string ExtensionData { get; set; }
        
        public virtual ICollection<TimelineFile> TimelineFiles { get; set; }

        #region 审核相关

        public Guid? AuditFlowId { get; set; }
        public int? Audit { get; set; }
        public int? AuditStatus { get; set; }

        [NotMapped]
        public bool IsAudited
        {
            get
            {
                if (!Audit.HasValue) //未初始化
                    return false;
                return Audit == AuditStatus;
            }
        }

        #endregion
    }
}