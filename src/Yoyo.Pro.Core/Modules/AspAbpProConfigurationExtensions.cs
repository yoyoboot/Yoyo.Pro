using Abp.Configuration.Startup;

namespace Yoyo.Pro.Modules
{
    public static class AspAbpProConfigurationExtensions
    {
        public static AspAbpProConfiguration AbpPro(this IModuleConfigurations modules)
        {
            return modules.AbpConfiguration.Get<AspAbpProConfiguration>();
        }
    }
}
