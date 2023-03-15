using System.Collections.Generic;

namespace Yoyo.Pro.WorkflowCore
{
    public class WorkflowNode
    {
        public string Key { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// 节点的描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否是开始节点
        /// </summary>
        public bool StartNode { get; set; }

        /// <summary>
        /// 是否是结束节点
        /// </summary>
        public bool EndNode { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public long[] Position { get; set; }

        /// <summary>
        /// 类型[left,top]
        /// </summary>
        public string Type { get; set; }

        public AbpStepBodyInput StepBody { get; set; }
        public IEnumerable<string> ParentNodes { get; set; }
        public IEnumerable<WorkflowConditionNode> NextNodes { get; set; }
    }

    public class AbpStepBodyInput
    {
        public string Name { get; set; }
        public Dictionary<string, WorkflowParamInput> Inputs { get; set; } = new Dictionary<string, WorkflowParamInput>();
    }

    public class WorkflowParamInput
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class WorkflowConditionNode
    {
        public string Label { get; set; }
        public string NodeId { get; set; }
        public IEnumerable<WorkflowConditionCondition> Conditions { get; set; } = new List<WorkflowConditionCondition>();
    }

    public class WorkflowConditionCondition
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
    }
}
