using Abp.Configuration.Startup;

namespace Yoyo.Pro.Configuration
{

    /// <summary>
    /// YoyoPro 对 Configuration 的扩展，用于配置 efcore
    /// </summary>
    public static class L52AuditingConfigurationExtensions
    {
        /// <summary>
        /// Used to configure YoyoPro Auditing module.
        /// </summary>
        public static IYoyoProAuditingConfiguration YoyoProAuditing(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IYoyoProAuditingConfiguration>();
        }
    }

}
