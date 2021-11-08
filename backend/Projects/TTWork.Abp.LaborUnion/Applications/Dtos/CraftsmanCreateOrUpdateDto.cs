using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using AutoMapper;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.LaborUnion.Domains;

namespace TTWork.Abp.LaborUnion.Applications.Dtos
{
    public class CraftsmanCreateOrUpdateDto : EntityDto<long>, INeedAuditBase
    {
        public RecomandState State { get; set; }

        public CraftsmanDetail Detail { get; set; } = new();

        public Guid? AuditFlowId { get; set; }

        #region 红包相关

        public bool RedpacketRecived { get; set; }
        
        public decimal? Redpacket { get; set; }

        #endregion
    }
}