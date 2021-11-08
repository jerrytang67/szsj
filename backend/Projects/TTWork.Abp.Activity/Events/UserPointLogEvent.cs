using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using TTWork.Abp.Activity.Domains;
using WorkflowCore.Interface;

namespace TTWork.Abp.Activity.Events
{
    public class UserPointLogEvent : EventData
    {
        /// <summary>
        /// UserPointLogEvent
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="before">变更前</param>
        /// <param name="after">变更后</param>
        /// <param name="eventType">eventType</param>
        /// <param name="eventId">eventId</param>
        /// <param name="desc">描述</param>
        public UserPointLogEvent(long userId, int before, int after, EnumClass.UserPointEventType eventType, string eventId,
            string desc)
        {
            UserId = userId;
            Before = before;
            After = after;
            EventType = eventType;
            EventId = eventId;
            Desc = desc;
        }

        public int After { get; set; }
        public int Before { get; set; }
        public long UserId { get; set; }

        public EnumClass.UserPointEventType EventType { get; set; }

        public string EventId { get; set; }
        public string Desc { get; set; }

        public class UserPointLogEventHandle : IAsyncEventHandler<UserPointLogEvent>, ITransientDependency
        {
            private readonly IRepository<UserPointLog, long> _logRepository;
            private readonly IUnitOfWorkManager _unitOfWorkManager;
            private readonly IWorkflowHost _host;

            public UserPointLogEventHandle(
                IRepository<UserPointLog, long> logRepository,
                IUnitOfWorkManager unitOfWorkManager,
                IWorkflowHost host
            )
            {
                _logRepository = logRepository;
                _unitOfWorkManager = unitOfWorkManager;
                _host = host;
            }

            [UnitOfWork]
            public virtual async Task HandleEventAsync(UserPointLogEvent eventData)
            {
                await _logRepository.InsertAsync(new UserPointLog
                {
                    UserId = eventData.UserId,
                    BeforePoints = eventData.Before,
                    AfterPoints = eventData.After,
                    EventType = eventData.EventType,
                    EventId = eventData.EventId,
                    Desc = eventData.Desc
                });

                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }
    }
}