using Abp;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TTWork.Abp.QA.Definitions;
using TTWork.Abp.QA.Localization;
using TTWork.Abp.AppManagement;
using TTWork.Abp.AuditManagement;
using TTWork.Abp.Core.Extensions;

namespace TTWork.Abp.QA
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreModule),
        typeof(AppManagementModule)
    )]
    // ReSharper disable once InconsistentNaming
    public class QAModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 加入权限Provider
            Configuration.Authorization.Providers.Add<QAAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(QAMapper.CreateMappings);

            //注册appservice
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(QAModule).Assembly, moduleName: "QA", useConventionalHttpVerbs: true);

            QALocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QAModule).GetAssembly());

            IocManager.RegisterMediatRAssembly<QAModule>();
        }

        public override void PostInitialize()
        {
            // App模块确保注入
            Configuration.Modules.AppModule().DefinitionProviders.AddIfNotContains(
                typeof(QAAppDefinitionProvider));

            // Audit模块确保注入
            Configuration.Modules.AuditModule().DefinitionProviders.AddIfNotContains(
                typeof(QAAuditDefinitionProvider));
        }
    }
}