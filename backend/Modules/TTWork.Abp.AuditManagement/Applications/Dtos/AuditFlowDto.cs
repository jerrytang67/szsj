using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TTWork.Abp.AuditManagement.Domain;

namespace TTWork.Abp.AuditManagement.Applications.Dtos
{
    /// <summary>
    /// <see cref="AuditFlow"/>
    /// </summary>
    [AutoMap(typeof(AuditFlow))]
    public class AuditFlowDto : CreationAuditedEntityDto<Guid>
    {
        public string AuditName { get; set; }
        public string AuditDisplayName { get; set; }
        public bool Enable { get; set; }
        public string ProviderName { get; set; }
        public string ProviderKey { get; set; }
        public int NodesMaxIndex { get; set; }
        public List<AuditNodeDto> AuditNodes { get; set; }

        public AuditFlowType Type { get; set; }
    }
}