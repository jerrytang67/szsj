using Abp.Dependency;

namespace TTWork.Abp.AuditManagement.Audits
{
    public abstract class AuditDefinitionProvider : IAuditDefinitionProvider, ITransientDependency
    {
        public abstract void Define(IAuditDefinitionContext context);
    }
}