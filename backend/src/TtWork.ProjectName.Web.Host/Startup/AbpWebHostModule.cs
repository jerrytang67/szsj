using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using TtWork.ProjectName.Authentication.External;
using TtWork.ProjectName.Configuration;

namespace TtWork.ProjectName.Web.Host.Startup
{
    [DependsOn(typeof(ProjectWebCoreModule),
        typeof(AbpRedisCacheModule)
    )]
    public class ProjectWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ProjectWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }


        public override void PreInitialize()
        {
            Configuration.Caching.UseRedis(options =>
            {
                options.ConnectionString = _appConfiguration["Redis:ConnectionString"];
                options.DatabaseId = _appConfiguration.GetValue<int>("Redis:DatabaseId");
            });

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ProjectWebHostModule).GetAssembly());
            ConfigureTokenAuth();
        }


        private void ConfigureTokenAuth()
        {
            var externalAuthConfig = IocManager.Resolve<IExternalAuthConfiguration>();

            // mini programe loginExt
            externalAuthConfig.Providers.Add(new ExternalLoginProviderInfo(
                WechatMiniProgramAuthProviderApi.ProviderName,
                "", //set in tenantSetting
                "", //set in tenantSetting
                typeof(WechatMiniProgramAuthProviderApi)));

            externalAuthConfig.Providers.Add(new ExternalLoginProviderInfo(
                WechatAuthProviderApi.ProviderName,
                "", //set in tenantSetting
                "", //set in tenantSetting
                typeof(WechatAuthProviderApi)));
        }
    }
}