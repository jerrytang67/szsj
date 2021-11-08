using Microsoft.EntityFrameworkCore;
using TTWork.Abp.AuditManagement.Domain;

namespace TTWork.Abp.AuditManagement.EntityFrameworkCore
{
    public interface IAuditDbContext
    {
        public DbSet<AuditFlow> AuditFlows { get; set; }

        public DbSet<AuditNode> AuditNodes { get; set; }

        public DbSet<AuditUserLog> AuditUserLogs { get; set; }
    }
}