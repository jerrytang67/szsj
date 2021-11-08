using Abp.Localization;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.AuditManagement.Events.Queries;
using TTWork.Abp.LaborUnion.Events.AuditQueries;

namespace TTWork.Abp.LaborUnion.Definitions
{
    public class LaborUnionAuditDefinitionProvider : AuditDefinitionProvider
    {
        public override void Define(IAuditDefinitionContext context)
        {
            context.Add(
                Create<CraftsmanRecommendAuditQuery>(LaborUnionAudit.CraftsmanRecommend, "G", "T")
            );
            context.Add(
                Create<CraftsmanAuditQuery>(LaborUnionAudit.Craftsman, "G", "T")
            );
        }

        private AuditDefinition Create<T>(string name, params string[] providers) where T : class, IAuditQueryBase, new()
        {
            return new AuditDefinition(name,
                    new LocalizableString(name, LaborUnionConsts.LocalizationSourceName),
                    new T())
                .WithProviders(providers);
        }
    }

    public static class LaborUnionAudit
    {
        public const string CraftsmanRecommend = "Audit_LaborUnion_Craftsman_Recommend";
        public const string Craftsman = "Audit_LaborUnion_Craftsman";
    }
}