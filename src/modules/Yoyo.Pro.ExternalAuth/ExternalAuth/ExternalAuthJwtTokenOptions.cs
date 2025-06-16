namespace Yoyo.Pro.ExternalAuth
{
    public class ExternalAuthJwtTokenOptions
    {
        public string SecurityKey { get; set; }
        public string Issuer { get; set; } = "external-auth";
        public string Audience { get; set; } = "external-auth";
        public int ExpireInSeconds { get; set; } = 30;
    }
}
