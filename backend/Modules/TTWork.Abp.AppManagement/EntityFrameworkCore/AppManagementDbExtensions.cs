using System.Collections.Generic;
using Abp.Json;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.AppManagement.Domain;
using TTWork.Abp.Core.Extensions;

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
                    v => v.ToJsonString(false, false),
                    v => v.FromJsonStringExt<Dictionary<string, string>>()
                );

                b.Property(x => x.ProviderKey).HasMaxLength(AppManagementConsts.ProviderKeyLength);
                b.Property(x => x.ProviderName).HasMaxLength(AppManagementConsts.ProviderNameLength);
            });
        }
    }
}