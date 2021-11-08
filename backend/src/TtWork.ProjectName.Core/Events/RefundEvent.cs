using Abp.Events.Bus;
using TtWork.ProjectName.Entities.Pay;

namespace TtWork.ProjectName.Events
{
    public class RefundEvent : EventData
    {
        public PayOrder PayOrder { get; set; }

        public int Price { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
        public string Reason { get; set; }

        public bool SendMsg { get; set; } = true;
    }
}