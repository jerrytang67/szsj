using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TTWork.Abp.Timeline.Domains;

namespace TTWork.Abp.Timeline.EntityFrameworkCore
{
    public static class TimelineDbExtensions
    {
        public static void ConfigurTimeline(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimelineEvent>(b =>
            {
                b.ToTable(TimelineConsts.DbTablePrefix + "TimelineEvents", TimelineConsts.DbSchema);
                b.Property(x => x.Settings).HasConversion(
                    v => v.ToString(),
                    v => JObject.Parse(v)
                );
            });

            modelBuilder.Entity<TimelineCategory>(b => { b.ToTable(TimelineConsts.DbTablePrefix + "TimelineCategories", TimelineConsts.DbSchema); });


            modelBuilder.Entity<TimelineFile>(b => { b.ToTable(TimelineConsts.DbTablePrefix + "TimelineFiles", TimelineConsts.DbSchema); });
        }
    }
}