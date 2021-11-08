using System;
using System.Net.Http;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using NSubstitute;
using Abp.AutoMapper;
using Abp.Castle.Logging.Log4Net;
using Abp.Configuration;
using Abp.Modules;
using Abp.Configuration.Startup;
using Abp.TestBase;
using Abp.Zero.Configuration;
using Abp.Zero.EntityFrameworkCore;
using TtWork.ProjectName.Tests.DependencyInjection;
using TtWork.ProjectName.Configuration;
using TtWork.ProjectName.EntityFrameworkCore;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using TT.HttpClient.Weixin;
using TTWork.Abp.AuditManagement;
using TTWork.Abp.Core.Applications.Wechat;
using TTWork.Abp.Core.Organizations;
using TTWork.Abp.Core.Oss;
using TTWork.Abp.FeatureManagement;
using TTWork.Abp.Oss.UpYun;
using WorkflowCore.Interface;

namespace TtWork.ProjectName.Tests
{
    [DependsOn(
        typeof(ProjectApplicationModule),
        typeof(AuditManagementModule),
        typeof(FeatureManagementModule),
        typeof(ProjectEntityFrameworkModule),
        typeof(AbpTestBaseModule)
    )]
    public class AbpTestModule : AbpModule
    {
        public AbpTestModule(ProjectEntityFrameworkModule projectProjectNameEntityFrameworkModule)
        {
            projectProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
            projectProjectNameEntityFrameworkModule.SkipDbSeed = true;
        }

        public override void PreInitialize()
        {
            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            Configuration.UnitOfWork.IsTransactional = false;

            // Disable static mapper usage since it breaks unit tests (see https://github.com/aspnetboilerplate/aspnetboilerplate/issues/2052)
            Configuration.Modules.AbpAutoMapper().UseStaticMapper = false;

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            RegisterFakeService<AbpZeroDbMigrator<AbpDbContext>>();

            RegisterFakeService<HttpClient>();
            RegisterFakeService<IPayApi>();
            RegisterFakeService<IWeixinApi>();

            RegisterFakeService<IConfiguration>();
            RegisterFakeService<IWorkflowRegistry>();


            // RegisterFakeService<IAbpAspNetCoreConfiguration>();

            Configuration.ReplaceService<IAppConfigurationAccessor, TestAppConfigurationAccessor>();

            Configuration.ReplaceService<WeixinManger, TestWeixinManger>();
            Configuration.ReplaceService<IUpyunApi, TestUpyunApi>();

            Configuration.ReplaceService<IOssClient, TestUpYunClient>();

            // Configuration.ReplaceService<ICurrentOrganizationAccessor, AsyncLocalCurrentShopAccessor>();

            Configuration.ReplaceService<ICurrentOrganization, CurrentOrganization>();

            IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config")
            );

            RegisterFakeService<ApplicationPartManager>();

        }

        public override void Initialize()
        {
            ServiceCollectionRegistrar.Register(IocManager);
        }

        private void RegisterFakeService<TService>() where TService : class
        {
            IocManager.IocContainer.Register(
                Component.For<TService>()
                    .UsingFactoryMethod(() => Substitute.For<TService>())
                    .LifestyleSingleton()
            );
        }
    }

    public class TestUpYunClient : UpYunClient
    {
        public TestUpYunClient(ISettingManager settingManager, IUpyunApi upyunApi) : base(settingManager, upyunApi)
        {
        }

        public override Task<bool> writeFile(string path, byte[] data, bool auto_mkdir)
        {
            return Task.FromResult(true);
        }
    }
}