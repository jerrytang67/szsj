using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TtWork.ProjectName.Entities.Pay;
using TTWork.Abp.AuditManagement.Applications;
using TTWork.Abp.AuditManagement.Applications.Dtos;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.Core.Applications.Dtos;

namespace TtWork.ProjectName.Apis.Pays.Dtos
{
    [AutoMapFrom(typeof(RefundLog))]
    public class RefundLogDto : EntityDto<long>, INeedAudit
    {
        public string BillNo { get; set; }

        public int Price { get; set; }

        public bool IsSuccess { get; set; }

        public DateTime? SuccessTime { get; set; }

        public DateTime CreationTime { get; set; }

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