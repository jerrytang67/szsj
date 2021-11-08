using Abp;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TTWork.Abp.Activity.Definitions;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.Activity.Localization;
using TTWork.Abp.AppManagement;
using TTWork.Abp.AuditManagement;
using TTWork.Abp.Core.Extensions;

namespace TTWork.Abp.Activity
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreModule),
        typeof(AppManagementModule)
    )]
    public class ActivityModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 加入权限Provider
            Configuration.Authorization.Providers.Add<ActivityAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(ActivityMapper.CreateMappings);

            //注册appservice
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(ActivityModule).Assembly, moduleName: "Activity", useConventionalHttpVerbs: true);


            ActivityLocalizationConfigurer.Configure(Configuration.Localization);


            Configuration.EntityHistory.IsEnabled = true;
            Configuration.EntityHistory.Selectors.Add("ActvityEntity", typeof(LuckDrawPrize));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ActivityModule).GetAssembly());

            IocManager.RegisterMediatRAssembly<ActivityModule>();
        }

        public override void PostInitialize()
        {
            // App模块确保注入
            Configuration.Modules.AppModule().DefinitionProviders.AddIfNotContains(
                typeof(ActivityAppDefinitionProvider));

            // Audit模块确保注入
            Configuration.Modules.AuditModule().DefinitionProviders.AddIfNotContains(
                typeof(ActivityAuditDefinitionProvider));
        }
    }
}