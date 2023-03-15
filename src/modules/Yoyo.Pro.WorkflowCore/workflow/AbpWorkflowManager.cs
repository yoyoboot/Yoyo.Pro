using Abp;
using System;
using Abp.UI;
using Abp.Json;
using Abp.Linq;
using System.Linq;
using Abp.Domain.Uow;
using System.Reflection;
using Abp.Domain.Services;
using WorkflowCore.Models;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using Abp.Domain.Repositories;
using System.Collections.Generic;
using Yoyo.Pro.WorkflowCore.StepBody;
using Yoyo.Pro.WorkflowCore.Persistence;
using WorkflowCore.Services.DefinitionStorage;
using WorkflowCore.Models.DefinitionStorage.v1;

namespace Yoyo.Pro.WorkflowCore.workflow
{
    public class AbpWorkflowManager : DomainService, IAbpWorkflowManager
    {
        protected readonly WorkflowDefinitionManager _workflowDefinitionManager;
        protected readonly IWorkflowHost _workflowHost;
        protected readonly IWorkflowController _workflowController;
        protected readonly IWorkflowRegistry _registry;
        public IAbpPersistenceProvider PersistenceProvider { get; }
        protected readonly ISearchIndex _searchService;
        protected readonly IDefinitionLoader _definitionLoader;
        protected readonly IRepository<PersistedWorkflowDefinition, string> _workflowDefinitionRepository;
        protected IReadOnlyCollection<AbpWorkflowStepBody> _stepBodys;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        public IQueryable<PersistedWorkflowDefinition> WorkflowDefinitions => _workflowDefinitionRepository.GetAll();

        private readonly IRepository<PersistedWorkflow, Guid> _workflowRepository;

        private readonly IRepository<PersistedExecutionPointer, string> _persistedExecutionPointerRepository;

        public IQueryable<PersistedWorkflow> PersistedWorkflows => _workflowRepository.GetAll();

