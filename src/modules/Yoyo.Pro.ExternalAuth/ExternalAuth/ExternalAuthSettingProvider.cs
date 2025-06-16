using Abp.Configuration;

using Microsoft.Extensions.Configuration;

using System.Collections.Generic;

namespace Yoyo.Pro.ExternalAuth
{
    public class ExternalAuthSettingProvider : SettingProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;

        public ExternalAuthSettingProvider(IConfiguration configuration = null, IExternalAuthConfiguration externalAuthConfiguration = null)
        {
            _configuration = configuration;
            _externalAuthConfiguration = externalAuthConfiguration;
        }

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            if (_configuration == null || _externalAuthConfiguration == null)
            {
                yield break;
            }
            else
            {
                var providerInfos = _externalAuthConfiguration.GetProviderInfos();
                foreach (var providerInfo in providerInfos)
                {
                    var aaa = providerInfo.ToString();

                    yield return new SettingDefinition(
                        $"ExternalLoginProvider.{providerInfo.Name}",
                        providerInfo.ToString(),
                        isVisibleToClients: false,
                        scopes: SettingScopes.Application | SettingScopes.Tenant
                        );
                }
            }
        }
    }
}
