using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using TTWork.Abp.Activity;
using TTWork.Abp.WorkFlowCore;
using TtWork.ProjectName.EntityFrameworkCore;
using TtWork.ProjectName.EntityFrameworkCore.Seed;

namespace TtWork.ProjectName
{
    [DependsOn(
            typeof(ProjectCoreModule),
            typeof(WorkFlowCoreModule),
            typeof(ActivityModule),
            typeof(AbpZeroCoreEntityFrameworkCoreModule)
        )
    ]
    public class ProjectEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<AbpDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        AbpDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        AbpDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ProjectEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}