using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.AuditManagement.Events;

namespace TTWork.Abp.AuditManagement.Applications
{
    public class AuditManager : ITransientDependency
    {
        public readonly IRepository<AuditFlow, Guid> _auditFlowRepository;
        public readonly IRepository<AuditNode, Guid> _auditNodeRepository;
        public readonly IRepository<AuditUserLog, long> _auditUserLogRepository;
        public readonly IMediator _Mediator;

        public readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IAbpSession _abpSession;


        public AuditManager(
            IIocManager iocManager
        )
        {
            _auditFlowRepository = iocManager.Resolve<IRepository<AuditFlow, Guid>>();
            _auditNodeRepository = iocManager.Resolve<IRepository<AuditNode, Guid>>();
            _auditUserLogRepository = iocManager.Resolve<IRepository<AuditUserLog, long>>();

            _unitOfWorkManager = iocManager.Resolve<IUnitOfWorkManager>();
            _Mediator = iocManager.Resolve<IMediator>();
            _abpSession = iocManager.Resolve<IAbpSession>();
        }

        [UnitOfWork]
        public virtual async Task StartAudit<T, TPrimaryKey>(T entity) where T : INeedAudit, IEntity<TPrimaryKey>
        {
            var auditFlow = await _auditFlowRepository.FirstOrDefaultAsync(x => x.Id == entity.AuditFlowId);

            entity.Audit = auditFlow.NodesMaxIndex;
            entity.AuditStatus = null;

            #region 前面的所有的相关通过设为过期

            var auditLogs = await _auditUserLogRepository.GetAll().Where(x => x.HostId == entity.Id.ToString()
                                                                              && x.AuditName == auditFlow.AuditName
                                                                              && x.Expired != true
                // && x.Status == AuditUserLogStatus.Pass
            ).ToListAsync();

            foreach (var log in auditLogs)
            {
                log.Expired = true;
            }

            await _unitOfWorkManager.Current.SaveChangesAsync();

            #endregion

            // TODO:发送通知的东西 
            // var result = await _mediator.Send(new QueryAuditNotify<T, TPrimaryKey>(entity));
        }


        [UnitOfWork]
        public virtual async Task<bool> CheckNodeIsAudit<T, TPrimaryKey>(T entity) where T : INeedAudit, IEntity<TPrimaryKey>
        {
            // var auditType = GetAuditType(entity.GetType().ToString().Split('.')[^1]);
            var _id = entity.Id.ToString();
            var any = await _auditUserLogRepository.GetAllListAsync(x => x.HostId == _id &&
                                                                         x.AuditFlowId == entity.AuditFlowId &&
                                                                         x.Status == AuditUserLogStatus.Pass &&
                                                                         x.AuditStatus == entity.AuditStatus &&
                                                                         x.Expired == null &&
                                                                         x.CreatorUserId == _abpSession.UserId
            );

            return any.Any();
        }


        // protected static string[] GetAuditType(string type) =>
        //     type switch
        //     {
        //         "Project" => new[] {AuditUserLogType.Project},
        //         "Reimbursement" => new[] {AuditUserLogType.Reimbursement},
        //         "PayrollRequisition" => new[] {AuditUserLogType.PayrollRequisition},
        //         "Activity" => new[] {AuditUserLogType.ActivityPublish},
        //         "RefundLog" => new[]
        //             {AuditUserLogType.ActivityRefund, AuditUserLogType.UserCardRefund},
        //         _ => new AuditUserLogType[] { }
        //     };
    }
}