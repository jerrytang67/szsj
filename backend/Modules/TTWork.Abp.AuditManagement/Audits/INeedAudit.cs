using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTWork.Abp.AuditManagement.Audits
{
    public interface INeedAudit : INeedAuditBase
    {
        //需要审批的状态 Audit >0为正常开始审核状态,-1为被拒绝,null为未提交审核
        public int? Audit { get; set; }

        //现在审批的状态
        public int? AuditStatus { get; set; }

        [NotMapped]
        bool IsAudited
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
    }

    public interface INeedAuditBase
    {
        public Guid? AuditFlowId { get; set; }
    }
}