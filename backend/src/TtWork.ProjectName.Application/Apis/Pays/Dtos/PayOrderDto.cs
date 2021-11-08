using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using TTWork.Abp.Core.Applications.Dtos;
using TtWork.ProjectName.Entities.Pay;

namespace TtWork.ProjectName.Apis.Pays
{
    public class PayOrderDto : EntityDto<long>
    {
        public string Body { get; protected set; }

        /// <summary>
        /// 单位:分
        /// </summary>
        public int TotalPrice { get; protected set; }

        [StringLength(48)] public string BillNo { get; protected set; }
        public OrderType Type { get; protected set; }
        public int OrderId { get; set; }
        public PayType PayType { get; protected set; }
        public bool IsSuccessPay { get; protected set; } = false;

        public DateTime? SuccessPayTime { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsRefund { get; protected set; } = false;

        public long? OrganizationUnitId { get; set; }

        public DateTime? RefundComplateTime { get; set; }

        public long? FromUserId { get; set; }

        public UserDtoBase FromUser { get; set; }
    }
}