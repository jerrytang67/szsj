using Abp.Localization;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.AuditManagement.Events.Queries;
using TTWork.Abp.LaborUnion;

namespace TTWork.Abp.QA.Definitions
{
    public class QAAuditDefinitionProvider : AuditDefinitionProvider
    {
        public override void Define(IAuditDefinitionContext context)
        {
            // context.Add(
            //     Create<CraftsmanRecommendAuditQuery>(LaborUnionAudit.CraftsmanRecommend, "G", "T")
            // );
        }

        private AuditDefinition Create<T>(string name, params string[] providers) where T : class, IAuditQueryBase, new()
        {
            return new AuditDefinition(name,
                    new LocalizableString(name, QAConsts.LocalizationSourceName),
                    new T())
                .WithProviders(providers);
        }
    }

    public static class QAAudit
    {
    }
}