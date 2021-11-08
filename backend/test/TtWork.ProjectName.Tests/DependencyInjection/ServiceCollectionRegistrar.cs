using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using Abp.Dependency;
using Microsoft.Data.Sqlite;
using TtWork.ProjectName.EntityFrameworkCore;
using TTWork.Abp.Core.Identity;

namespace TtWork.ProjectName.Tests.DependencyInjection
{
    public static class ServiceCollectionRegistrar
    {
        public static void Register(IIocManager iocManager)
        {
            RegisterIdentity(iocManager);

            var builder = new DbContextOptionsBuilder<AbpDbContext>();

            var inMemorySqlite = new SqliteConnection("Data Source=:memory:");

            builder
                .UseSqlite(inMemorySqlite)
                .UseTriggers(triggerOptions =>
                {
                    //triggerOptions.AddTrigger<TTWork.Triggers.Timeline.TimelineEventTrigger>();
                    // triggerOptions.AddAssemblyTriggers(typeof(TimelineModule).Assembly);
                });
            // .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryPossibleExceptionWithAggregateOperatorWarning))


            iocManager.IocContainer.Register(
                Component
                    .For<DbContextOptions<AbpDbContext>>()
                    .Instance(builder.Options)
                    .LifestyleSingleton()
            );

            inMemorySqlite.Open();

            new AbpDbContext(builder.Options).Database.EnsureCreated();
        }

        private static void RegisterIdentity(IIocManager iocManager)
        {
            var services = new ServiceCollection();

            IdentityRegistrar.Register(services);

            WindsorRegistrationHelper.CreateServiceProvider(iocManager.IocContainer, services);
        }
    }
}