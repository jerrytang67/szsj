using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TTWork.Abp.Core.Oss;

namespace TTWork.Abp.Oss.UpYun
{
    [DependsOn(typeof(TtWork.ProjectName.ProjectCoreModule))]
    public class UpYunModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(GetType().GetAssembly());

            IocManager.Register<IOssClient, UpYunClient>(DependencyLifeStyle.Singleton);

            Configuration.Settings.Providers.Add<UpYunOssSettingProvider>();
        }
    }
}