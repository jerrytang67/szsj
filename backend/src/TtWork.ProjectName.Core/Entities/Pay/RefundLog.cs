using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using Abp.UI;
using TtWork.ProjectName.Managers;
using Castle.Core.Logging;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.Core.Authorization.Users;

namespace TtWork.ProjectName.Entities.Pay
{
    [Table("T_RefundLogs")]
    public class RefundLog : Entity<long>, IModificationAudited<User>, IMustHaveTenant, INeedAudit, ICreationAudited<User>
    {
        protected RefundLog()
        {
        }

        public RefundLog(int price, string billNo)
        {
            Price = price;
            BillNo = billNo;
        }

        [StringLength(48)] public string BillNo { get; set; }

        /// <summary>
        /// 退款金额单位：分
        /// </summary>
        public int Price { get; protected set; }

        public bool IsSuccess { get; protected set; }

        public DateTime? SuccessTime { get; protected set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public User LastModifierUser { get; set; }

        public void Success()
        {
            SuccessTime = DateTime.Now;
            IsSuccess = true;
        }

        public long? CreatorUserId { get; set; }

        public User CreatorUser { get; set; }

        public int TenantId { get; set; }

        #region 审核相关

        public Guid? AuditFlowId { get; set; }
        public int? Audit { get; set; }
        public int? AuditStatus { get; set; }

        [NotMapped]
        public bool IsAudited
        {
            get
            {
                if (!Audit.HasValue) //未初始化
                    return false;
                return Audit == AuditStatus;
            }
        }

        #endregion
        
    }


    public class RefundAgreeEventData : EventData
    {
        public RefundAgreeEventData(RefundLog refundLog, PayOrder payOrder)
        {
            RefundLog = refundLog;
            PayOrder = payOrder;
        }

        public RefundLog RefundLog { get; set; }
        public PayOrder PayOrder { get; set; }
    }


    /// <summary>
    /// 同意退款处理逻辑
    /// </summary>
    // public class RefundAgreeHandler : IAsyncEventHandler<RefundAgreeEventData>, ITransientDependency
    // {
    //     public ILogger Logger { get; set; }
    //     public readonly PayManager _PayManager;
    //     private readonly IRepository<ActivityApply> _applyRepository;
    //
    //     public RefundAgreeHandler(PayManager payManager, IRepository<ActivityApply> applyRepository)
    //     {
    //         _PayManager = payManager;
    //         _applyRepository = applyRepository;
    //         Logger = NullLogger.Instance;
    //     }
    //
    //
    //     public async Task HandleEventAsync(RefundAgreeEventData eventData)
    //     {
    //         //eventData.RefundLog.CreationTime = eventData.RefundLog.CreationTime.AddMinutes(1);
    //         //微信退款操作
    //         var apply = await _applyRepository.FirstOrDefaultAsync(z => z.Id == eventData.PayOrder.OrderId);
    //
    //         apply.Cancel();
    //
    //         var result = await _PayManager.TenPayRefund(eventData.PayOrder, eventData.RefundLog);
    //         Logger.Warn($"订单 退款{result} ");
    //         if (result.Item1 == false)
    //             throw new UserFriendlyException(result.Item2);
    //         eventData.RefundLog.Success();
    //         eventData.PayOrder.RefundComplate();
    //         Logger.Warn($"订单 {eventData.PayOrder.BillNo} 退款成功");
    //     }
    // }
}