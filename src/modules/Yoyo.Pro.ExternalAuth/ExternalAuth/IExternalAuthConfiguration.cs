using System.Collections.Generic;

namespace Yoyo.Pro.ExternalAuth
{
    public interface IExternalAuthConfiguration
    {
        /// <summary>
        /// 添加配置信息
        /// </summary>
        /// <param name="providerInfo"></param>
        /// <returns></returns>
        IExternalAuthConfiguration AddProviderInfo(IExternalAuthProviderInfo providerInfo);

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<IExternalAuthProviderInfo> GetProviderInfos();


        bool TryGetProviderInfo(string providerName,out IExternalAuthProviderInfo providerInfo);
    }
}
