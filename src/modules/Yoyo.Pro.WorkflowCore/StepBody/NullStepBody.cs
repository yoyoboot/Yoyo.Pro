using Abp.Dependency;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Yoyo.Pro.WorkflowCore.StepBody
{
    public class NullStepBody : global::WorkflowCore.Models.StepBody, ITransientDependency
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            return ExecutionResult.Next();
        }
    }
}
