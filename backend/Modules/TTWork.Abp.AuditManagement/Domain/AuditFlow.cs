using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;
using TTWork.Abp.AuditManagement.Events.Queries;

namespace TTWork.Abp.AuditManagement.Domain
{
    [Table("AuditFlows")]
    public class AuditFlow : FullAuditedAggregateRoot<Guid>
    {
        protected AuditFlow()
        {
        }

        public AuditFlow(
            [NotNull] string auditName,
            bool enable,
            [NotNull] string providerName,
            [CanBeNull] string providerKey,
            ICollection<AuditNode> auditNodes = null)
        {
            AuditName = auditName;
            Enable = enable;
            ProviderName = providerName;
            ProviderKey = providerKey;
            AuditNodes = auditNodes ?? new List<AuditNode>();
        }

        [NotNull] [StringLength(64)] public virtual string AuditName { get; protected set; }

        //Same AuditName and ProviderName only 1 Entity Enable
        public virtual bool Enable { get; protected set; }

        public virtual AuditFlowType Type { get; set; } = AuditFlowType.AudtitOne;

        //This field "G","T","S"
        [NotNull] [StringLength(1)] public virtual string ProviderName { get; protected set; }
        [CanBeNull] [StringLength(64)] public virtual string ProviderKey { get; protected set; }

        public int NodesMaxIndex { get; set; }

        // one to many entities
        public virtual ICollection<AuditNode> AuditNodes { get; protected set; }
    }
}