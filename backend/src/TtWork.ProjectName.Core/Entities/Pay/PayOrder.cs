using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using Abp.UI;
using TtWork.ProjectName.Events;
using TTWork.Abp.Core.Authorization.Users;
using TtWork.Lib.Extensions;

namespace TtWork.ProjectName.Entities.Pay
{
    [Table("T_PayOrders")]
    public class PayOrder : AggregateRoot<long>, ICreationAudited<User>, IExtendableObject, ISoftDelete, IMayHaveOrganizationUnit, IMustHaveTenant, IMayHaveFromUser
    {
        [StringLength(128)] public string Body { get; protected set; }

        /// <summary>
        /// 单位:分
        /// </summary>
        public int TotalPrice { get; protected set; }

        [StringLength(48)] public string BillNo { get; protected set; }

        [StringLength(32)] public string OpenId { get; protected set; }

        public OrderType Type { get; protected set; }
        public int OrderId { get; set; }
        public PayType PayType { get; protected set; }

        #region 支付

        public bool IsSuccessPay { get; protected set; } = false;
        public DateTime? SuccessPayTime { get; protected set; } = null;

        #endregion


        #region 退款

        public bool IsRefund { get; protected set; }
        public DateTime? RefundTime { get; protected set; }
        public DateTime? RefundComplateTime { get; protected set; }
        public int? RefundPrice { get; protected set; } = null;

        #endregion

        public int State { get; protected set; }

        #region from_interface

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public User CreatorUser { get; set; }

        public bool IsDeleted { get; set; }

        [StringLength(512)] public string ExtensionData { get; set; }

        public int FromClient { get; protected set; } = 1; //小程序

        #endregion

        public void SuccessPay(int notifyId)
        {
            IsSuccessPay = true;
            SuccessPayTime = DateTime.Now;
            this.SetData("TenPayNotify_Id", notifyId);
            DomainEvents.Add(new PaySuccessEvent {PayOrder = this});
        }

        /// <summary>
        /// 退款操作
        /// </summary>
        /// <param name="value">退款金额，单位：分</param>
        public void Refund(int value, bool sendMsg = true)
        {
            IsRefund = true;
            RefundPrice = value;
            RefundTime = DateTime.Now;
            // 领域事件（退款）
            DomainEvents.Add(new RefundEvent {PayOrder = this, Price = value, SendMsg = sendMsg});
        }

        public void RefundAgree(RefundLog refundLog)
        {
            if (RefundComplateTime != null)
                throw new UserFriendlyException($"退款已经在{RefundComplateTime:g}成功");

            DomainEvents.Add(new RefundAgreeEventData(refundLog, this));
        }


        /// <summary>
        /// 从活动订单生成支付Order
        /// </summary>
        // public void CreatWxPayFromActivityApply(ActivityApply apply, string mchId, string openid, int fromClient = 1) // fromClient:2,公众号
        // {
        //     InitBillNo(mchId);
        //     Body = apply.Activity?.GetPayBody();
        //     OpenId = openid;
        //     TotalPrice = Convert.ToInt32(apply.TotalPrice * 100);
        //     Type = OrderType.Default;
        //     OrderId = apply.Id;
        //     PayType = PayType.微信;
        //     FromClient = fromClient;
        //     CreatorUserId = apply.CreatorUserId;
        //     OrganizationUnitId = apply.OrganizationUnitId;
        //     TenantId = apply.TenantId;
        // }

        #region 私有方法

        private void InitBillNo(string mchId)
        {
            BillNo = $"{mchId}{DateTime.Now:yyyyMMddHHmmss}{StringEx.BuildRandomStr(6)}";
        }

        #endregion

        public long? OrganizationUnitId { get; set; }
        public int TenantId { get; set; }

        public void RefundComplate()
        {
            RefundComplateTime = DateTime.Now;
        }

        public void RefundReject(RefundLog refundLog)
        {
            this.RefundPrice -= refundLog.Price;
        }

        public long? FromUserId { get; set; }

        [ForeignKey("FromUserId")] public virtual User FromUser { get; set; }

        [Column(TypeName = "decimal(18,2)")] public virtual decimal? Commission { get; set; }
    }


    public enum PayType
    {
        [Display(Name = "微信")] 微信 = 1,
        [Display(Name = "微信扫码")] 微信扫码 = 2,
        [Display(Name = "支付宝")] 支付宝 = 3,
        [Display(Name = "银联")] 银联 = 4,
        [Display(Name = "用户余额")] 用户余额 = 10
    }

    public enum OrderType
    {
        Default = 0
    }
}