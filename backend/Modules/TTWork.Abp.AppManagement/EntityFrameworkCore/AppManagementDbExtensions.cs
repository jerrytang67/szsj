using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.AppManagement.Domain;

namespace TTWork.Abp.AppManagement.EntityFrameworkCore
{
    public static class AppManagementDbExtensions
    {
        public static void ConfigureAppManagement(this ModelBuilder builder)
        {
            builder.Entity<App>(b =>
            {
                b.ToTable(AppManagementConsts.DbTablePrefix + "Apps", AppManagementConsts.DbSchema);

                b.Property(x => x.Name).IsRequired().HasMaxLength(AppManagementConsts.MaxNameLength);
                b.Property(x => x.ClientName).IsRequired().HasMaxLength(AppManagementConsts.MaxNameLength);
                b.Property(x => x.Value).HasConversion(
                    v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, null));

                b.Property(x => x.ProviderKey).HasMaxLength(AppManagementConsts.ProviderKeyLength);
                b.Property(x => x.ProviderName).HasMaxLength(AppManagementConsts.ProviderNameLength);
            });
        }
    }
}