using System.Collections.Generic;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Yoyo.Pro.WorkflowCore
{
    public class PersistedWorkflowDefinition : FullAuditedEntity<string>, IMayHaveTenant
    {
        public string Title { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }

        public string Group { get; set; }

        /// <summary>
        /// 动态表单
        /// </summary>
        public byte[] Inputs { get; set; }

        /// <summary>
        /// 流程节点
        /// </summary>
        public List<WorkflowNode> Nodes { get; set; }

        public string TenantId { get; set; }
    }
}
