using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Handlers;
using TtWork.ProjectName.Entities.Pay;

namespace TtWork.ProjectName.Events.Handles
{
    public class RefundRejectedEventHandler : IAsyncEventHandler<RefundRejectedEvent>, ITransientDependency
    {
        private readonly IRepository<PayOrder, long> _payOrderRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public RefundRejectedEventHandler(IRepository<PayOrder, long> payOrderRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _payOrderRepository = payOrderRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(RefundRejectedEvent eventData)
        {
            var payOrder = await _payOrderRepository.FirstOrDefaultAsync(s => s.BillNo == eventData.RefundLog.BillNo);
            payOrder.RefundReject(eventData.RefundLog);
            await Task.CompletedTask;
        }
    }
}