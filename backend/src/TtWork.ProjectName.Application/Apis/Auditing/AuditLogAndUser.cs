using Abp.Auditing;
using TtWork.ProjectName.Authorization.Users;
using TTWork.Abp.Core.Authorization.Users;

namespace TtWork.ProjectName.Auditing
{
    public class AuditLogAndUser
    {
        public AuditLog AuditLog { get; set; }

        public User User { get; set; }
    }
}