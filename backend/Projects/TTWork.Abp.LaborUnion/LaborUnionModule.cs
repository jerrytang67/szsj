using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TTWork.Abp.AppManagement;
using TTWork.Abp.AuditManagement;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.LaborUnion.Definitions;
using TTWork.Abp.LaborUnion.Localization;

namespace TTWork.Abp.LaborUnion
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreModule),
        typeof(AppManagementModule)
        //, typeof(AuditManagementModule)
    )]
    public class LaborUnionModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 加入权限Provider
            Configuration.Authorization.Providers.Add<LaborUnionAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(LaborUnionMapper.CreateMappings);

            //注册appservice
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(LaborUnionModule).Assembly, moduleName: "LaborUnion", useConventionalHttpVerbs: true);
            
            LaborUnionLocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LaborUnionModule).GetAssembly());

            IocManager.RegisterMediatRAssembly<LaborUnionModule>();
        }

        public override void PostInitialize()
        {
            // App模块确保注入
            Configuration.Modules.AppModule().DefinitionProviders.AddIfNotContains(
                typeof(LaborUnionAppDefinitionProvider));
            
            
            // Audit模块确保注入
            Configuration.Modules.AuditModule().DefinitionProviders.AddIfNotContains(
                typeof(LaborUnionAuditDefinitionProvider));
        }
    }
}