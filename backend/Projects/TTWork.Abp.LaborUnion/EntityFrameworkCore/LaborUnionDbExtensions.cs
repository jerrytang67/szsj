using Microsoft.EntityFrameworkCore;
using TTWork.Abp.AuditManagement;
using TTWork.Abp.LaborUnion.Domains;

namespace TTWork.Abp.LaborUnion.EntityFrameworkCore
{
    public static class LaborUnionDbExtensions
    {
        public static void ConfigurLaborUnion(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Craftsman>(b =>
                {
                    b.ToTable(LaborUnionConsts.DbTablePrefix + "Craftsman", LaborUnionConsts.DbSchema);
                    b.OwnsOne(p => p.Detail);
                })
                ;


            modelBuilder.Entity<CraftsmanRecommend>(b => { b.ToTable(LaborUnionConsts.DbTablePrefix + "CraftsmanRecommends", LaborUnionConsts.DbSchema); }
            );
        }
    }
}