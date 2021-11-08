using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TTWork.Abp.AuditManagement.Applications.Dtos;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.LaborUnion.Domains;

namespace TTWork.Abp.LaborUnion.Applications.Dtos
{
    public class CraftsmanRecommendDto : CreationAuditedEntityDto<long>, INeedAuditBase
    {
        public RecomandState State { get; set; }

        public virtual CraftsmanRecommendDetail Detail { get; set; } = new();

        public bool RedpacketRecived { get; set; }

        public DateTime? RedpacketRecivedTime { get; set; }

        public decimal? Redpacket { get; set; }

        public string RejectText { get; set; }

        public UserDtoBase CreatorUser { get; set; }

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