using System.Collections.Generic;

namespace Yoyo.Pro.ExternalAuth
{
    public class ExternalAuthUserInfo
    {
        public string Provider { get; set; }

        public string ProviderKey { get; set; }

        public string EmailAddress { get; set; }

        public string MobilePhone { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public List<ExternalAuthClaim> Claims { get; internal set; } = new List<ExternalAuthClaim>();
    }
}
