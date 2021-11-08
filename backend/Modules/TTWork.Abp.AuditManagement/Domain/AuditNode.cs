using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Abp.Domain.Entities.Auditing;

namespace TTWork.Abp.AuditManagement.Domain
{
    [Table("AuditNodes")]
    public class AuditNode : CreationAuditedEntity<Guid>
    {
        public string Desc { get; protected set; }

        [NotNull] public string UserName { get; set; }
        public long? UserId { get; set; }
        
        public int? RoleId { get; set; }
        [StringLength(64)] public string RoleName { get; set; }

        
        public int Index { get; set; } // 0,1,2

        public Guid AuditFlowId { get; set; }

        [ForeignKey("AuditFlowId")] public virtual AuditFlow AuditFlow { get; set; }


        public virtual bool CanIAudit(List<int> roles, in long userId)
        {
            if (RoleId is >0 && UserId is > 0) return roles.IndexOf(RoleId.Value) >= 0 || UserId.Value == userId;
            if (RoleId is >0) return roles.IndexOf(RoleId.Value) >= 0;
            if (UserId is > 0) return userId == UserId.Value;
            return false;
        }
    }
}