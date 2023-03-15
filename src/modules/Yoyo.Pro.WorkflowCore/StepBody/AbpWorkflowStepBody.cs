using System;

namespace Yoyo.Pro.WorkflowCore.StepBody
{
    public class AbpWorkflowStepBody
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public WorkflowParamDictionary Inputs { get; set; } = new WorkflowParamDictionary();

        public Type StepBodyType { get; set; }
    }
}
