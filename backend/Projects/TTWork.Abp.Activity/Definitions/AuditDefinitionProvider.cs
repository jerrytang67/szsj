using Abp.Localization;
using TTWork.Abp.Activity.Events;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.AuditManagement.Events.Queries;
using TTWork.Abp.LaborUnion;

namespace TTWork.Abp.Activity.Definitions
{
    public class ActivityAuditDefinitionProvider : AuditDefinitionProvider
    {
        public override void Define(IAuditDefinitionContext context)
        {
            // context.Add(
            //     Create<VoteItemAuditQuery>(VoteAudit.VoteItem, "G", "T")
            // );
        }

        private AuditDefinition Create<T>(string name, params string[] providers) where T : class, IAuditQueryBase, new()
        {
            return new AuditDefinition(name,
                    new LocalizableString(name, ActivityConsts.LocalizationSourceName),
                    new T())
                .WithProviders(providers);
        }
    }

    public static class ActivityAudit
    {
    }
}