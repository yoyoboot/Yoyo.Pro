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

            UserInformationByCodeEndpoint = DingTalkAuthenticationDefaults.UserInformationByCodeEndpoint;
            UserIdByUnionidEndpoint = DingTalkAuthenticationDefaults.UserIdByUnionidEndpoint;

            IsEmployee = false;
        }

        public string UserInformationByCodeEndpoint
        {
            get => GetValueOrDefault(nameof(UserInformationByCodeEndpoint));
            set => SetValue(nameof(UserInformationByCodeEndpoint), value);
        }

        public string UserIdByUnionidEndpoint
        {
            get => GetValueOrDefault(nameof(UserIdByUnionidEndpoint));
            set => SetValue(nameof(UserIdByUnionidEndpoint), value);
        }

        public bool IsEmployee
        {
            get => GetValueOrDefault<bool>(nameof(IsEmployee));
            set => SetValue(nameof(IsEmployee), value);
        }

        public string AppId
        {
            get => GetValueOrDefault(nameof(AppId));
            set => SetValue(nameof(AppId), value);
        }
        
        public string AppSecret
        {
            get => GetValueOrDefault(nameof(AppSecret));
            set => SetValue(nameof(AppSecret), value);
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

            return instance;
        }
    }
}
