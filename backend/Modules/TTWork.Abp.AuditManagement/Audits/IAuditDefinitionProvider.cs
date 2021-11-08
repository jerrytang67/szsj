namespace TTWork.Abp.AuditManagement.Audits
{
    public interface IAuditDefinitionProvider
    {
        void Define(IAuditDefinitionContext context);
    }
}