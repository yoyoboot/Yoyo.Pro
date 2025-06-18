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

            Scope.Add("snsapi_login");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "unionid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nick");
            ClaimActions.MapJsonKey(Claims.UnionId, "unionid");
        }


        public string UserInformationByCodeEndpoint { get; set; }

        public string UserIdByUnionidEndpoint { get; set; }
    }
}
