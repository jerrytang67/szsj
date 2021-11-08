﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TtWork.ProjectName.Configuration.Tenants.Dto
{
    public class NotifySettingEditDto
    {
        public string Phone { get; set; }
        public bool NewOrderSendStatus { get; set; }
        public bool RefundSendStatus { get; set; }
        public string AdminOpenid { get; set; }
    }
}