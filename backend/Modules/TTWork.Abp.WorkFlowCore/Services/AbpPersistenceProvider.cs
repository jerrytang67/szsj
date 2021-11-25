using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Linq;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.WorkFlowCore.Interfaces;
using TTWork.Abp.WorkFlowCore.Models;
using WorkflowCore.Models;

namespace TTWork.Abp.WorkFlowCore.Services
{
    public class AbpPersistenceProvider : DomainService, IAbpPersistenceProvider
    {
        protected readonly IRepository<PersistedEvent, Guid> _eventRepository;
        protected readonly IRepository<PersistedExecutionPointer, string> _executionPointerRepository;
        protected readonly IRepository<PersistedWorkflow, Guid> _workflowRepository;
        protected readonly IRepository<PersistedWorkflowDefinition, string> _workflowDefinitionRepository;
        private readonly IRepository<User, long> _userRepository;
        protected readonly IRepository<PersistedSubscription, Guid> _eventSubscriptionRepository;
        protected readonly IRepository<PersistedExecutionError, Guid> _executionErrorRepository;
        protected readonly IGuidGenerator _guidGenerator;
        protected readonly IAsyncQueryableExecuter _asyncQueryableExecuter;
        public IAbpSession AbpSession { get; set; }

        public AbpPersistenceProvider(
            IRepository<PersistedEvent, Guid> eventRepository,
            IRepository<PersistedExecutionPointer, string> executionPointerRepository,
            IRepository<PersistedWorkflow, Guid> workflowRepository,
            IRepository<PersistedSubscription, Guid> eventSubscriptionRepository,
            IRepository<PersistedExecutionError, Guid> executionErrorRepository,
            IRepository<PersistedWorkflowDefinition, string> workflowDefinitionRepository,
            IRepository<User, long> userRepository,
            IGuidGenerator guidGenerator,
            IAsyncQueryableExecuter asyncQueryableExecuter)
        {
            _eventRepository = eventRepository;
            _executionPointerRepository = executionPointerRepository;
            _workflowRepository = workflowRepository;
            _eventSubscriptionRepository = eventSubscriptionRepository;
            _guidGenerator = guidGenerator;
            _asyncQueryableExecuter = asyncQueryableExecuter;
            _executionErrorRepository = executionErrorRepository;
            _workflowDefinitionRepository = workflowDefinitionRepository;
            _userRepository = userRepository;
        }

        [UnitOfWork]
        public virtual async Task<string> CreateEventSubscription(EventSubscription subscription, CancellationToken cancellationToken = new())
        {
            subscription.Id = _guidGenerator.Create().ToString();
            var persistable = subscription.ToPersistable();
            await _eventSubscriptionRepository.InsertAsync(persistable);
            return subscription.Id;
        }

        [UnitOfWork]
        public virtual async Task<string> CreateNewWorkflow(WorkflowInstance workflow, CancellationToken cancellationToken = new())
        {
            workflow.Id = _guidGenerator.Create().ToString();
            var persistable = workflow.ToPersistable();
            if (AbpSession.UserId.HasValue)
            {
                var userCache = await _userRepository.FirstOrDefaultAsync(x => x.Id == AbpSession.UserId.Value);
                persistable.CreateUserIdentityName = userCache.FullName;
            }

            await _workflowRepository.InsertAsync(persistable);
            return workflow.Id;
        }

        [UnitOfWork]
        public virtual async Task<IEnumerable<string>> GetRunnableInstances(DateTime asAt, CancellationToken cancellationToken = new())
        {
            var now = asAt.ToUniversalTime().Ticks;
            var query = _workflowRepository.GetAll().Where(x => x.NextExecution.HasValue && (x.NextExecution <= now) && (x.Status == WorkflowStatus.Runnable))
                .Select(x => x.Id);
            var raw = await _asyncQueryableExecuter.ToListAsync(query);

            return raw.Select(s => s.ToString()).ToList();
        }

        [UnitOfWork]
        public virtual async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(WorkflowStatus? status, string type, DateTime? createdFrom, DateTime? createdTo, int skip, int take)
        {
            IQueryable<PersistedWorkflow> query = _workflowRepository.GetAll()
                .Include(wf => wf.ExecutionPointers)
                .ThenInclude(ep => ep.ExtensionAttributes)
                .Include(wf => wf.ExecutionPointers)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (!String.IsNullOrEmpty(type))
                query = query.Where(x => x.WorkflowDefinitionId == type);

            if (createdFrom.HasValue)
                query = query.Where(x => x.CreateTime >= createdFrom.Value);

            if (createdTo.HasValue)
                query = query.Where(x => x.CreateTime <= createdTo.Value);

            var rawResult = await query.Skip(skip).Take(take).ToListAsync();
            var result = new List<WorkflowInstance>();

            foreach (var item in rawResult)
                result.Add(item.ToWorkflowInstance());

            return result;
        }

