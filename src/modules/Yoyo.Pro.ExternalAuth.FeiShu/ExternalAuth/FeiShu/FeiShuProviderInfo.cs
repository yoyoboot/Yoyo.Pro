using Yoyo.Pro.ExternalAuth.OAuth;

using Microsoft.Extensions.Configuration;

using System.Collections.Generic;

namespace Yoyo.Pro.ExternalAuth.FeiShu
{
    public class FeiShuProviderInfo : OAuthProviderInfo
    {
        public FeiShuProviderInfo()
        {
            AuthorizationEndpoint = FeiShuAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = FeiShuAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = FeiShuAuthenticationDefaults.UserInformationEndpoint;
        }


        public static FeiShuProviderInfo CreateByConfiguration(IConfigurationSection configurationSection)
        {
            var instance = new FeiShuProviderInfo();
            configurationSection.Bind(instance);

            instance.ClaimsMapping = new List<ExternalAuthClaimsMapping>();
            configurationSection.Bind("ClaimsMapping", instance.ClaimsMapping);

            return instance;
        }

        public static FeiShuProviderInfo CreateByInfo(IExternalAuthProviderInfo info)
        {
            var instance = new FeiShuProviderInfo();

            instance.Icon = info.Icon;
            instance.IsEnabled = info.IsEnabled;
            instance.Name = info.Name;
            instance.RequireHttpsMetadata = info.RequireHttpsMetadata;
            instance.ClaimsMapping = info.ClaimsMapping;
            instance.AdditionalParams = info.AdditionalParams;
            instance.TenancyName = info.TenancyName;

            return instance;
        }
    }
}
