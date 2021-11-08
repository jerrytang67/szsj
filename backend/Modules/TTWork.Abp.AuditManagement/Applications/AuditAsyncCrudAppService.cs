using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Core.Internal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.AuditManagement.Applications.Dtos;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.AuditManagement.Events;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Authorization.Users;

namespace TTWork.Abp.AuditManagement.Applications
{
    public abstract class AuditAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> :
        AbpAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity<TPrimaryKey>, INeedAudit
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>, INeedAuditBase
        where TCreateInput : INeedAuditBase
        where TGetAllInput : PagedResultRequestDto

    {
        private IocManager IocManager { get; }

        // protected IWxTemplateMsgSender WxSender { get; private set; }
        private IRepository<UserRole, long> UserRoleRepository { get; }
        private IRepository<AuditFlow, Guid> AuditFlowRepository { get; }
        protected IRepository<AuditNode, Guid> AuditNodeRepository { get; }

        private IRepository<AuditUserLog, long> AuditUserLogRepository { get; }

        // private IRepository<OrganizationUnit, long> OrganizationUnitRepository { get; }
        private AuditManager AuditManager { get; }
        private IMediator Mediator { get; }

        protected IAuditProvider AuditProvider { get; }

        private IAuditDefinitionManager AuditDefinitionManager { get; }

        protected string AuditName { get; set; }

        protected bool AuditedCanEdit { get; set; } = false;

        public AuditAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository
            , IocManager iocManager
        ) : base(repository, iocManager)
        {
            IocManager = iocManager;
            LocalizationSourceName = AbpConsts.LocalizationSourceName;
            // WxSender = iocManager.Resolve<IWxTemplateMsgSender>();
            UserManager = iocManager.Resolve<UserManager>();
            AuditFlowRepository = iocManager.Resolve<IRepository<AuditFlow, Guid>>();
            AuditNodeRepository = iocManager.Resolve<IRepository<AuditNode, Guid>>();
            AuditUserLogRepository = iocManager.Resolve<IRepository<AuditUserLog, long>>();
            UserRoleRepository = iocManager.Resolve<IRepository<UserRole, long>>();
            AuditManager = iocManager.Resolve<AuditManager>();
            Mediator = iocManager.Resolve<IMediator>();
            // OrganizationUnitRepository = iocManager.Resolve<IRepository<OrganizationUnit, long>>();

            AuditProvider = iocManager.Resolve<IAuditProvider>();
            AuditDefinitionManager = iocManager.Resolve<IAuditDefinitionManager>();
        }

        protected async Task<bool> IsAdminAsync()
        {
            return await IsInRoleAsync(AbpSession.GetUserId(), "Admin");
        }

        protected void ThrowNotFindException()
        {
            throw new UserFriendlyException(L("NotFind"));
        }

