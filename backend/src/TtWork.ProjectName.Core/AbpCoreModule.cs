using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using TtWork.ProjectName.Authorization.Roles;
using TtWork.ProjectName.Configuration;
using TtWork.ProjectName.Timing;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Authorization.Roles;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.Core.MultiTenancy;
using TtWork.ProjectName.Localization;

namespace TtWork.ProjectName
{
    [DependsOn(
        typeof(AbpZeroCoreModule),
        typeof(TTWorkAbpCoreModule)
    )]
    public class ProjectCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            // Enable this line to create a multi-tenant application.
            // 多租户开启状态
            Configuration.MultiTenancy.IsEnabled = ProjectNameConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            // Adding setting providers
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            AbpLocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ProjectCoreModule).GetAssembly());
            IocManager.RegisterMediatRAssembly<ProjectCoreModule>();
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}