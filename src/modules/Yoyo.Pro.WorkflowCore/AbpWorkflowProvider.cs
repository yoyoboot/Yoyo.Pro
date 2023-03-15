using Abp.Dependency;
using WorkflowCore.Interface;

namespace Yoyo.Pro.WorkflowCore
{
    public abstract class AbpWorkflowProvider : ITransientDependency
    {
        /// <summary>
        /// 设置码表类型
        /// </summary>
        /// <param name="context"></param>
        public abstract void Builds(IWorkflowBuilder context);
    }
}
