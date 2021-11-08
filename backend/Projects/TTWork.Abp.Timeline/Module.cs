using Abp;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TTWork.Abp.Timeline.Definitions;
using TTWork.Abp.Timeline.Localization;
using TTWork.Abp.AppManagement;
using TTWork.Abp.AuditManagement;
using TTWork.Abp.Core.Extensions;

namespace TTWork.Abp.Timeline
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreModule),
        typeof(AppManagementModule)
    )]
    public class TimelineModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 加入权限Provider
            Configuration.Authorization.Providers.Add<TimelineAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(TimelineMapper.CreateMappings);

            //注册appservice
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(TimelineModule).Assembly, moduleName: "Timeline", useConventionalHttpVerbs: true);

            TimelineLocalizationConfigurer.Configure(Configuration.Localization);

            // Configuration.EntityHistory.IsEnabled = true;
            // Configuration.EntityHistory.Selectors.Add("ActvityEntity", typeof(LuckDrawPrize));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TimelineModule).GetAssembly());

            IocManager.RegisterMediatRAssembly<TimelineModule>();
        }

        public override void PostInitialize()
        {
            // App模块确保注入
            Configuration.Modules.AppModule().DefinitionProviders.AddIfNotContains(
                typeof(TimelineAppDefinitionProvider));

            // Audit模块确保注入
            Configuration.Modules.AuditModule().DefinitionProviders.AddIfNotContains(
                typeof(TimelineAuditDefinitionProvider));
        }
    }
}