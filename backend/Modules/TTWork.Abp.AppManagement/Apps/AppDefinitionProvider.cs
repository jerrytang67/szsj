using Abp.Dependency;

namespace TTWork.Abp.AppManagement.Apps
{
    public abstract class AppDefinitionProvider : IAppDefinitionProvider, ITransientDependency
    {
        public abstract void Define(IAppDefinitionContext context);
    }
}