using Microsoft.Extensions.Options;

namespace Yoyo.Pro.ExternalAuth
{
    public class ExternalAuthPostConfigureOptions<T> : IPostConfigureOptions<T>
        where T : class
    {
        public void PostConfigure(string name, T options)
        {

        }
    }
}
