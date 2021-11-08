namespace TTWork.Abp.AuditManagement.Events
{
    public class AuditResult
    {
        public int? NextNodeId { get; set; }
        public AuditUserLogStatus Status { get; set; } = AuditUserLogStatus.Reject;
        public long? CreatorUserId { get; set; }
    }

    public enum AuditUserLogStatus
    {
        // 拒绝
        Reject = 0,
        
        // 通过
        Pass = 1,

        //回退
        Back = 2,

        // 等待平级其他节点通过
        Continue = 8
    }
}