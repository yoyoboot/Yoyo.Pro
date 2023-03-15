using Abp.Modules;
using Abp.Reflection.Extensions;
using WorkflowCore.Interface;

namespace Yoyo.Pro.WorkflowCore
{
    public class AbpWorkflowCoreModule : AbpModule
    {
        public override void Initialize()
        {
            base.Initialize();
            IocManager.RegisterAssemblyByConvention(typeof(AbpWorkflowCoreModule).GetAssembly());
            IocManager.IocContainer.Install(new WorkflowInstaller());
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            IocManager.Resolve<WorkflowDefinitionManager>().Initialize();

            //移动至 FoundationEntityFrameworkModule进行初始化
            // 解决种子数据没初始化完就开始初始化工作流了
            // var host = IocManager.Resolve<IWorkflowHost>();
            //
            // host.Start();
            // IocManager.Resolve<AbpWorkflowManager>().Initialize();
        }

        public override void Shutdown()
        {
            base.Shutdown();
            IocManager.Resolve<IWorkflowHost>().Stop();
        }
    }
}
