using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TtWork.ProjectName.Definitions;
using TTWork.Abp.AppManagement;
using TTWork.Abp.AuditManagement;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.FeatureManagement;
using TTWork.Abp.Oss.UpYun;

namespace TtWork.ProjectName
{
    [DependsOn(
        typeof(ProjectCoreModule),
        typeof(AuditManagementModule),
        typeof(FeatureManagementModule),
        typeof(UpYunModule),
        typeof(AbpAutoMapperModule))]
    public class ProjectApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ProjectNameAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ProjectApplicationModule).GetAssembly());
            IocManager.RegisterMediatRAssembly<ProjectApplicationModule>();
        }

        public override void PostInitialize()
        {
            // App模块确保注入
            Configuration.Modules.AppModule().DefinitionProviders.AddIfNotContains(
                typeof(ProjectNameAppDefinitionProvider));

            // Audit模块确保注入
            Configuration.Modules.AuditModule().DefinitionProviders.AddIfNotContains(
                typeof(ProjectNameAuditDefinitionProvider));

            // Feature模块确保注入
            Configuration.Modules.FeatureModule().DefinitionProviders.AddIfNotContains(
                typeof(ProjectNameFeatureDefinitionProvider));
        }
    }
}