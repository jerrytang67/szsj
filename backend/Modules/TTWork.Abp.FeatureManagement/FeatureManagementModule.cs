using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AutoMapper;
using TTWork.Abp.AppManagement;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.FeatureManagement.Features;
using TTWork.Abp.FeatureManagement.ValueProviders;

namespace TTWork.Abp.FeatureManagement
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreModule),
        typeof(AppManagementModule)
    )]
    public class FeatureManagementModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<FeatureOptions>();
            Configuration.Modules.FeatureModule().ValueProviders.Add<GlobalFeatureValueProvider>();
            Configuration.Modules.FeatureModule().ValueProviders.Add<TenantFeatureValueProvider>();
            Configuration.Modules.FeatureModule().ValueProviders.Add<OrganizationFeatureValueProvider>();
            // Configuration.Modules.FeatureModule().ValueProviders.Add<AreaFeatureValueProvider>();

            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(FeatureManagementModule).Assembly, moduleName: "FeatureManagement", useConventionalHttpVerbs: true);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(FeatureManagementMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FeatureManagementModule).GetAssembly());
            IocManager.RegisterMediatRAssembly<FeatureManagementModule>();
        }
    }

    public class FeatureManagementMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression cfg)
        {
        }
    }

    public static class FeatureConfigurationExtensions
    {
        public static FeatureOptions FeatureModule(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<FeatureOptions>();
        }
    }
}