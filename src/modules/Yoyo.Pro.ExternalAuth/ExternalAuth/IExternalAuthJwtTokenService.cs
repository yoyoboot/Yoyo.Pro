using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System;
using System.Security.Claims;
using System.Text;

namespace Yoyo.Pro.ExternalAuth
{
    public interface IExternalAuthJwtTokenService
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        string GenerateToken(ClaimsPrincipal principal);

        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ExternalAuthJwtTokenValidateResult ValidateToken(string token);
    }

    public class ExternalAuthJwtTokenService : IExternalAuthJwtTokenService
    {
        readonly ExternalAuthOptions _options;

        public ExternalAuthJwtTokenService(ExternalAuthOptions options)
        {
            _options = options;
        }

        public string GenerateToken(ClaimsPrincipal principal)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Issuer,
                claims: principal.Claims,
                expires: DateTime.Now.AddSeconds(_options.ExpireInSeconds),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ExternalAuthJwtTokenValidateResult ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_options.SecurityKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _options.Issuer,
                ValidAudience = _options.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return new ExternalAuthJwtTokenValidateResult()
                {
                    Principal = principal,
                    ValidatedToken = validatedToken
                };
            }
            catch
            {
                // Token validation failed
                return null;
            }
        }
    }
}
