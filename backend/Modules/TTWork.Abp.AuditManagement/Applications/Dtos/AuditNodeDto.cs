using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TTWork.Abp.AuditManagement.Domain;

namespace TTWork.Abp.AuditManagement.Applications.Dtos
{
    /// <summary>
    /// <see cref="AuditNode"/>
    /// </summary>
    [AutoMap(typeof(AuditNode))]
    public class AuditNodeDto : CreationAuditedEntityDto<Guid>
    {
        public string Desc { get; set; }

        public string UserName { get; set; }

        public long UserId { get; set; }

        public int Index { get; set; }

        public Guid AuditFlowId { get; set; }

        public bool ICanAudit { get; set; }

        public bool CanIAudit(in long userId)
        {
            return UserId == userId;
        }
    }
}