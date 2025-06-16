using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;


using Microsoft.AspNetCore.Authentication;
using static Yoyo.Pro.ExternalAuth.DingTalk.DingTalkAuthenticationConstants;

namespace Yoyo.Pro.ExternalAuth.DingTalk
{
    /// <summary>
    /// Defines a set of options used by <see cref="DingTalkAuthenticationHandler"/>.
    /// </summary>
    public class DingTalkAuthenticationOptions : OAuthOptions
    {
        public DingTalkAuthenticationOptions()
        {
            ClaimsIssuer = DingTalkAuthenticationDefaults.Issuer;
            CallbackPath = DingTalkAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = DingTalkAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = DingTalkAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = DingTalkAuthenticationDefaults.UserInformationEndpoint;

            UserInformationByCodeEndpoint = DingTalkAuthenticationDefaults.UserInformationByCodeEndpoint;
            UserIdByUnionidEndpoint = DingTalkAuthenticationDefaults.UserIdByUnionidEndpoint;

            Scope.Add("snsapi_login");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "unionid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nick");
            ClaimActions.MapJsonKey(Claims.UnionId, "unionid");

            IsEmployee = false;
        }


        public string UserInformationByCodeEndpoint { get; set; }

        public string UserIdByUnionidEndpoint { get; set; }

        public bool IsEmployee { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }
    }
}