        public IQueryable<PersistedExecutionPointer> ExecutionPointer => _persistedExecutionPointerRepository.GetAll();
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public AbpWorkflowManager(WorkflowDefinitionManager workflowDefinitionManager,
            IWorkflowHost workflowHost,
            IWorkflowController workflowController,
            IWorkflowRegistry registry,
            IAbpPersistenceProvider workflowStore,
            ISearchIndex searchService,
            IDefinitionLoader definitionLoader,
            IRepository<PersistedWorkflowDefinition, string> workflowDefinitionRepository,
            IAsyncQueryableExecuter asyncQueryableExecuter,
            IRepository<PersistedWorkflow, Guid> workflowRepository,
            IRepository<PersistedExecutionPointer, string> persistedExecutionPointerRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _workflowDefinitionManager = workflowDefinitionManager;
            _workflowHost = workflowHost;
            _workflowController = workflowController;
            _registry = registry;
            PersistenceProvider = workflowStore;
            _searchService = searchService;
            _definitionLoader = definitionLoader;
            _workflowDefinitionRepository = workflowDefinitionRepository;
            _stepBodys = _workflowDefinitionManager.GetAllStepBodys();
            AsyncQueryableExecuter = asyncQueryableExecuter;
            _workflowRepository = workflowRepository;
            _persistedExecutionPointerRepository = persistedExecutionPointerRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// 终止流程
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public virtual async Task<bool> TerminateWorkflow(string workflowId)
        {
            return await _workflowController.TerminateWorkflow(workflowId);
        }

        public virtual WorkflowDefinition GetDefinition(string workflowId, int? version = null)
        {
            return _registry.GetDefinition(workflowId, version);
        }

        /// <summary>
        ///  初始化注册流程
        /// </summary>
        public void Initialize()
        {
            using (UnitOfWorkManager.Begin())
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var workflows = WorkflowDefinitions.ToList();
                foreach (var workflow in workflows)
                {
                    LoadDefinition(workflow);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<AbpWorkflowStepBody> GetAllStepBodys()
        {
            return _stepBodys;
        }

        public virtual async Task PublishEventAsync(string eventName, string eventKey, object eventData)
        {
            await _workflowHost.PublishEvent(eventName, eventKey, eventData);
        }

        /// <summary>
        /// 启动工作流
        /// </summary>
        /// <returns></returns>
        public virtual async Task StartWorkflow(string id, int version, Dictionary<string, object> inputs)
        {
            if (!_registry.IsRegistered(id, version))
            {
                throw new UserFriendlyException("the workflow  has not been defined!");
            }
            await _workflowHost.StartWorkflow(id, version, inputs);
        }

        /// <summary>
        /// 创建流程
        /// </summary>
        /// <returns></returns>
        public virtual async Task CreateAsync(PersistedWorkflowDefinition entity)
        {
            LoadDefinition(entity);
            await _workflowDefinitionRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(string id)
        {
            var entity = await _workflowDefinitionRepository.GetAsync(id);

            var all = await PersistenceProvider.GetAllRunnablePersistedWorkflow(entity.Id, entity.Version);
            if (all.Count() > 0)
            {
                throw new UserFriendlyException("删不了！！还有没有执行完的流程！");
            }

            if (_registry.IsRegistered(entity.Id, entity.Version))
            {
                _registry.DeregisterWorkflow(entity.Id, entity.Version);
            }

            await _workflowDefinitionRepository.DeleteAsync(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(PersistedWorkflowDefinition entity)
        {  
            // 构建和检查
            var defJson = GenAndCheck(entity);

            // 移除
            if (_registry.IsRegistered(entity.Id, entity.Version))
            {
                _registry.DeregisterWorkflow(entity.Id, entity.Version);
            }

            // 重新加载
            LoadDefinition(defJson);

            // 持久化
            await _workflowDefinitionRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 注册工作流
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startNodeKey">指定开始节点</param>
        /// <returns></returns>
        public WorkflowDefinition LoadDefinition(PersistedWorkflowDefinition input, string startNodeKey = null)
        {
            var json = GenAndCheck(input, startNodeKey);

            if (startNodeKey == null)
            {
                var def = LoadDefinition(json);

                return def;
            }
            else
            {
                var convert = typeof(DefinitionLoader).GetMethod("Convert", BindingFlags.NonPublic | BindingFlags.Instance);

                WorkflowDefinition def = null;
                if (convert is not null)
                {
                    var sourceObj = getDefinitionSourceV1(json, Deserializers.Json);

                    def = (WorkflowDefinition)convert.Invoke(new DefinitionLoader(_registry), new[] { sourceObj });
                }

                return def;
            }
        }

        private DefinitionSourceV1 getDefinitionSourceV1(string source, Func<string, DefinitionSourceV1> deserializer)
        {
            return deserializer(source);
        }

        /// <summary>
        /// 获取流程的JSON
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startNodeKey">指定初始化的起始节点</param>
        /// <returns></returns>
        protected virtual string GenAndCheck(PersistedWorkflowDefinition input, string startNodeKey = null)
        {
            var source = new DefinitionSourceV1();
            source.Id = input.Id;
            source.Version = input.Version;
            source.Description = input.Title;
            source.DataType = $"{typeof(Dictionary<string, object>).FullName}, {typeof(Dictionary<string, object>).Assembly.FullName}";

            var startNode = input.Nodes.FirstOrDefault(x => startNodeKey == null ? x.StartNode : x.Key == startNodeKey);
            if (startNode == null)
            {
                throw new AbpException("没有设置开始节点");
            }
            BuildWorkflow(input.Nodes, source, _stepBodys, startNode);
            var json = source.ToJsonString();
            Logger.DebugFormat("Workflow Json:{0}", json);

            return json;
        }

        internal WorkflowDefinition LoadDefinition(string defJson)
        {
            var def = _definitionLoader.LoadDefinition(defJson, Deserializers.Json);

            return def;
        }




        protected virtual void BuildWorkflow(IEnumerable<WorkflowNode> allNodes, DefinitionSourceV1 source, IEnumerable<AbpWorkflowStepBody> stepBodys, WorkflowNode node)
        {
            var stepBobyList = stepBodys.ToList();

            if (source.Steps.Any(u => u.Id == node.Key))
            {
                return;
            }

            var stepSource = new StepSourceV1();
            stepSource.Id = node.Key;
            stepSource.Name = node.Key;

            AbpWorkflowStepBody stepBody = null;
            if (node.StepBody != null)
            {
                stepBody = stepBobyList.FirstOrDefault(u => u.Name == node.StepBody.Name);
            }
            stepBody ??= new AbpWorkflowStepBody() { StepBodyType = typeof(NullStepBody) };
            stepSource.StepType = $"{stepBody.StepBodyType.FullName}, {stepBody.StepBodyType.Assembly.FullName}";

            foreach (var input in stepBody.Inputs)
            {
                if (node.StepBody != null)
                {
                    var value = node.StepBody.Inputs[input.Key].Value;
                    if (!(value is IDictionary<string, object> || value is IDictionary<object, object>))
                    {
                        value = $"\"{value}\"";
                    }
                    stepSource.Inputs.TryAdd(input.Key, value);
                }
            }
            source.Steps.Add(stepSource);
            BuildBranching(allNodes, source, stepSource, stepBobyList, node.NextNodes);
        }

        protected virtual void BuildBranching(IEnumerable<WorkflowNode> allNodes, DefinitionSourceV1 source, StepSourceV1 stepSource, IEnumerable<AbpWorkflowStepBody> stepBodys, IEnumerable<WorkflowConditionNode> nodes)
        {
            var allNodeList = allNodes.ToList();
            var stepBodyList = stepBodys.ToList();
            foreach (var nextNode in nodes)
            {
                var node = allNodeList.First(u => u.Key == nextNode.NodeId);
                stepSource.SelectNextStep[nextNode.NodeId] = "1==1";
                if (nextNode.Conditions.Any())
                {
                    var exps = new List<string>();
                    foreach (var cond in nextNode.Conditions)
                    {
                        if (cond.Value is string && (!decimal.TryParse(cond.Value.ToString(), out _)))
                        {
                            if (cond.Operator != "==" && cond.Operator != "!=")
                            {
                                throw new AbpException($" if {cond.Field} is type of 'String', the Operator must be \"==\" or \"!=\"");
                            }
                            exps.Add($"data[\"{cond.Field}\"].ToString() {cond.Operator} \"{cond.Value}\"");
                            continue;
                        }
                        exps.Add($"decimal.Parse(data[\"{cond.Field}\"].ToString()) {cond.Operator} {cond.Value}");
                    }
                    stepSource.SelectNextStep[nextNode.NodeId] = string.Join(" && ", exps);
                }

                BuildWorkflow(allNodeList, source, stepBodyList, node);
            }
        }
    }
}
