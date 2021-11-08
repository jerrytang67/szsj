using System.Threading;
using System.Threading.Tasks;
using Abp.Dependency;
using EntityFrameworkCore.Triggered;
using TTWork.Abp.Timeline.Domains;

// ReSharper disable once CheckNamespace
namespace TTWork.Triggers.Timeline
{
    public class TimelineEventTrigger : IBeforeSaveTrigger<TimelineEvent>, ITransientDependency, ITriggerPriority
    {
        public Task BeforeSave(ITriggerContext<TimelineEvent> context, CancellationToken cancellationToken)
        {
            // context.Entity.Tag = "Trigger Created";
            return Task.CompletedTask;
        }

        public int Priority => CommonTriggerPriority.Earlier; //0
    }
}