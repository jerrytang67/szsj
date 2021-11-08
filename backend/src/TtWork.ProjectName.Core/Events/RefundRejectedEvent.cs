using Abp.Events.Bus;
using TtWork.ProjectName.Entities.Pay;

namespace TtWork.ProjectName.Events
{
    public class RefundRejectedEvent : EventData
    {
        public RefundLog RefundLog { get; set; }
    }
}