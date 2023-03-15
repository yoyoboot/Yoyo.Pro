using Abp.Configuration.Startup;

namespace Yoyo.Pro.Configuration
{

    /// <summary>
    /// YoyoPro 对 Configuration 的扩展，用于配置 efcore
    /// </summary>
    public static class YoyoProEfCoreConfigurationExtensions
    {
        /// <summary>
        /// Used to configure YoyoPro EntityFramework Core module.
        /// </summary>
        public static IYoyoProEfCoreConfiguration YoyoProEfCore(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IYoyoProEfCoreConfiguration>();
        }
    }

}
