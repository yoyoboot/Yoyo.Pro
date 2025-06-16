using Abp;

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Yoyo.Pro.ExternalAuth
{
    public class ExternalAuthConfiguration : IExternalAuthConfiguration
    {
        readonly IDictionary<string, IExternalAuthProviderInfo> _optoinsMap;


        public ExternalAuthConfiguration()
        {
            _optoinsMap = new Dictionary<string, IExternalAuthProviderInfo>();
        }


        public IExternalAuthConfiguration AddProviderInfo(IExternalAuthProviderInfo providerInfo)
        {
            Check.NotNull(providerInfo, nameof(providerInfo));
            Check.NotNullOrWhiteSpace(providerInfo.Name, nameof(providerInfo.Name));

            _optoinsMap[providerInfo.Name] = providerInfo;

            return this;
        }

        public IReadOnlyList<IExternalAuthProviderInfo> GetProviderInfos()
        {
            return _optoinsMap.Values.ToImmutableList();
        }

        public bool TryGetProviderInfo(string providerName, out IExternalAuthProviderInfo providerInfo)
        {
           return _optoinsMap.TryGetValue(providerName, out providerInfo);
        }
    }
}
