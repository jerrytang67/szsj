using Microsoft.EntityFrameworkCore;
using TTWork.Abp.Timeline.Domains;

namespace TTWork.Abp.Timeline.EntityFrameworkCore
{
    public interface ITimelineDbContext
    {
        DbSet<TimelineEvent> TimelineEvents { get; set; }
        DbSet<TimelineCategory> TimelineCategories { get; set; }
        DbSet<TimelineFile> TimelineFiles { get; set; }
    }
}