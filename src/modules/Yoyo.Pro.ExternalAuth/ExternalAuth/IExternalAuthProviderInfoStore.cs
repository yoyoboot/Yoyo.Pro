using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Configuration.Provider;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using Abp.Configuration;

namespace Yoyo.Pro.ExternalAuth
{
    public interface IExternalAuthProviderInfoStore
    {
        /// <summary>
        /// 获取指定的扩展登录信息
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        Task<IExternalAuthProviderInfo> GetProviderInfo(string providerName, bool? enabled = true);

        /// <summary>
        /// 获取所有扩展登录信息
        /// </summary>
        /// <param name="enabled">启用的</param>
        /// <returns></returns>
        Task<List<IExternalAuthProviderInfo>> GetProviderInfos(bool? enabled = true);
    }


    public class ExternalAuthProviderInfoStore : IExternalAuthProviderInfoStore
    {
        readonly IServiceProvider _serviceProvider;
        readonly IExternalAuthConfiguration _externalAuthConfiguration;
        readonly Lazy<ISettingManager> _settingManager;

        public ExternalAuthProviderInfoStore(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _externalAuthConfiguration = serviceProvider.GetRequiredService<IExternalAuthConfiguration>();
            _settingManager = _serviceProvider.GetLazy<ISettingManager>();
        }

        public async Task<IExternalAuthProviderInfo> GetProviderInfo(string providerName, bool? enabled = true)
        {
            var providerInfoString = await _settingManager.Value.GetSettingValueAsync($"ExternalLoginProvider.{providerName}");
            if (string.IsNullOrWhiteSpace(providerInfoString))
            {
                //throw new Exception($"No valid provider could be found (1)！{providerName}");
                return null;
            }

            var providerInfo = SimpleExternalAuthProviderInfo.CreateByString(providerInfoString);

            if (!enabled.HasValue)
            {
                return providerInfo;
            }

            if (providerInfo.IsEnabled != enabled.Value)
            {
                //throw new Exception($"No valid provider could be found (2)！{providerName}");

                return null;
            }

            return providerInfo;
        }

        public async Task<List<IExternalAuthProviderInfo>> GetProviderInfos(bool? enabled = true)
        {
            var list = new List<IExternalAuthProviderInfo>();
            foreach (var item in _externalAuthConfiguration.GetProviderInfos())
            {
                var providerInfo = await GetProviderInfo(item.Name, enabled);
                if (providerInfo == null)
                {
                    continue;
                }
                list.Add(providerInfo);
            }
            return list;
        }
    }
}
