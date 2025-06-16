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
        public static readonly string AuthorizationEndpoint = "https://oapi.dingtalk.com/connect/qrconnect";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public static readonly string TokenEndpoint = "https://oapi.dingtalk.com/gettoken";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://oapi.dingtalk.com/topapi/v2/user/get";

        public static readonly string UserInformationByCodeEndpoint = "https://oapi.dingtalk.com/sns/getuserinfo_bycode";

        public static readonly string UserIdByUnionidEndpoint = "https://oapi.dingtalk.com/topapi/user/getbyunionid";
    }

}
