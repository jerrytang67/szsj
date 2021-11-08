using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.Core.Authorization.Users;

namespace TTWork.Abp.LaborUnion.Domains
{
    public class Craftsman : FullAuditedEntity<long>, INeedAudit, IMayHaveOrganizationUnit
    {
        public RecomandState State { get; set; } = RecomandState.已提交;
        public CraftsmanDetail Detail { get; set; }

        #region 审核相关

        public Guid? AuditFlowId { get; set; }
        public int? Audit { get; set; }
        public int? AuditStatus { get; set; }

        [ForeignKey("CreatorUserId")] public virtual User CreatorUser { get; set; }

        [StringLength(64)] public string RejectText { get; set; }

        [NotMapped]
        public bool IsAudited
        {
            get
            {
                switch (Audit)
                {
                    //未初始化
                    case null:
                    case -1:
                        return false;
                    default:
                        return Audit == AuditStatus;
                }
            }
        }

        #endregion

        public long? OrganizationUnitId { get; set; }

        #region 红包相关

        public bool RedpacketRecived { get; set; } = false;

        public DateTime? RedpacketRecivedTime { get; set; }

        [Column(TypeName = "decimal(18,2)")] public decimal? Redpacket { get; set; }

        public Guid? SecurityStamp { get; set; } = Guid.NewGuid();

        #endregion
    }
}