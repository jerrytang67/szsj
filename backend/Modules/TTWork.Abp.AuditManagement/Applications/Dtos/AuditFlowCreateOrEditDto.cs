using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Castle.Core.Internal;
using TTWork.Abp.AuditManagement.Domain;

namespace TTWork.Abp.AuditManagement.Applications.Dtos
{
    /// <summary>
    /// <see cref="AuditFlow"/>
    /// </summary>
    public class AuditFlowCreateOrEditDto : EntityDto<Guid>, IShouldNormalize
    {
        [Required] public string AuditName { get; set; }
        public bool Enable { get; set; }
        [Required] public string ProviderName { get; set; }
        public string ProviderKey { get; set; }

        public AuditFlowType Type { get; set; } = AuditFlowType.AudtitOne;

        public List<AuditNodeCreateOrEditDto> AuditNodes { get; set; }

        public void Normalize()
        {
            if (ProviderKey.IsNullOrEmpty())
            {
                ProviderKey = null;
            }
        }
    }
}