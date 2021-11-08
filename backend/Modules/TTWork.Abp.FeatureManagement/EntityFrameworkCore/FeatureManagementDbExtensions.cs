using Microsoft.EntityFrameworkCore;
using TTWork.Abp.FeatureManagement.Domain;

namespace TTWork.Abp.FeatureManagement.EntityFrameworkCore
{
    public static class FeatureManagementDbExtensions
    {
        public static void ConfigureFeatureManagement(this Microsoft.EntityFrameworkCore.ModelBuilder builder)
        {
            builder.Entity<AbpFeature>(
                b =>
                {
                    b.ToTable(FeatureManagementConsts.DbTablePrefix + "AbpFeatures", FeatureManagementConsts.DbSchema);

                    b.Property(x => x.Name).IsRequired().HasMaxLength(FeatureManagementConsts.MaxNameLength);

                    b.Property(x => x.Value).HasMaxLength(FeatureManagementConsts.MaxValueLength);
                });
        }
    }
}