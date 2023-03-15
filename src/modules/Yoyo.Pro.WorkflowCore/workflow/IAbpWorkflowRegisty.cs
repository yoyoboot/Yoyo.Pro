using System;

namespace Yoyo.Pro.WorkflowCore.workflow
{
    public interface IAbpWorkflowRegistry
    {
        void RegisterWorkflow(Type type);
    }
}
