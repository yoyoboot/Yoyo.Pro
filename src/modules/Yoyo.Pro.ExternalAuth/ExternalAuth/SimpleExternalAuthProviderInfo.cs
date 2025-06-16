using Newtonsoft.Json;

namespace Yoyo.Pro.ExternalAuth
{
    /// <summary>
    /// 简易扩展登录配置
    /// </summary>
    public class SimpleExternalAuthProviderInfo : ExternalAuthProviderInfoBase
    {        
        public static IExternalAuthProviderInfo CreateByString(string json)
        {
            return JsonConvert.DeserializeObject<SimpleExternalAuthProviderInfo>(json);
        }
    }
}
