using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;


using Microsoft.AspNetCore.Authentication;
using static Yoyo.Pro.ExternalAuth.FeiShu.FeiShuAuthenticationConstants;

namespace Yoyo.Pro.ExternalAuth.FeiShu
{
    /// <summary>
    /// Defines a set of options used by <see cref="FeiShuAuthenticationHandler"/>.
    /// </summary>
    public class FeiShuAuthenticationOptions : OAuthOptions
    {
        public FeiShuAuthenticationOptions()
        {
            ClaimsIssuer = FeiShuAuthenticationDefaults.Issuer;
            CallbackPath = FeiShuAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = FeiShuAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = FeiShuAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = FeiShuAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(Claims.UnionId, "union_id");
            ClaimActions.MapJsonKey(Claims.Avatar, "avatar_big");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "open_id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.MobilePhone, "mobile");
        }
    }
}
