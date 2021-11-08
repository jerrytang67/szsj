using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;
using TtWork.ProjectName.Entities.Organizations;
using TTWork.Abp.AuditManagement.Applications.Dtos;
using TTWork.Abp.AuditManagement.Audits;

namespace TtWork.ProjectName.Apis.Organizations.Dtos
{
    [AutoMapFrom(typeof(OrganizationApply))]
    public class OrganizationApplyDto : EntityDto<long>, INeedAudit, IMayHaveOrganizationUnit
    {
        public string DisplayName { get; set; }

        public OrganizationUnitDetailDto Detail { get; set; }

        public int Type { get; set; }

        public long? OrganizationUnitId { get; set; }

        #region 审核相关

        public Guid? AuditFlowId { get; set; }
        public int? Audit { get; set; }
        public int? AuditStatus { get; set; }
        public bool IsAudited { get; set; }
        public AuditFlowDto AuditFlow { get; set; }
        public List<AuditNodeDto> CurrentAuditNodes { get; set; }

        #endregion

        public DateTime CreationTime { get; set; }
        
        public long? CreatorUserId { get; set; }
        
        public string RefuseText { get; set; }
    }
}