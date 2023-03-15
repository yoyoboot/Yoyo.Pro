using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.Pro.WorkflowCore.StepBody;

namespace Yoyo.Pro.WorkflowCore.workflow
{
    public interface IAbpWorkflowManager
    {
        Task<bool> TerminateWorkflow(string workflowId);

        IEnumerable<AbpWorkflowStepBody> GetAllStepBodys();

        Task PublishEventAsync(string eventName, string eventKey, object eventData);

        Task CreateAsync(PersistedWorkflowDefinition entity);

        Task DeleteAsync(string id);

        Task UpdateAsync(PersistedWorkflowDefinition entity);
    }
}
