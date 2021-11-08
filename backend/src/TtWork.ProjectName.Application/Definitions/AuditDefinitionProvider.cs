using Abp.Localization;
using TtWork.ProjectName.Events.AuditQueries;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.AuditManagement.Events.Queries;

namespace TtWork.ProjectName.Definitions
{
    public class ProjectNameAuditDefinitionProvider : AuditDefinitionProvider
    {
        public override void Define(IAuditDefinitionContext context)
        {
            context.Add(
                Create<OrganizationEditAproveAuditQuery>(OrganizationAudit.ApplyAprove, "G", "T"),
                Create<OrganizationEditAproveAuditQuery>(OrganizationAudit.EditAprove, "G", "T"));

        }

        private AuditDefinition Create<T>(string name, params string[] providers) where T : class, IAuditQueryBase, new()
        {
            return new AuditDefinition(name,
                    new LocalizableString(name, ProjectNameConsts.LocalizationSourceName),
                    new T())
                .WithProviders(providers);
        }
    }

    public static class OrganizationAudit
    {
        public const string ApplyAprove = "Audit_Organization_ApplyAprove";
        public const string EditAprove = "Audit_Organization_EditAprove";
    }

    public static class RefundAudit
    {
        public const string Default = "Audit_Refund_Default";
    }

    public static class SettleAudit
    {
        public const string WithdrewalAprove = "Withdrewal_Aprove";
    }
}