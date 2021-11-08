using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.Core.Authorization.Users;

namespace TtWork.ProjectName.Entities.Organizations
{
    [Table("T_OrganizationApply")]
    public class OrganizationApply : AggregateRoot<long>, IAudited<User>, IMustHaveTenant, IMayHaveOrganizationUnit, INeedAudit, IExtendableObject
    {
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        public virtual OrganizationUnitDetail Detail { get; set; }

        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public User CreatorUser { get; set; }
        public User LastModifierUser { get; set; }
        public int TenantId { get; set; }

        public long? OrganizationUnitId { get; set; }
        [ForeignKey("OrganizationUnitId")] public virtual OrganizationUnit OrganizationUnit { get; set; }

        #region 审核相关

        public Guid? AuditFlowId { get; set; }
        public int? Audit { get; set; }
        public int? AuditStatus { get; set; }

        [NotMapped]
        public bool IsAudited
        {
            get
            {
                if (!Audit.HasValue) //未初始化
                    return false;
                return Audit == AuditStatus;
            }
        }

        #endregion
        
        [NotMapped]
        public virtual string RefuseText
        {
            set => this.SetData("RefuseText", value);
            get => this.GetData<string>("RefuseText");
        }


        [StringLength(512)] public string ExtensionData { get; set; }
    }
}