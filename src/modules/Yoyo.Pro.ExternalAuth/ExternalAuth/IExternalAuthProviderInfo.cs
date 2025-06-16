using System.Collections.Generic;

namespace Yoyo.Pro.ExternalAuth
{
    /// <summary>
    /// 扩展登录配置信息接口
    /// </summary>
    public interface IExternalAuthProviderInfo
    {
        /// <summary>
        /// 图标
        /// </summary>
        string Icon { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 认证名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 要求https
        /// </summary>
        bool RequireHttpsMetadata { get; set; }

        /// <summary>
        /// Claim映射
        /// </summary>
        List<ExternalAuthClaimsMapping> ClaimsMapping { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        Dictionary<string, string> AdditionalParams { get; set; }

        string GetValueOrDefault(string key);

        T GetValueOrDefault<T>(string key);

        void SetValue(string key, string value);

        void SetValue<T>(string key, T value);
    }
}
