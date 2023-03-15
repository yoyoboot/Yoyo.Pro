using Abp.Collections;
using Abp.Dependency;

namespace Yoyo.Pro.WorkflowCore.StepBody
{
    public interface IAbpStepBodyConfiguration
    {
        /// <summary>
        ///
        /// </summary>
        ITypeList<AbpStepBodyProvider> Providers { get; }
    }

    internal class AbpStepBodyConfiguration : IAbpStepBodyConfiguration, ISingletonDependency
    {
        public ITypeList<AbpStepBodyProvider> Providers { get; }

        public bool IsEnabled { get; set; }

        public AbpStepBodyConfiguration()
        {
            Providers = new TypeList<AbpStepBodyProvider>();
            IsEnabled = true;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class AbpStepBodyProvider : ITransientDependency
    {
        /// <summary>
        /// 设置码表类型
        /// </summary>
        /// <param name="context"></param>
        public abstract void Build(IAbpStepBodyDefinitionContext context);
    }
}