        [UnitOfWork]
        public virtual async Task<WorkflowInstance> GetWorkflowInstance(string Id, CancellationToken cancellationToken = new())
        {
            var uid = new Guid(Id);
            var raw = await _workflowRepository.GetAll()
                .Include(wf => wf.ExecutionPointers)
                .ThenInclude(ep => ep.ExtensionAttributes)
                .Include(wf => wf.ExecutionPointers)
                .FirstAsync(x => x.Id == uid, cancellationToken: cancellationToken);

            if (raw == null)
                return null;

            var ins = raw.ToWorkflowInstance();

            return ins;
        }

        [UnitOfWork]
        public virtual async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(IEnumerable<string> ids, CancellationToken cancellationToken = new())
        {
            if (ids == null)
            {
                return new List<WorkflowInstance>();
            }

            var uids = ids.Select(i => new Guid(i));
            var raw = _workflowRepository.GetAll()
                .Include(wf => wf.ExecutionPointers)
                .ThenInclude(ep => ep.ExtensionAttributes)
                .Include(wf => wf.ExecutionPointers)
                .Where(x => uids.Contains(x.Id));

            return (await raw.ToListAsync(cancellationToken: cancellationToken)).Select(i => i.ToWorkflowInstance());
        }

        [UnitOfWork]
        public virtual async Task PersistWorkflow(WorkflowInstance workflow, CancellationToken cancellationToken = new())
        {
            var uid = new Guid(workflow.Id);
            var existingEntity = await _workflowRepository.GetAll()
                .Where(x => x.Id == uid)
                .Include(wf => wf.ExecutionPointers)
                .ThenInclude(ep => ep.ExtensionAttributes)
                .Include(wf => wf.ExecutionPointers)
                .AsTracking()
                .FirstAsync(cancellationToken: cancellationToken);

            var persistable = workflow.ToPersistable(existingEntity);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [UnitOfWork]
        public virtual async Task TerminateSubscription(string eventSubscriptionId, CancellationToken cancellationToken = new())
        {
            var uid = new Guid(eventSubscriptionId);
            var existing = await _eventSubscriptionRepository.FirstOrDefaultAsync(x => x.Id == uid);
            await _eventSubscriptionRepository.DeleteAsync(existing);
            await CurrentUnitOfWork.SaveChangesAsync();
        }


        [UnitOfWork]
        public virtual void EnsureStoreExists()
        {
        }

        [UnitOfWork]
        public virtual async Task<IEnumerable<EventSubscription>> GetSubscriptions(string eventName, string eventKey, DateTime asOf, CancellationToken cancellationToken = new())
        {
            asOf = asOf.ToUniversalTime();
            var raw = await _eventSubscriptionRepository.GetAll()
                .Where(x => x.EventName == eventName && x.EventKey == eventKey && x.SubscribeAsOf <= asOf)
                .ToListAsync(cancellationToken: cancellationToken);

            return raw.Select(item => item.ToEventSubscription()).ToList();
        }

        [UnitOfWork]
        public virtual async Task<string> CreateEvent(Event newEvent, CancellationToken cancellationToken = new())
        {
            newEvent.Id = _guidGenerator.Create().ToString();
            var persistable = newEvent.ToPersistable();
            var result = await _eventRepository.InsertAsync(persistable);
            await CurrentUnitOfWork.SaveChangesAsync();
            return newEvent.Id;
        }

        [UnitOfWork]
        public virtual async Task<Event> GetEvent(string id, CancellationToken cancellationToken = new())
        {
            var uid = new Guid(id);
            var raw = await _eventRepository
                .FirstOrDefaultAsync(x => x.Id == uid);

            return raw?.ToEvent();
        }

        [UnitOfWork]
        public virtual async Task<IEnumerable<string>> GetRunnableEvents(DateTime asAt, CancellationToken cancellationToken = new())
        {
            // var now = asAt.ToUniversalTime();
            var now = asAt.ToUniversalTime();

            var raw = await _eventRepository.GetAll()
                .Where(x => !x.IsProcessed)
                .Where(x => x.EventTime <= now)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken: cancellationToken);

            return raw.Select(s => s.ToString()).ToList();
        }

