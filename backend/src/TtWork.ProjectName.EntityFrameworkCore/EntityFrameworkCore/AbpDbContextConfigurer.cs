using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TtWork.ProjectName.EntityFrameworkCore
{
    public static class AbpDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AbpDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString)
                .ConfigureWarnings(warnings =>
                {
                    warnings.Throw(RelationalEventId.QueryPossibleExceptionWithAggregateOperatorWarning);
                    // warnings.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS);
                })
                //.EnableSensitiveDataLogging()
                ;
            ;
        }

        public static void Configure(DbContextOptionsBuilder<AbpDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection)
                .ConfigureWarnings(warnings =>
                {
                    warnings.Throw(RelationalEventId.QueryPossibleExceptionWithAggregateOperatorWarning);
                    // warnings.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS);
                })
                //.EnableSensitiveDataLogging()
                ;
            ;
        }
    }
}