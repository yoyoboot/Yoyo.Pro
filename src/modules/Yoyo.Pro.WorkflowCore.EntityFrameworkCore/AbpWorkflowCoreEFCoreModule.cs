using Abp.Modules;
using Yoyo.Pro.WorkflowCore;

namespace Yoyo.Pro.WorkflowCore.EntityFrameworkCore
{
    [DependsOn(typeof(AbpWorkflowCoreModule))]
    public class AbpWorkflowCoreEFCoreModule : AbpModule
    {
        public override void Initialize()
        {
            base.Initialize();
            IocManager.RegisterAssemblyByConvention(typeof(AbpWorkflowCoreEFCoreModule).Assembly);
        }
    }
}
