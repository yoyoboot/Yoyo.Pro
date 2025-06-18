using Yoyo.Pro.ExternalAuth.OAuth;

using Microsoft.Extensions.Configuration;

using System.Collections.Generic;

namespace Yoyo.Pro.ExternalAuth.DingTalk
{
    public class DingTalkProviderInfo : OAuthProviderInfo
    {
        public DingTalkProviderInfo()
        {
            AuthorizationEndpoint = DingTalkAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = DingTalkAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = DingTalkAuthenticationDefaults.UserInformationEndpoint;


        }

        public static DingTalkProviderInfo CreateByConfiguration(IConfigurationSection configurationSection)
        {
            var instance = new DingTalkProviderInfo();
            configurationSection.Bind(instance);

            instance.ClaimsMapping = new List<ExternalAuthClaimsMapping>();
            configurationSection.Bind("ClaimsMapping", instance.ClaimsMapping);

            return instance;
        }

        public static DingTalkProviderInfo CreateByInfo(IExternalAuthProviderInfo info)
        {
            var instance = new DingTalkProviderInfo();

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
