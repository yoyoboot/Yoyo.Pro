using Abp;
using System;
using Abp.Linq;
using System.Linq;
using Abp.Domain.Uow;
using System.Threading;
using System.Transactions;
using Abp.Domain.Services;
using WorkflowCore.Models;
using Yoyo.Pro.WorkflowCore;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Yoyo.Pro.WorkflowCore.Persistence;

namespace Yoyo.Pro.WorkflowCore.EntityFrameworkCore
{
    public class AbpPersistenceProvider : DomainService, IAbpPersistenceProvider
    {
        protected readonly IRepository<PersistedEvent, Guid> _eventRepository;
        protected readonly IRepository<PersistedExecutionPointer, string> _executionPointerRepository;
        protected readonly IRepository<PersistedWorkflow, Guid> _workflowRepository;
        protected readonly IRepository<PersistedWorkflowDefinition, string> _workflowDefinitionRepository;
        protected readonly IRepository<PersistedSubscription, Guid> _eventSubscriptionRepository;
        protected readonly IRepository<PersistedExecutionError, Guid> _executionErrorRepository;
        protected readonly IGuidGenerator _guidGenerator;
        protected readonly IAsyncQueryableExecuter _asyncQueryableExecuter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AbpPersistenceProvider(IRepository<PersistedEvent, Guid> eventRepository,
            IRepository<PersistedExecutionPointer, string> executionPointerRepository,
            IRepository<PersistedWorkflow, Guid> workflowRepository,
            IRepository<PersistedSubscription, Guid> eventSubscriptionRepository, IGuidGenerator guidGenerator,
            IAsyncQueryableExecuter asyncQueryableExecuter,
            IRepository<PersistedExecutionError, Guid> executionErrorRepository,
            IRepository<PersistedWorkflowDefinition, string> workflowDefinitionRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _eventRepository = eventRepository;
            _executionPointerRepository = executionPointerRepository;
            _workflowRepository = workflowRepository;
            _eventSubscriptionRepository = eventSubscriptionRepository;
            _guidGenerator = guidGenerator;
            _asyncQueryableExecuter = asyncQueryableExecuter;
            _executionErrorRepository = executionErrorRepository;
            _workflowDefinitionRepository = workflowDefinitionRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// 工作单元
        /// 解决持久化bug Enumerator failed to MoveNextAsync.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected async Task<T> UowWork<T>(Func<Task<T>> func)
        {
            using var uow = this.UnitOfWorkManager.Begin(TransactionScopeOption.Suppress);
            T res = await func.Invoke();

            await uow.CompleteAsync();

            return res;
        }

        protected async Task UowWork(Func<Task> func)
        {
            using var uow = this.UnitOfWorkManager.Begin(TransactionScopeOption.Suppress);
            await func.Invoke();
            await uow.CompleteAsync();
        }


        public virtual async Task<string> CreateEventSubscription(EventSubscription subscription, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                subscription.Id = _guidGenerator.Create().ToString();

                var persistable = subscription.ToPersistable();

                await _eventSubscriptionRepository.InsertAsync(persistable);

                return subscription.Id;
            });
        }


