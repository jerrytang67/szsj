using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace TTWork.Abp.Timeline.Domains
{
    public class TimelineFileCreateOrUpdateDto : EntityDto<Guid>
    {
        public long? EventId { get; set; }
        public EnumClass.TimelineFileType Type { get; set; }
        public int State { get; set; } = 0;
        public int Sort { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string Desc { get; set; }
    }


    public class TimelineFileDto : CreationAuditedEntityDto<Guid>
    {
        public long? EventId { get; set; }
        public EnumClass.TimelineFileType Type { get; set; }
        public int State { get; set; } = 0;
        public int Sort { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string Desc { get; set; }

        public JObject Data { get; set; } = new();
    }

    public class TimelineFile : FullAuditedAggregateRoot<Guid>, IMustHaveTenant, IMayHaveOrganizationUnit, IExtendableObject
    {
        public TimelineFile()
        {
        }

        public TimelineFile(long? eventId, EnumClass.TimelineFileType type, string url)
        {
            EventId = eventId;
            Type = type;
            Url = url;
        }


        public long? EventId { get; set; }

        [ForeignKey("EventId")] [CanBeNull] public virtual TimelineEvent TimelineEvent { get; set; }

        public EnumClass.TimelineFileType Type { get; set; }

        public int State { get; set; } = 0;

        public int Sort { get; set; }

        [StringLength(256)] public string Url { get; set; }

        [StringLength(256)] public string FileName { get; set; }
        [StringLength(64)] public string MimeType { get; set; }
        [StringLength(256)] public string Desc { get; set; }

        public int TenantId { get; set; }
        public long? OrganizationUnitId { get; set; }
        public string ExtensionData { get; set; }
    }
}