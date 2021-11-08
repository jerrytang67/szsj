using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.AuditManagement.Events;
using TTWork.Abp.Core.Applications.Dtos;

namespace TTWork.Abp.AuditManagement.Applications.Dtos
{
    /// <summary>
    /// <see cref="AuditUserLog"/>
    /// </summary>
    [AutoMapFrom(typeof(AuditUserLog))]
    public class AuditUserLogDto : EntityDto<long>
    {
        public string AuditName { get; set; }

        public AuditUserLogStatus Status { get; set; }

        public string Desc { get; set; }

        public DateTime CreationTime { get; set; }

        public UserDtoBase CreatorUser { get; set; }

        public bool? Expired { get; set; }

        public Guid AuditFlowId { get; set; }

        public Guid AuditNodeId { get; set; }
    }
}