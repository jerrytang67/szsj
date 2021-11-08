using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TtWork.ProjectName.Configuration;
using TtWork.ProjectName.EntityFrameworkCore;
using TtWork.ProjectName.Migrator.DependencyInjection;

namespace TtWork.ProjectName.Migrator
{
    [DependsOn(typeof(ProjectEntityFrameworkModule))]
    public class AbpMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public AbpMigratorModule(ProjectEntityFrameworkModule projectProjectNameEntityFrameworkModule)
        {
            projectProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(AbpMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                ProjectNameConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
