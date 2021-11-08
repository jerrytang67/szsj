using System;
using Abp.Application.Services.Dto;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.LaborUnion.Domains;

namespace TTWork.Abp.LaborUnion.Applications.Dtos
{
    public class CraftsmanRecommendCreateOrUpdateDto : EntityDto<long>, INeedAuditBase
    {
        public virtual CraftsmanRecommendDetail Detail { get; set; } = new();

        public Guid? AuditFlowId { get; set; }
    }
}