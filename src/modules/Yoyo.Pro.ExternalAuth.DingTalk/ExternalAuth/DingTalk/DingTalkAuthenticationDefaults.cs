using System.Collections.Generic;

namespace Yoyo.Pro.ExternalAuth.DingTalk
{
    /// <summary>
    /// Default values for DingTalk authentication.
    /// </summary>
    public static class DingTalkAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "DingTalk";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public static readonly string DisplayName = "DingTalk";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public static readonly string CallbackPath = "/signin-dingtalk";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public static readonly string Issuer = "DingTalk";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://login.dingtalk.com/oauth2/auth";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public static readonly string TokenEndpoint = "https://api.dingtalk.com/v1.0/oauth2/userAccessToken";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://api.dingtalk.com/v1.0/contact/users/me";

        /// <summary>
        /// 默认授权范围
        /// </summary>
        public static List<string> ScopeList { get; set; } = new List<string>()
        {
            "snsapi_login"
        };

        /// <summary>
        /// 登录认证方式
        /// </summary>
        public static string Prompt { get; set; } = "login consent";
    }

}
