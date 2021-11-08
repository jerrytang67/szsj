using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TtWork.ProjectName.Configuration;
using TtWork.ProjectName.Web;

namespace TtWork.ProjectName.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class AbpDbContextFactory : IDesignTimeDbContextFactory<AbpDbContext>
    {
        public AbpDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AbpDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            AbpDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ProjectNameConsts.ConnectionStringName));

            return new AbpDbContext(builder.Options);
        }
    }
}
