using System.Collections.Generic;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Organizations;
using TtWork.ProjectName.Entities.Organizations;
using TtWork.ProjectName.Entities.Shares;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TTWork.Abp.Core.Domains.Weixin;
using TtWork.ProjectName.Entities.Cms;
using TtWork.ProjectName.Entities.Users;

namespace TtWork.ProjectName.EntityFrameworkCore
{
    public static class DbContextModelCreatingExtensions
    {
        public static void ConfigureDb(this ModelBuilder builder)
        {
            builder.Entity<Edition>().HasMany<EditionFeatureSetting>().WithOne(b => b.Edition).IsRequired(false);


            builder.Entity<OrganizationUnit>().HasDiscriminator<int>("OUType")
                .HasValue<OrganizationUnit>(0)
                .HasValue<ProjectNameOrganizationUnit>(1);


            builder.Entity<ProjectNameOrganizationUnit>()
                .OwnsOne(p => p.Detail, ob => ob.ToTable("T_OrganizationUnitDetails"));

            builder.Entity<WechatUserinfo>().HasKey(ba => new {ba.openid, ba.TenantId});

            builder.Entity<WechatUserinfo>()
                .Ignore(x => x.LastModificationTime)
                .Ignore(x => x.LastModifierUser)
                .Ignore(x => x.LastModifierUserId)
                ;


            builder.Entity<UserEvent>(b =>
            {
                // b.ToTable("T_UserEvents");
                b.Property(x => x.Value).HasConversion(
                    v => JsonConvert.SerializeObject(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}),
                    v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
            });


            // builder.Entity<CmsContent>()
            //     .HasMany(b => b.CmsContentEvents).WithOne(p => p.CmsContent).IsRequired(false);

            builder.Entity<QrDetail>(b =>
                {
                    //b.ToTable(MallConsts.DbTablePrefix + "QrDetails", MallConsts.DbSchema);

                    b.Property(x => x.AppName).IsRequired().HasMaxLength(MallConsts.MaxNameLength);
                    b.Property(x => x.EventName).IsRequired().HasMaxLength(MallConsts.MaxNameLength);
                    b.Property(x => x.EventKey).HasMaxLength(MallConsts.MaxImageLength);
                    b.Property(x => x.Path).HasMaxLength(MallConsts.MaxImageLength);
                    b.Property(x => x.QrImageUrl).HasMaxLength(MallConsts.MaxImageLength);

                    b.Property(x => x.Params).HasConversion(
                        v => JsonConvert.SerializeObject(v,
                            new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}),
                        v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v,
                            new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
                }
            );
        }
    }
}