using Microsoft.EntityFrameworkCore;
using TTWork.Abp.AuditManagement.Domain;

namespace TTWork.Abp.AuditManagement.EntityFrameworkCore
{
    public static class AuditManagementDbExtensions
    {
        public static void ConfigureAuditManagement(this ModelBuilder builder)
        {
            builder.Entity<AuditUserLog>(
                b => { b.ToTable(AuditManagementConsts.DbTablePrefix + "AuditUserLogs", AuditManagementConsts.DbSchema); });


            builder.Entity<AuditFlow>(
                b =>
                {
                    b.ToTable(AuditManagementConsts.DbTablePrefix + "AuditFlows", AuditManagementConsts.DbSchema);
                    // b.HasMany(b => b.AuditNodes).WithOne(p => p.AuditFlow).IsRequired(false);
                });

            builder.Entity<AuditNode>(
                b => { b.ToTable(AuditManagementConsts.DbTablePrefix + "AuditNodes", AuditManagementConsts.DbSchema); });
        }
    }
}