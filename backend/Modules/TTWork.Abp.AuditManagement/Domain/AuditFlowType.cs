using System.ComponentModel.DataAnnotations;

namespace TTWork.Abp.AuditManagement.Domain
{
    public enum AuditFlowType
    {
        [Display(Name = "当前层级有一人同意就通过")] AudtitOne = 0,

        [Display(Name = "当前层级全部人同意才通知")] AuditAll = 1
    }
}