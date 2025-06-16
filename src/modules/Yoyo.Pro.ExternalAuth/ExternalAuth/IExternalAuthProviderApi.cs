using Abp.Runtime.Session;

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Yoyo.Pro.ExternalAuth
{
    public interface IExternalAuthProviderApi
    {
        Task<ExternalAuthUserInfo> GetUserInfo(string providerName, string token);
    }


    public class ExternalAuthProviderApi : IExternalAuthProviderApi
    {
        readonly IExternalAuthJwtTokenService _jwtTokenService;
        readonly IExternalAuthProviderInfoStore _externalAuthOptionsStore;

        public ExternalAuthProviderApi(IExternalAuthJwtTokenService jwtTokenService, IExternalAuthProviderInfoStore externalAuthOptionsStore)
        {
            _jwtTokenService = jwtTokenService;
            _externalAuthOptionsStore = externalAuthOptionsStore;
        }


        public async Task<ExternalAuthUserInfo> GetUserInfo(string providerName, string token)
        {

            // 验证token
            var res = _jwtTokenService.ValidateToken(token);
            if (res == null)
            {
                throw new ApplicationException("token couldn't verified.");
            }


            var options = await _externalAuthOptionsStore.GetProviderInfo(providerName);



            var validatedToken = (JwtSecurityToken)res.ValidatedToken;



            try
            {
                var subject = validatedToken.GetClaimByMapping(options.ClaimsMapping, ClaimTypes.NameIdentifier)?.Value;
                var fullName = validatedToken.GetClaimByMapping(options.ClaimsMapping, ClaimTypes.Name)?.Value;
                var email = validatedToken.GetClaimByMapping(options.ClaimsMapping, ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(subject))
                {
                    throw new Exception($"No claim was matched! {ClaimTypes.NameIdentifier}");
                }
                if (string.IsNullOrEmpty(fullName))
                {
                    throw new Exception($"No claim was matched! {ClaimTypes.Name}");
                }
                //if (string.IsNullOrEmpty(email))
                //{
                //    throw new Exception($"No claim was matched! {ClaimTypes.Email}");
                //}

                var fullNameParts = fullName.Split(' ');
                return new ExternalAuthUserInfo
                {
                    Provider = providerName,
                    ProviderKey = subject,
                    EmailAddress = email,
                    Name = fullNameParts[0],
                    Surname = ((fullNameParts.Length > 1) ? fullNameParts[1] : fullNameParts[0]),
                    Claims = validatedToken.Claims.Select((Claim c) => new ExternalAuthClaim(c.Type, c.Value)).ToList()
                };
            }
            catch (Exception)
            {
                foreach (var item in validatedToken.Claims)
                {
                    await Console.Out.WriteLineAsync($"key:{item.Type} value:{item.Value}");
                }
                throw;
            }
        }
    }
}
