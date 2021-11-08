using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using TTWork.Abp.AuditManagement.Events;
using TTWork.Abp.Core.Authorization.Users;

namespace TTWork.Abp.AuditManagement.Domain
{
    /// <summary>
    /// 用户审核日志
    /// </summary>
    [Table("AuditUserLog")]
    public class AuditUserLog : Entity<long>, ICreationAudited<User>, IMustHaveTenant
    {
        [StringLength(64)] public string HostId { get; set; }

        [StringLength(64)] public string AuditName { get; set; }

        public Guid AuditFlowId { get; set; }

        public Guid AuditNodeId { get; set; }

        public AuditUserLogStatus Status { get; set; }

        [StringLength(512)] public string Desc { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public User CreatorUser { get; set; }

        public int TenantId { get; set; }

        public int? AuditStatus { get; set; }

        /// <summary>
        /// 重新提交审核时判断用于排除非当前流程
        /// </summary>
        public bool? Expired { get; set; }

        public int GetNextAuditNodeIndex()
        {
            return AuditStatus + 1 ?? 0;
        }
    }
}