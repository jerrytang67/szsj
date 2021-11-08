using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.AuditManagement.Events.Queries;
using TTWork.Abp.Core.Extensions;

namespace TTWork.Abp.AuditManagement.Events
{
    public abstract class AuditQueryHandlerBase<TQuery,
        TEntity, TPrimaryKey> : IRequestHandler<TQuery, AuditResult>
        where TQuery : class, IAuditQueryBase
        where TEntity : class, IEntity<TPrimaryKey>, INeedAudit, ICreationAudited
    {
        private readonly IRepository<TEntity, TPrimaryKey> _repository;
        private readonly IRepository<AuditUserLog, long> _auditLogRepository;
        private readonly IRepository<AuditNode, Guid> _auditNodeRepository;
        private readonly IIocManager _iocManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        protected AuditQueryHandlerBase(
            IRepository<TEntity, TPrimaryKey> repository,
            IIocManager iocManager)
        {
            _repository = repository;
            _iocManager = iocManager;

            _unitOfWorkManager = iocManager.Resolve<IUnitOfWorkManager>();
            _auditLogRepository = iocManager.Resolve<IRepository<AuditUserLog, long>>();
            _auditNodeRepository = iocManager.Resolve<IRepository<AuditNode, Guid>>();
        }

        [UnitOfWork]
        public virtual async Task<AuditResult> Handle(TQuery request, CancellationToken cancellationToken)
        {
            var projectId = ConvertToType(request.Input.HostId, typeof(TPrimaryKey));

            // var all = await _repository.GetAllListAsync();

            var find = await _repository.GetAll()
                .FirstOrDefaultAsync(ExpressionUtils.BuildPredicate<TEntity>("Id", "==", projectId),
                    cancellationToken);

            if (find == null)
                throw new AbpException("找不到这条记录");

            // 判断下一步是不是正确的次序
            if (find.AuditStatus == null && request.Node.Index != 0)
                throw new AbpException("Node顺序有误");
            if (find.AuditStatus.HasValue && find.AuditStatus.Value + 1 != request.Node.Index)
                throw new AbpException("Node顺序有误");

            if (request.Input.Id == 0)
            {
                await _auditLogRepository.InsertAsync(request.Input);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }

            if (request.Input.Status == AuditUserLogStatus.Pass)
            {
                #region 全部通过的条件 - 从日志中搜索平级节点

                if (request.Flow.Type == AuditFlowType.AuditAll)
                {
                    var index = find.AuditStatus == null ? 0 : find.AuditStatus + 1;
                    var flowNodesCount = await _auditNodeRepository.GetAll()
                        .CountAsync(x => x.AuditFlowId == request.Flow.Id &&
                                         x.Index == index, cancellationToken);
                    var passNodesCount = await _auditLogRepository.GetAllListAsync(x =>
                        x.AuditName == request.Input.AuditName &&
                        x.HostId == request.Input.HostId &&
                        x.Status == AuditUserLogStatus.Pass &&
                        x.AuditStatus == find.AuditStatus &&
                        x.Expired == null);

                    if (flowNodesCount != passNodesCount.Count)
                    {
                        // 平级未全部通过
                        return new AuditResult()
                        {
                            NextNodeId = null,
                            CreatorUserId = find.CreatorUserId,
                            Status = AuditUserLogStatus.Continue
                        };
                    }
                }

                #endregion

                // 同意的操作
                find.AuditStatus = request.Node.Index;
                if (find.AuditStatus == find.Audit) //表示没有后继了
                {
                    await Do(find, request);
                    return new AuditResult()
                    {
                        NextNodeId = null,
                        CreatorUserId = find.CreatorUserId,
                        Status = AuditUserLogStatus.Pass
                    };
                }

                return new AuditResult()
                {
                    NextNodeId = request.Node.Index + 1,
                    CreatorUserId = find.CreatorUserId,
                    Status = AuditUserLogStatus.Pass
                };
            }
            else if (request.Input.Status == AuditUserLogStatus.Back)
            {
                if (find.AuditStatus == null)
                    throw new AbpException("没有提交审核无法退回");

                #region 退回本级通过日志为过期

                // 取得本级和上一级的日志
                var backStatus = (new[] {find.AuditStatus, find.AuditStatus == 0 ? null : find.AuditStatus - 1}).ToList();
                var currentNodeLogs = await _auditLogRepository.GetAllListAsync(x =>
                    x.HostId == find.Id.ToString() &&
                    x.AuditName == request.Input.AuditName &&
                    backStatus.Contains(x.AuditStatus) &&
                    x.Status == AuditUserLogStatus.Pass &&
                    x.Expired == null);

                foreach (var log in currentNodeLogs)
                    log.Expired = true;

                #endregion

                // 退回的操作
                find.AuditStatus = find.AuditStatus > 0 ? find.AuditStatus - 1 : null;
                return new AuditResult()
                {
                    NextNodeId = find.AuditStatus + 1 ?? 0,
                    CreatorUserId = find.CreatorUserId,
                    Status = AuditUserLogStatus.Back
                };
            }

            // 被拒
            find.AuditStatus = null;
            find.Audit = -1;

            await Reject(find, request);
            return new AuditResult()
            {
                NextNodeId = -1,
                CreatorUserId = find.CreatorUserId
            };
        }

        /// <summary>
        /// 当全部审核通过以后，要执行的动作
        /// </summary>
        protected abstract Task Do(TEntity entity, TQuery request = null);

        protected abstract Task Reject(TEntity entity, TQuery request = null);
        
        
        public static object ConvertToType(string value, Type targetType)
        {
            try
            {
                Type underlyingType = Nullable.GetUnderlyingType(targetType);

                if (underlyingType != null)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                }
                else
                {
                    underlyingType = targetType;
                }
                switch (underlyingType.Name)
                {
                    case "Guid":
                        return Guid.Parse(value);
                    default:
                        return Convert.ChangeType(value, underlyingType);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}