        [UnitOfWork]
        public virtual async Task MarkEventProcessed(string id, CancellationToken cancellationToken = new())
        {
            var uid = new Guid(id);
            var existingEntity = await _eventRepository.GetAll()
                .Where(x => x.Id == uid)
                .AsTracking()
                .FirstAsync(cancellationToken: cancellationToken);

            existingEntity.IsProcessed = true;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [UnitOfWork]
        public virtual async Task<IEnumerable<string>> GetEvents(string eventName, string eventKey, DateTime asOf, CancellationToken cancellationToken = new())
        {
            var raw = await _eventRepository.GetAll()
                .Where(x => x.EventName == eventName && x.EventKey == eventKey)
                .Where(x => x.EventTime >= asOf)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken: cancellationToken);

            var result = new List<string>();

            foreach (var s in raw)
                result.Add(s.ToString());

            return result;
        }

        [UnitOfWork]
        public virtual async Task MarkEventUnprocessed(string id, CancellationToken cancellationToken = new())
        {
            var uid = new Guid(id);
            var existingEntity = await _eventRepository.GetAll()
                .Where(x => x.Id == uid)
                .AsTracking()
                .FirstAsync(cancellationToken: cancellationToken);

            existingEntity.IsProcessed = false;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [UnitOfWork]
        public virtual async Task PersistErrors(IEnumerable<ExecutionError> errors, CancellationToken cancellationToken = new())
        {
            var executionErrors = errors as ExecutionError[] ?? errors.ToArray();
            if (executionErrors.Any())
            {
                foreach (var error in executionErrors)
                {
                    await _executionErrorRepository.InsertAsync(error.ToPersistable());
                }

                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task<EventSubscription> GetSubscription(string eventSubscriptionId, CancellationToken cancellationToken = new())
        {
            var uid = new Guid(eventSubscriptionId);
            var raw = await _eventSubscriptionRepository.FirstOrDefaultAsync(x => x.Id == uid);

            return raw?.ToEventSubscription();
        }

        [UnitOfWork]
        public virtual async Task<EventSubscription> GetFirstOpenSubscription(string eventName, string eventKey, DateTime asOf, CancellationToken cancellationToken = new())
        {
            var raw = await _eventSubscriptionRepository.FirstOrDefaultAsync(x => x.EventName == eventName && x.EventKey == eventKey && x.SubscribeAsOf <= asOf && x.ExternalToken == null);

            return raw?.ToEventSubscription();
        }

        [UnitOfWork]
        public virtual async Task<bool> SetSubscriptionToken(string eventSubscriptionId, string token, string workerId, DateTime expiry, CancellationToken cancellationToken = new())
        {
            var uid = new Guid(eventSubscriptionId);
            var existingEntity = await _eventSubscriptionRepository.GetAll()
                .Where(x => x.Id == uid)
                .AsTracking()
                .FirstAsync(cancellationToken: cancellationToken);

            existingEntity.ExternalToken = token;
            existingEntity.ExternalWorkerId = workerId;
            existingEntity.ExternalTokenExpiry = expiry;
            await CurrentUnitOfWork.SaveChangesAsync();

            return true;
        }

        [UnitOfWork]
        public virtual async Task ClearSubscriptionToken(string eventSubscriptionId, string token, CancellationToken cancellationToken = new())
        {
            var uid = new Guid(eventSubscriptionId);
            var existingEntity = await _eventSubscriptionRepository.GetAll()
                .Where(x => x.Id == uid)
                .AsTracking()
                .FirstAsync(cancellationToken: cancellationToken);

            if (existingEntity.ExternalToken != token)
                throw new InvalidOperationException();

            existingEntity.ExternalToken = null;
            existingEntity.ExternalWorkerId = null;
            existingEntity.ExternalTokenExpiry = null;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public Task<PersistedWorkflow> GetPersistedWorkflow(Guid id)
        {
            return _workflowRepository.GetAsync(id);
        }

        public Task<PersistedWorkflowDefinition> GetPersistedWorkflowDefinition(string id, int version)
        {
            return _workflowDefinitionRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(u => u.Id == id && u.Version == version);
        }

        public Task<PersistedExecutionPointer> GetPersistedExecutionPointer(string id)
        {
            return _executionPointerRepository.GetAsync(id);
        }

        public Task ScheduleCommand(ScheduledCommand command)
        {
            throw new NotImplementedException();
        }

        public Task ProcessCommands(DateTimeOffset asOf, Func<ScheduledCommand, Task> action, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public bool SupportsScheduledCommands { get; }
    }
}