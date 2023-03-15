using System;
using Abp;
using Abp.Dependency;
using WorkflowCore.Interface;

namespace Yoyo.Pro.WorkflowCore.workflow
{
    public class AbpWorkflowRegistry : IAbpWorkflowRegistry, ISingletonDependency
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IIocManager _iocManager;

        public AbpWorkflowRegistry(IWorkflowRegistry workflowRegistry, IIocManager iocManager)
        {
            this._workflowRegistry = workflowRegistry;
            this._iocManager = iocManager;
        }

        public void RegisterWorkflow(Type type)
        {
            var workflow = _iocManager.Resolve(type);
            if (!(workflow is IAbpWorkflow))
            {
                throw new AbpException("RegistType must implement from AbpWorkflow!");
            }
            _workflowRegistry.RegisterWorkflow(workflow as IWorkflow<WorkflowParamDictionary>);
        }
    }
}