        public override async Task<TEntityDto> CreateAsync(TCreateInput input)
        {
            await BeforeCreateAsync(input);

            CheckCreatePermission();

            var entity = MapToEntity(input);

            var auditName = GetAuditName(entity);

            if (!auditName.IsNullOrEmpty())
            {
                var auditFlowId = await AuditProvider.GetOrNullAsync(auditName);

                entity.AuditFlowId = auditFlowId;

                // 如果选中了需要审核,更新最新的审核流程过来
                if (input.AuditFlowId.HasValue)
                {
                    // 新建时为未提交审核状态
                    entity.Audit = null;
                }
            }

            await Repository.InsertAsync(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            await AfterCreateAsync(entity, input);

            return MapToEntityDto(entity);
        }

        protected virtual string GetAuditName(TEntity entity)
        {
            return AuditName;
        }

        /// <summary>
        /// 待审核的项目如果审核中编辑了,审核状态将清0
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public override async Task<TEntityDto> UpdateAsync(TUpdateInput input)
        {
            await BeforeUpdateAsync(input);

            CheckUpdatePermission();

            var entity = await GetEntityByIdAsync(input.Id);

            if (entity.IsAudited && !AuditedCanEdit)
                throw new UserFriendlyException("审核通过的不能再编辑");

            //if (!entity.AuditFlowId.HasValue) // 如果编辑的时候没有原来的审核,就拉取
            //{
            var auditName = GetAuditName(entity);

            if (!auditName.IsNullOrEmpty())
            {
                var auditFlowId = await AuditProvider.GetOrNullAsync(auditName);
                input.AuditFlowId = auditFlowId;
            }
            //}

            MapToEntity(input, entity);

            // 如果修改了审核流程,审核将重新开始
            entity.Audit = null;
            entity.AuditStatus = null;

            await Repository.UpdateAsync(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            await AfterUpdateAsync(entity, input);

            return MapToEntityDto(entity);
        }

        public virtual async Task StartAudit(EntityDto<TPrimaryKey> input)
        {
            CheckUpdatePermission();

            var entity = await GetEntityByIdAsync(input.Id);

            await StartAudit(entity);
        }

        protected async Task StartAudit(TEntity entity)
        {
            if (!entity.AuditFlowId.HasValue)
            {
                if (AuditName.IsNullOrEmpty())
                {
                    throw new UserFriendlyException("API中没有设置基础审核名称");
                }

                var flowId = await AuditProvider.GetOrNullAsync(AuditName);

                if (flowId.HasValue)
                {
                    entity.AuditFlowId = flowId;
                    await BeforeStartAuditDo(entity);
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
                else
                {
                    throw new UserFriendlyException("没有制定审核流程");
                }
            }

            await AuditManager.StartAudit<TEntity, TPrimaryKey>(entity);

            await AfterStartAuditDo(entity);
        }

        /// <summary>
        ///  [用于重写]
        /// </summary>
        protected virtual Task AfterUpdateAsync(TEntity entity, TUpdateInput input = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  [用于重写]
        /// </summary>
        protected virtual Task BeforeUpdateAsync(TUpdateInput input = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  [用于重写]
        /// </summary>
        protected virtual Task AfterCreateAsync(TEntity entity, TCreateInput input = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  [用于重写]
        /// </summary>
        protected virtual Task BeforeCreateAsync(TCreateInput input = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// [重写]
        /// </summary>
        protected virtual Task AfterStartAuditDo(TEntity entity)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// [用于重写]当全部审核通过以后，要执行的动作
        /// </summary>
        protected virtual Task BeforeStartAuditDo(TEntity entity)
        {
            return Task.CompletedTask;
        }


        public override async Task DeleteAsync(EntityDto<TPrimaryKey> input)
        {
            CheckDeletePermission();

            var entity = await GetEntityByIdAsync(input.Id);

            if (entity.IsAudited)
                throw new UserFriendlyException("审核通过后不能删除");

            await Repository.DeleteAsync(input.Id);
        }

        [AbpAllowAnonymous]
        public virtual async Task Audit(AuditUserLog input)
        {
            if (!AbpSession.UserId.HasValue)
                throw new UserFriendlyException("请先登录");

            // 审核流程
            var currentAuditFlow = await AuditFlowRepository.GetAll()
                .Include(x => x.AuditNodes)
                .FirstOrDefaultAsync(x => x.Id == input.AuditFlowId);

            if (currentAuditFlow == null)
                throw new UserFriendlyException("找不到这个审核流程");

            if (input.AuditName.IsNullOrEmpty())
            {
                input.AuditName = currentAuditFlow.AuditName;
                // throw new UserFriendlyException("未知的审核类型");
            }


            // 当前登录用户Roles
            var roles = await UserRoleRepository.GetAll()
                .Where(x => x.UserId == AbpSession.UserId)
                .Select(x => x.RoleId)
                .ToListAsync();

            var userId = AbpSession.UserId.Value;

            var success1Node = false;

            var nodes = currentAuditFlow
                .AuditNodes
                .Where(x => x.Index == input.GetNextAuditNodeIndex())
                .Where(x => x.CanIAudit(roles, userId))
                .ToList();
            // 根据传入的当前auditStatus在nodes 中取得相应的index节点
            foreach (var node in nodes)
            {
                // 假如这个节点只要1人通过,那么就不用例遍所有nodes
                if (currentAuditFlow.Type == AuditFlowType.AudtitOne && success1Node)
                    break;

                // 根据当前用户的权限和userId判断是否有权限审核

                // input传入的这个是00000,所以要赋值
                input.AuditNodeId = node.Id;

                var auditFlowDefinitiion = AuditDefinitionManager.Get(currentAuditFlow.AuditName);

                var q = auditFlowDefinitiion.AuditQuery;

                (q.Flow, q.Input, q.Node) = (currentAuditFlow, input, node);

                var result = await Mediator.Send(q);

                success1Node = true;

                // 给下一步的审核人发通知
                if (result.Status == AuditUserLogStatus.Pass && result.NextNodeId.HasValue)
                {
                    var nextNodes = await AuditNodeRepository.GetAllListAsync(
                        x => x.AuditFlowId == input.AuditFlowId && x.Index == result.NextNodeId.Value);
                    foreach (var nextNode in nextNodes)
                    {
                        try
                        {
                            using var localizationContext = IocManager.ResolveAsDisposable<ILocalizationContext>();

                            var localizeStr = auditFlowDefinitiion.DisplayName.Localize(localizationContext.Object);

                            // 通知
                            // await Mediator.Publish(new NotificationMessage(
                            //     $"你有待审核任务需要处理",
                            //     nextNode.UserId, null, new JObject
                            //     {
                            //         {"订单类型", localizeStr},
                            //         {"审核信息", $"当前审进度 {input.GetNextAuditNodeIndex() + 1} / {currentAuditFlow.NodesMaxIndex + 1}"},
                            //         {"审核时间", $"{input.CreationTime:g}"}
                            //     }));
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }
                else if (result.Status == AuditUserLogStatus.Pass && !result.NextNodeId.HasValue)
                {
                    // 审核全通过了,发送通知给建立的人
                    try
                    {
                        // 通知
                        // await Mediator.Publish(new NotificationMessage(
                        //     $"审核<{currentAuditFlow.Title}>已通过",
                        //     input, result.CreatorUserId, null));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                else if (result.Status == AuditUserLogStatus.Back && result.NextNodeId.HasValue)
                {
                    var nextNodes = await AuditNodeRepository.GetAllListAsync(
                        x => x.AuditFlowId == input.AuditFlowId && x.Index == result.NextNodeId.Value);
                    foreach (var nextNode in nextNodes)
                    {
                        // 通知
                        // await Mediator.Publish(new NotificationMessage(
                        //     $"你有待审核任务被退回重新审核<{currentAuditFlow.Title}>,退回原因{input.Desc}",
                        //     input, nextNode.UserId, nextNode.RoleId));
                    }
                }
                else if (result.Status == AuditUserLogStatus.Reject)
                {
                    //被拒绝
                    // 通知
                    // await Mediator.Publish(new NotificationMessage(
                    //     $"你提交的审核<{currentAuditFlow.Title}>被拒绝,原因:{input.Desc}",
                    //     input, result.CreatorUserId, null));
                }
                else if (result.Status == AuditUserLogStatus.Continue)
                {
                    // nothing to do
                }

                await Task.CompletedTask;
            }
        }


        /// <summary>
        /// 取得当前登录用户能审核的所有机构的项目
        /// </summary>
        public virtual async Task<PagedResultDto<TEntityDto>> GetMyAll(TGetAllInput input)
        {
            if (!AbpSession.UserId.HasValue)
                throw new UserFriendlyException("请先登录");

            var query = CreateFilteredQuery(input);

            query = query
                .Where(x => x.Audit != x.AuditStatus
                            && x.Audit >= 0); //Audit >=0为正常开始审核状态,-1为被拒绝,null为为提交审核
            var total = await query.CountAsync();

            query = ApplySorting(query, input);

            query = ApplyPaging(query, input);


            var result = await query.ToListAsync();

            var userId = AbpSession.UserId.Value;

            var resultList = new List<TEntityDto>();

            // 当前登录用户Roles
            var roles = await UserRoleRepository.GetAll()
                .Where(x => x.UserId == AbpSession.UserId)
                .Select(x => x.RoleId)
                .ToListAsync();

            foreach (var item in result)
            {
                var index = item.AuditStatus.HasValue ? item.AuditStatus + 1 : 0;
                var node = await AuditNodeRepository.GetAllListAsync(x =>
                    x.AuditFlowId == item.AuditFlowId && x.Index == index);

                if (node.Any(x => x.CanIAudit(roles, userId)))
                {
                    // 检查是否我已经通过
                    if (!await AuditManager.CheckNodeIsAudit<TEntity, TPrimaryKey>(item))
                        resultList.Add(ObjectMapper.Map<TEntityDto>(item));
                }
            }

            return new PagedResultDto<TEntityDto>(total, resultList);
        }

        public virtual async Task<List<AuditUserLogDto>> GetLogs(EntityDto<TPrimaryKey> input)
        {
            var entity = await GetEntityByIdAsync(input.Id);
            var hostId = entity.Id.ToString();
            var list = await AuditUserLogRepository.GetAll()
                .Include(x => x.CreatorUser)
                .Where(x => x.AuditFlowId == entity.AuditFlowId && x.HostId == hostId)
                .ToListAsync();

            return ObjectMapper.Map<List<AuditUserLogDto>>(list);
        }
    }

    public class AuditLogItem<TEntity>
    {
        public AuditUserLog Log { get; set; }

        public TEntity Entity { get; set; }
    }
}