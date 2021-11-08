using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Session;
using Abp.UI;
using TtWork.ProjectName.Entities.Pay;
using Castle.Core.Logging;
using TTWork.Abp.Core.Applications.Wechat;
using TTWork.Abp.Core.Extensions;

namespace TtWork.ProjectName.Events.Handles
{
    public class RefundEventHandle : IAsyncEventHandler<RefundEvent>, ITransientDependency
    {
        private readonly IRepository<RefundLog, long> _refundLogRepository;

        // private readonly IRepository<ActivityApply> _applyRepository;

        //        private readonly ISmsSender _smsSender;
        private readonly ISettingManager _settingManager;
        private readonly IAbpSession _abpSession;
        private readonly WeixinManger _weixinManger;
        private readonly WxTemplateMsgSender _sender;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public ILogger Logger { get; set; }

        public RefundEventHandle(
            IRepository<RefundLog, long> refundLogRepository,
            //            ISmsSender smsSender, 
            IAbpSession abpSession,
            ISettingManager settingManager,
            WeixinManger weixinManger,
            WxTemplateMsgSender sender,
            IUnitOfWorkManager unitOfWorkManager
            // ,IRepository<ActivityApply> applyRepository
        )
        {
            _refundLogRepository = refundLogRepository;
            //            _smsSender = smsSender;
            _abpSession = abpSession;
            _settingManager = settingManager;
            _weixinManger = weixinManger;
            _sender = sender;
            _unitOfWorkManager = unitOfWorkManager;
            // _applyRepository = applyRepository;
            Logger = NullLogger.Instance;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(RefundEvent eventData)
        {
            var find = await _refundLogRepository.FirstOrDefaultAsync(x => x.BillNo == eventData.PayOrder.BillNo);
            if (find != null)
            {
                throw new UserFriendlyException("此订单已申请退款,请不要重复申请");
            }

            var insert = await _refundLogRepository.InsertAndGetIdAsync(new RefundLog(eventData.Price, eventData.PayOrder.BillNo));
            await _unitOfWorkManager.Current.SaveChangesAsync();

            if (eventData.SendMsg)
            {
            }
        }
    }
}