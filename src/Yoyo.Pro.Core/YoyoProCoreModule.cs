using System.Reflection;

using Abp.Modules;
using Abp.Zero;
using Yoyo.Pro.Localization;
using Yoyo.Pro.Modules.DynamicView.DomainService;

namespace Yoyo.Pro
{


    [DependsOn(typeof(AbpZeroCoreModule))]
    public class YoyoProCoreModule : AbpModule
    {


        public override void PreInitialize()
        {
            Configuration.Localization.AddYoyoProLocalization();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

        }

        public override void PostInitialize()
        {
            var dynamicViewManager = IocManager.IocContainer.Resolve<IDynamicViewManager>();
            dynamicViewManager.AddEnum(GetType().Assembly);
        }


    }
}
