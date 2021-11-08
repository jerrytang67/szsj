using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Handlers;
using TtWork.ProjectName.Entities.Pay;
using TtWork.ProjectName.Managers;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.Core.Applications.Wechat;
using TTWork.Abp.Core.Extensions;

namespace TtWork.ProjectName.Events.Handles
{
    public class PayEventHandle : IAsyncEventHandler<PayEvent>, ITransientDependency
    {
        private readonly WeixinManger _wxManger;
        private readonly PayManager _payManager;
        private readonly IRepository<PayOrder, long> _payorderRepository;
        private readonly IWxTemplateMsgSender _sender;

        public PayEventHandle(WeixinManger wxManger, PayManager payManager,
            IRepository<PayOrder, long> payorderRepository,
            IWxTemplateMsgSender sender)
        {
            _wxManger = wxManger;
            _payManager = payManager;
            _payorderRepository = payorderRepository;
            _sender = sender;
        }

        public async Task<List<PayOrder>> GetApplyPay(int applyid)
        {
            return await _payorderRepository.GetAll().Where(z => z.OrderId == applyid).ToListAsync();
        }


        [UnitOfWork]
        public virtual Task HandleEventAsync(PayEvent eventData)
        {
            if (eventData is PaySuccessEvent)
            {
                // var pay = eventData.PayOrder;
            }

            return Task.CompletedTask;
        }
    }
}