using Microsoft.EntityFrameworkCore;
using TTWork.Abp.Activity.Domains;

namespace TTWork.Abp.Activity.EntityFrameworkCore
{
    public interface IActivityDbContext
    {
        public DbSet<PointActivity> PointActivities { get; set; }
        public DbSet<PointActivityUserLog> PointActivityUserLogs { get; set; }
        public DbSet<UserPointLog> UserPointLogs { get; set; }

        public DbSet<LuckDraw> LuckDraws { get; set; }
        public DbSet<LuckDrawPrize> LuckDrawPrizes { get; set; }
        public DbSet<UserPrize> UserPrizes { get; set; }
        public DbSet<UserLuckTime> UserLuckTimes { get; set; }
    }
}