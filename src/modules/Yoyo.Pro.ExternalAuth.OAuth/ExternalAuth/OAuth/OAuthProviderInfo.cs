using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication;

using System.Collections.Generic;
using System.IO;

namespace Yoyo.Pro.ExternalAuth.OAuth
{
    public class OAuthProviderInfo : ExternalAuthProviderInfoBase
    {
        /// <summary>
        /// Gets or sets the provider-assigned client id.
        /// </summary>
        public string ClientId
        {
            get => GetValueOrDefault(nameof(ClientId));
            set => SetValue(nameof(ClientId), value);
        }

        /// <summary>
        /// Gets or sets the provider-assigned client secret.
        /// </summary>
        public string ClientSecret
        {
            get => GetValueOrDefault(nameof(ClientSecret));
            set => SetValue(nameof(ClientSecret), value);
        }

        /// <summary>
        /// Gets or sets the URI where the client will be redirected to authenticate.
        /// </summary>
        public string AuthorizationEndpoint
        {
            get => GetValueOrDefault(nameof(AuthorizationEndpoint));
            set => SetValue(nameof(AuthorizationEndpoint), value);
        }

        /// <summary>
        /// Gets or sets the URI the middleware will access to exchange the OAuth token.
        /// </summary>
        public string TokenEndpoint
        {
            get => GetValueOrDefault(nameof(TokenEndpoint));
            set => SetValue(nameof(TokenEndpoint), value);
        }

        /// <summary>
        /// Gets or sets the URI the middleware will access to obtain the user information.
        /// This value is not used in the default implementation, it is for use in custom implementations of
        /// <see cref="OAuthEvents.OnCreatingTicket" />.
        /// </summary>
        public string UserInformationEndpoint
        {
            get => GetValueOrDefault(nameof(UserInformationEndpoint));
            set => SetValue(nameof(UserInformationEndpoint), value);
        }
    }
}
