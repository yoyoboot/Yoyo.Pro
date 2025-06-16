using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Yoyo.Pro.ExternalAuth
{
    public static class ExternalAuthClaimExtensions
    {
        public static Claim GetClaimByMapping(this ClaimsPrincipal principal, List<ExternalAuthClaimsMapping> mappings, string claim)
        {
            if (mappings == null || !mappings.Any())
            {
                goto end;
            }

            var claimMapping = mappings.FirstOrDefault(o => o.Claim == claim);
            if (claimMapping != null)
            {
                return principal.GetClaimByMapping(null, claimMapping.Key);
            }
        end:
            return principal.Claims.FirstOrDefault(o => o.Type == claim);
        }

        public static Claim GetClaimByMapping(this JwtSecurityToken token, List<ExternalAuthClaimsMapping> mappings, string claim)
        {
            if (mappings == null || !mappings.Any())
            {
                goto end;
            }

            var claimMapping = mappings.FirstOrDefault(o => o.Claim == claim);
            if (claimMapping != null)
            {
                return token.GetClaimByMapping(null, claimMapping.Key);
            }

        end:
            return token.Claims.FirstOrDefault(o => o.Type == claim);
        }
    }
}
