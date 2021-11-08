﻿using Abp.Events.Bus;
using TtWork.ProjectName.Entities.Pay;

namespace TtWork.ProjectName.Events
{
    public class PayEvent : EventData
    {
        public PayOrder PayOrder { get; set; }
    }

    public class PaySuccessEvent : PayEvent
    {
    }
}