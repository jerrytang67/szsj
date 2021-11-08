using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.LaborUnion;

namespace TTWork.Abp.Activity.EntityFrameworkCore
{
    public static class ActivityDbExtensions
    {
        public static void ConfigurActivity(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPointLog>(b => { b.ToTable(ActivityConsts.DbTablePrefix + "UserPointLogs", ActivityConsts.DbSchema); })
                ;


            modelBuilder.Entity<PointActivity>(b => { b.ToTable(ActivityConsts.DbTablePrefix + "PointActivities", ActivityConsts.DbSchema); })
                ;


            modelBuilder.Entity<PointActivityUserLog>(b => { b.ToTable(ActivityConsts.DbTablePrefix + "PointActivityUserLogs", ActivityConsts.DbSchema); })
                ;


            modelBuilder.Entity<LuckDraw>(b => { b.ToTable(ActivityConsts.DbTablePrefix + "LuckDraws", ActivityConsts.DbSchema); })
                ;

            modelBuilder.Entity<LuckDrawPrize>(b => { b.ToTable(ActivityConsts.DbTablePrefix + "LuckDrawPrizes", ActivityConsts.DbSchema); })
                ;

            modelBuilder.Entity<UserPrize>(b => { b.ToTable(ActivityConsts.DbTablePrefix + "UserPrizes", ActivityConsts.DbSchema); })
                ;

            modelBuilder.Entity<VotePlan>(b =>
            {
                b.ToTable(ActivityConsts.DbTablePrefix + "VotePlans", ActivityConsts.DbSchema);

                b.Property(x => x.Settings).HasConversion(
                    v => v.ToString(),
                    v => JObject.Parse(v)
                );
            });

            modelBuilder.Entity<VoteItem>(b =>
            {
                b.ToTable(ActivityConsts.DbTablePrefix + "VoteItems", ActivityConsts.DbSchema);
                b.Property(x => x.Form).HasConversion(
                    v => v.ToString(),
                    v => JObject.Parse(v)
                );
                b.HasIndex(x => new { x.State, x.VotePlanId });
            });


            modelBuilder.Entity<UserLuckTime>(b =>
            {
                b.ToTable(ActivityConsts.DbTablePrefix + "UserLuckTimes", ActivityConsts.DbSchema);
                b.HasIndex(x => new { x.Status });
            });
        }
    }
}