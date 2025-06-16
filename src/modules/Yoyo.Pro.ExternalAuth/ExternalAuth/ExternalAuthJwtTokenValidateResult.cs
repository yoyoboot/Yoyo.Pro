using Microsoft.IdentityModel.Tokens;

using System.Security.Claims;

namespace Yoyo.Pro.ExternalAuth
{
    public class ExternalAuthJwtTokenValidateResult
    {
        public ClaimsPrincipal Principal { get; set; }

        public SecurityToken ValidatedToken { get; set; }
    }
}