        public virtual async Task<string> CreateNewWorkflow(WorkflowInstance workflow, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                workflow.Id = _guidGenerator.Create().ToString();
                var persistable = workflow.ToPersistable();

                await _workflowRepository.InsertAsync(persistable);


                return workflow.Id;
            });
        }


        public virtual async Task<IEnumerable<string>> GetRunnableInstances(DateTime asAt, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var now = asAt.ToUniversalTime().Ticks;

                var query = _workflowRepository.GetAll().Where(x =>
                        x.NextExecution.HasValue && (x.NextExecution <= now) && (x.Status == WorkflowStatus.Runnable))
                    .Select(x => x.Id);
                var raw = await _asyncQueryableExecuter.ToListAsync(query);

                return raw.Select(s => s.ToString()).ToList();
            });

        }


        public virtual async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(WorkflowStatus? status,
            string type, DateTime? createdFrom, DateTime? createdTo, int skip, int take)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var query = _workflowRepository.GetAll()
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
            });

        }


        public virtual async Task<WorkflowInstance> GetWorkflowInstance(string id, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var uid = new Guid(id);
                var raw = await _workflowRepository.GetAll()
                    .Include(wf => wf.ExecutionPointers)
                    .ThenInclude(ep => ep.ExtensionAttributes)
                    .Include(wf => wf.ExecutionPointers)
                    .FirstOrDefaultAsync(x => x.Id == uid);

                if (raw == null)
                    return null;

                return raw.ToWorkflowInstance();
            });

        }


        public virtual async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                if (ids == null)
                {
                    return new List<WorkflowInstance>();
                }
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);

                var uids = ids.Select(i => new Guid(i));
                var raw = _workflowRepository.GetAll()
                    .Include(wf => wf.ExecutionPointers)
                    .ThenInclude(ep => ep.ExtensionAttributes)
                    .Include(wf => wf.ExecutionPointers)
                    .Where(x => uids.Contains(x.Id));

                return (await raw.ToListAsync()).Select(i => i.ToWorkflowInstance());
            });

        }


        public virtual async Task PersistWorkflow(WorkflowInstance workflow, CancellationToken cancellationToken = default)
        {
            await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var uid = new Guid(workflow.Id);
                var existingEntity = await _workflowRepository.GetAll()
                    .Where(x => x.Id == uid)
                    .Include(wf => wf.ExecutionPointers)
                    .ThenInclude(ep => ep.ExtensionAttributes)
                    .Include(wf => wf.ExecutionPointers)
                    .AsTracking()
                    .FirstAsync();
                workflow.ToPersistable(existingEntity);
            });
        }


        public virtual async Task TerminateSubscription(string eventSubscriptionId, CancellationToken cancellationToken = default)
        {
            await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var uid = new Guid(eventSubscriptionId);
                var existing = await _eventSubscriptionRepository.FirstOrDefaultAsync(x => x.Id == uid);
                _eventSubscriptionRepository.Delete(existing);
            });
        }


        public virtual void EnsureStoreExists()
        {
        }


        public virtual async Task<IEnumerable<EventSubscription>> GetSubscriptions(string eventName, string eventKey,
            DateTime asOf, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                asOf = asOf.ToUniversalTime();
                var raw = await _eventSubscriptionRepository.GetAll()
                    .Where(x => x.EventName == eventName && x.EventKey == eventKey && x.SubscribeAsOf <= asOf)
                    .ToListAsync();

                return raw.Select(item => item.ToEventSubscription()).ToList();
            });

        }


        public virtual async Task<string> CreateEvent(Event newEvent, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                newEvent.Id = _guidGenerator.Create().ToString();
                var persistable = newEvent.ToPersistable();
                await _eventRepository.InsertAsync(persistable);
                return newEvent.Id;
            });
        }


        public virtual async Task<Event> GetEvent(string id, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                Guid uid = new Guid(id);
                var raw = await _eventRepository
                    .FirstOrDefaultAsync(x => x.Id == uid);

                if (raw == null)
                    return null;

                return raw.ToEvent();
            });

        }


        public virtual async Task<IEnumerable<string>> GetRunnableEvents(DateTime asAt, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var now = asAt.ToUniversalTime();

                asAt = asAt.ToUniversalTime();
                var raw = await _eventRepository.GetAll()
                    .Where(x => !x.IsProcessed)
                    .Where(x => x.EventTime <= now)
                    .Select(x => x.Id)
                    .ToListAsync();

                return raw.Select(s => s.ToString()).ToList();
            });

        }


        public virtual async Task MarkEventProcessed(string id, CancellationToken cancellationToken = default)
        {
            await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var uid = new Guid(id);
                var existingEntity = await _eventRepository.GetAll()
                    .Where(x => x.Id == uid)
                    .AsTracking()
                    .FirstAsync();

                existingEntity.IsProcessed = true;
            });
        }


        public virtual async Task<IEnumerable<string>> GetEvents(string eventName, string eventKey, DateTime asOf, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var raw = await _eventRepository.GetAll()
                    .Where(x => x.EventName == eventName && x.EventKey == eventKey)
                    // .Where(x => x.EventTime >= asOf)
                    .Select(x => x.Id)
                    .ToListAsync();

                var result = new List<string>();

                foreach (var s in raw)
                    result.Add(s.ToString());

                return result;
            });

        }


        public virtual async Task MarkEventUnprocessed(string id, CancellationToken cancellationToken = default)
        {
            await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var uid = new Guid(id);
                var existingEntity = await _eventRepository.GetAll()
                    .Where(x => x.Id == uid)
                    .AsTracking()
                    .FirstAsync();

                existingEntity.IsProcessed = false;
            });

        }


        public virtual async Task PersistErrors(IEnumerable<ExecutionError> errors, CancellationToken cancellationToken = default)
        {
            await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var executionErrors = errors as ExecutionError[] ?? errors.ToArray();
                if (executionErrors.Any())
                {
                    foreach (var error in executionErrors)
                    {
                        await _executionErrorRepository.InsertAsync(error.ToPersistable());
                    }

                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            });

        }


        public virtual async Task<EventSubscription> GetSubscription(string eventSubscriptionId, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var uid = new Guid(eventSubscriptionId);
                var raw = await _eventSubscriptionRepository.FirstOrDefaultAsync(x => x.Id == uid);

                return raw?.ToEventSubscription();
            });

        }


        public virtual async Task<EventSubscription> GetFirstOpenSubscription(string eventName, string eventKey,
            DateTime asOf, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var raw = await _eventSubscriptionRepository.FirstOrDefaultAsync(x =>
                    x.EventName == eventName && x.EventKey == eventKey && x.SubscribeAsOf <= asOf &&
                    x.ExternalToken == null);

                return raw?.ToEventSubscription();
            });

        }


        public virtual async Task<bool> SetSubscriptionToken(string eventSubscriptionId, string token, string workerId,
            DateTime expiry, CancellationToken cancellationToken = default)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var uid = new Guid(eventSubscriptionId);
                var existingEntity = await _eventSubscriptionRepository.GetAll()
                    .Where(x => x.Id == uid)
                    .AsTracking()
                    .FirstAsync();

                existingEntity.ExternalToken = token;
                existingEntity.ExternalWorkerId = workerId;
                existingEntity.ExternalTokenExpiry = expiry;
                await CurrentUnitOfWork.SaveChangesAsync();

                return true;
            });

        }


        public virtual async Task ClearSubscriptionToken(string eventSubscriptionId, string token, CancellationToken cancellationToken = default)
        {
            await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var uid = new Guid(eventSubscriptionId);
                var existingEntity = await _eventSubscriptionRepository.GetAll()
                    .Where(x => x.Id == uid)
                    .AsTracking()
                    .FirstAsync();

                if (existingEntity.ExternalToken != token)
                    throw new InvalidOperationException();

                existingEntity.ExternalToken = null;
                existingEntity.ExternalWorkerId = null;
                existingEntity.ExternalTokenExpiry = null;
            });
        }

        public virtual async Task<PersistedWorkflow> GetPersistedWorkflow(Guid id)
        {
            return await UowWork(async () => await _workflowRepository.GetAsync(id));
        }

        public async Task<PersistedWorkflowDefinition> GetPersistedWorkflowDefinition(string id, int version)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                return await _workflowDefinitionRepository.GetAll().AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id && u.Version == version);
            });

        }

        public async Task<PersistedExecutionPointer> GetPersistedExecutionPointer(string id)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                return await _executionPointerRepository.GetAsync(id);
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="definitionId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PersistedWorkflow>> GetAllRunnablePersistedWorkflow(string definitionId,
            int version)
        {
            return await UowWork(async () =>
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                return await _workflowRepository.GetAll()
                    .Where(u => u.WorkflowDefinitionId == definitionId && u.Version == version).ToListAsync();
            });

        }


        public async Task ScheduleCommand(ScheduledCommand command)
        {
            await Task.CompletedTask;
        }

        public async Task ProcessCommands(DateTimeOffset asOf, Func<ScheduledCommand, Task> action, CancellationToken cancellationToken = new CancellationToken())
        {
            await Task.CompletedTask;
        }

        public bool SupportsScheduledCommands { get; }

    }
}
