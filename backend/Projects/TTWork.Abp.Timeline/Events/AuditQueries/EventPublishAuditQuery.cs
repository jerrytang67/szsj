using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.AuditManagement.Events;
using TTWork.Abp.AuditManagement.Events.Queries;
using TTWork.Abp.Timeline.Domains;

namespace TTWork.Abp.Timeline.Events.AuditQueries
{
    public class EventPublishAuditQuery : IAuditQueryBase
    {
        public AuditUserLog Input { get; set; }
        public AuditFlow Flow { get; set; }
        public AuditNode Node { get; set; }

        public class EventPublishAuditQueryHandle : AuditQueryHandlerBase<EventPublishAuditQuery, TimelineEvent, long>
        {
            public EventPublishAuditQueryHandle(IRepository<TimelineEvent, long> repository, IIocManager iocManager) : base(repository, iocManager)
            {
            }

            [UnitOfWork]
            protected override async Task Do(TimelineEvent entity, EventPublishAuditQuery request = null)
            {
                await Task.CompletedTask;
            }

            [UnitOfWork]
            protected override async Task Reject(TimelineEvent entity, EventPublishAuditQuery request = null)
            {
                await Task.CompletedTask;
            }
        }
    }
}