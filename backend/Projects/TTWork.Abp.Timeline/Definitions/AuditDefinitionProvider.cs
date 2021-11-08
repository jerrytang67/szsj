using Abp.Localization;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.AuditManagement.Events.Queries;
using TTWork.Abp.Timeline.Events.AuditQueries;

namespace TTWork.Abp.Timeline.Definitions
{
    public class TimelineAuditDefinitionProvider : AuditDefinitionProvider
    {
        public override void Define(IAuditDefinitionContext context)
        {
            context.Add(
                Create<EventPublishAuditQuery>(TimelineAudit.EventPublish, "G", "T")
            );
        }

        private AuditDefinition Create<T>(string name, params string[] providers) where T : class, IAuditQueryBase, new()
        {
            return new AuditDefinition(name,
                    new LocalizableString(name, TimelineConsts.LocalizationSourceName),
                    new T())
                .WithProviders(providers);
        }
    }

    public static class TimelineAudit
    {
        public const string EventPublish = "Audit_Timeline_Event_Publish";
    }
}