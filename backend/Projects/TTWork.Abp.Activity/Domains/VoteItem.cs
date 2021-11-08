using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using TTWork.Abp.AuditManagement.Applications.Dtos;
using TTWork.Abp.AuditManagement.Audits;

namespace TTWork.Abp.Activity.Domains
{
    public class VoteItem : FullAuditedAggregateRoot<Guid>, IMustHaveTenant, INeedAudit
    {
        public int TenantId { get; set; }
        public long VotePlanId { get; set; }
        [ForeignKey("VotePlanId")] public virtual VotePlan VotePlan { get; set; }
        public EnumClass.ListState State { get; set; } = 0;
        public int Sort { get; set; }
        public JObject Form { get; set; } = new() { { "name", "" }, { "address", "" }, { "desc", "" }, { "imageList", new JArray() } };

        #region 审核相关

        public Guid? AuditFlowId { get; set; }
        public int? Audit { get; set; }
        public int? AuditStatus { get; set; }

        [NotMapped]
        public bool IsAudited
        {
            get
            {
                switch (Audit)
                {
                    //未初始化
                    case null:
                    case -1:
                        return false;
                    default:
                        return Audit == AuditStatus;
                }
            }
        }

        [StringLength(64)] public string RejectText { get; set; }

        #endregion
    }

    public class VoteItemDto : EntityDto<Guid>
    {
        public long VotePlanId { get; set; }

        public VotePlanDto VotePlan { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EnumClass.ListState State { get; set; } = 0;

        public int Sort { get; set; }
        public JObject Form { get; set; }

        #region 审核相关

        public Guid? AuditFlowId { get; set; }
        public int? Audit { get; set; }
        public int? AuditStatus { get; set; }
        public bool IsAudited { get; set; }
        public AuditFlowDto AuditFlow { get; set; }
        public List<AuditNodeDto> CurrentAuditNodes { get; set; }
        public string RejectText { get; set; }

        #endregion
    }

    public class VoteItemCreateOrUpdateDto : EntityDto<Guid>, INeedAuditBase
    {
        public long VotePlanId { get; set; }

        public int State { get; set; }
        public int Sort { get; set; }
        public JObject Form { get; set; }
        public Guid? AuditFlowId { get; set; }
    }

    public class VoteItemCreateOrUpdateDtoValidate : AbstractValidator<VoteItemCreateOrUpdateDto>
    {
        public VoteItemCreateOrUpdateDtoValidate()
        {
            RuleFor(x => x.VotePlanId).GreaterThan(0).WithName("活动ID");
        }
    }
}