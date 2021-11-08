using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;
using Abp.Runtime.Validation;
using TtWork.ProjectName.Entities.Organizations;
using TTWork.Abp.AuditManagement.Audits;
using TtWork.ProjectName.Apis.Organizations.Dtos;

namespace TtWork.ProjectName.Apis.Organizations
{
    [AutoMapTo(typeof(OrganizationApply))]
    public class CreatorOrUpdateOrganizationApplyDto : EntityDto<long>, IShouldNormalize, INeedAuditBase
    {
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        public int Type { get; set; }

        public OrganizationUnitDetailCreateDto Detail { get; set; }
        public Guid? AuditFlowId { get; set; }

        public void Normalize()
        {
        }
    }
}