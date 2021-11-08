using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using TtWork.ProjectName.Authentication.JwtBearer;
using TtWork.ProjectName.Configuration;
using TtWork.ProjectName.EntityFrameworkCore;
using TTWork.Abp.AppManagement;
using TTWork.Abp.AuditManagement;
using TTWork.Abp.Core;

namespace TtWork.ProjectName
{
    [DependsOn(
        typeof(ProjectApplicationModule),
        typeof(ProjectEntityFrameworkModule),
        typeof(AbpAspNetCoreModule)
    )]
    public class ProjectWebCoreModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ProjectWebCoreModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            // Clock.Provider = ClockProviders.Local;

            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                ProjectNameConsts.ConnectionStringName
            );

            // 使用netcore AddNewtonsoftJson中的时间格式
            Configuration.Modules.AbpAspNetCore().UseMvcDateTimeFormatForAppServices = true;
            
            
            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(ProjectApplicationModule).GetAssembly());
            
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(TTWorkAbpCoreModule).Assembly);

            // 审核管理模块API注册
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(AuditManagementModule).Assembly, moduleName: "AuditManagement", useConventionalHttpVerbs: true);

            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(AppManagementModule).Assembly, moduleName: "AppManagement", useConventionalHttpVerbs: true);


// #if DEBUG
//             Configuration.Modules.AbpWebCommon().SendAllExceptionsToClients = true;
// #else
//             Configuration.Modules.AbpWebCommon().SendAllExceptionsToClients = false;
// #endif

            //Configuration for a specific cache
            Configuration.Caching.Configure("wechat", cache => { cache.DefaultSlidingExpireTime = TimeSpan.FromHours(1); });

            ConfigureTokenAuth();

            Configuration.ReplaceService<IAppConfigurationAccessor, AppConfigurationAccessor>();
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();

            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(Convert.ToDouble(_appConfiguration["Authentication:JwtBearer:Day"]));
            tokenAuthConfig.AccessTokenExpiration = TimeSpan.FromDays(int.Parse(_appConfiguration["Authentication:JwtBearer:ExpirationDay"]));

            tokenAuthConfig.RefreshTokenExpiration = AppConsts.RefreshTokenExpiration;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ProjectWebCoreModule).GetAssembly());
        }
    }
}