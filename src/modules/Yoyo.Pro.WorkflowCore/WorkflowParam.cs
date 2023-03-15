using Abp.UI.Inputs;

namespace Yoyo.Pro.WorkflowCore
{
    public class WorkflowParam
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IInputType InputType { get; set; }
        public object Value { get; set; }
    }
}
