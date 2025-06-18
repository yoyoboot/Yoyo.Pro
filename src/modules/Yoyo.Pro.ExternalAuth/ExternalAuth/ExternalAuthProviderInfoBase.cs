using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;

namespace Yoyo.Pro.ExternalAuth
{
    /// <summary>
    /// 扩展登录配置信息基类
    /// </summary>
    public abstract class ExternalAuthProviderInfoBase : IExternalAuthProviderInfo
    {

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 认证名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 要求https
        /// </summary>
        public bool RequireHttpsMetadata { get; set; }

        /// <summary>
        /// Claim映射
        /// </summary>
        public List<ExternalAuthClaimsMapping> ClaimsMapping { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public Dictionary<string, string> AdditionalParams { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        public string TenancyName { get; set; }


        #region 构造函数，配置默认值


        public ExternalAuthProviderInfoBase()
        {
            RequireHttpsMetadata = true;
            ClaimsMapping = new List<ExternalAuthClaimsMapping>();
            AdditionalParams = new Dictionary<string, string>();
        }


        #endregion


        #region 增强函数

        public virtual string GetValueOrDefault(string key)
        {
            if (this.AdditionalParams.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
        }

        public virtual T GetValueOrDefault<T>(string key)
        {
            if (this.AdditionalParams.TryGetValue(key, out var value))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }

            return default;
        }

        public virtual void SetValue(string key, string value)
        {
            this.AdditionalParams[key] = value;
        }

        public virtual void SetValue<T>(string key, T value)
        {
            this.AdditionalParams[key] = value.ToString();
        }

        #endregion


        #region 重写

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }



        #endregion
    }
}
