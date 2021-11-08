using Abp.Events.Bus;
using TtWork.ProjectName.Entities.Pay;

namespace TtWork.ProjectName.Events
{
    public class RefundCompletedEvent : EventData
    {
        public PayOrder PayOrder { get; set; }

        public RefundLog RefundLog { get; set; }
    }
}