using Microsoft.AspNetCore.Authentication;

using System.Security.Claims;

namespace Yoyo.Pro.ExternalAuth
{
    public class ExternalAuthResult
    {
        public bool Success { get; set; }



        public string ErrorMessage { get; set; }


        public ClaimsPrincipal Principal { get; set; }
        public AuthenticationProperties Properties { get; set; }
        public string AccessToken { get; set; }


        public static ExternalAuthResult Failure()
        {
            return new ExternalAuthResult()
            {
                Success = false
            };
        }

        public static ExternalAuthResult Successful()
        {
            return new ExternalAuthResult()
            {
                Success = true
            };
        }
    }
}
