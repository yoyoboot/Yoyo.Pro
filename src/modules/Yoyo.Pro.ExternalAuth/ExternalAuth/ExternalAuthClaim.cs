namespace Yoyo.Pro.ExternalAuth
{
    public class ExternalAuthClaim
    {
        public string Type { get; }
        public string Value { get; }

        public ExternalAuthClaim(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }

    }
}
