namespace Yoyo.Pro.ExternalAuth.FeiShu
{
    /// <summary>
    /// Default values for FeiShu authentication.
    /// </summary>
    public static class FeiShuAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "FeiShu";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public static readonly string DisplayName = "FeiShu";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public static readonly string CallbackPath = "/signin-feishu";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public static readonly string Issuer = "FeiShu";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        //public static readonly string AuthorizationEndpoint = "https://passport.feishu.cn/suite/passport/oauth/authorize";
        public static readonly string AuthorizationEndpoint = "https://accounts.feishu.cn/open-apis/authen/v1/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        //public static readonly string TokenEndpoint = "https://passport.feishu.cn/suite/passport/oauth/token";
        public static readonly string TokenEndpoint = "https://open.feishu.cn/open-apis/authen/v2/oauth/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        //public static readonly string UserInformationEndpoint = "https://passport.feishu.cn/suite/passport/oauth/userinfo";
        public static readonly string UserInformationEndpoint = "https://open.feishu.cn/open-apis/authen/v1/user_info";
    }

}
