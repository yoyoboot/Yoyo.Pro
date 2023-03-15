// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoyo.Pro.Configuration
{
    public static class BasicAppSettings
    {
        public static class ExternalLoginProvider
        {
            public const string OpenIdConnectMappedClaims = "ExternalLoginProvider.OpenIdConnect.MappedClaims";
            public const string WsFederationMappedClaims = "ExternalLoginProvider.WsFederation.MappedClaims";

            public static class Host
            {
                public const string Facebook = "ExternalLoginProvider.Facebook";
                public const string Google = "ExternalLoginProvider.Google";
                public const string Twitter = "ExternalLoginProvider.Twitter";
                public const string Microsoft = "ExternalLoginProvider.Microsoft";
                public const string OpenIdConnect = "ExternalLoginProvider.OpenIdConnect";
                public const string WsFederation = "ExternalLoginProvider.WsFederation";
            }

            public static class Tenant
            {
                public const string Facebook = "ExternalLoginProvider.Facebook.Tenant";
                public const string Facebook_IsDeactivated = "ExternalLoginProvider.Facebook.IsDeactivated";
                public const string Google = "ExternalLoginProvider.Google.Tenant";
                public const string Google_IsDeactivated = "ExternalLoginProvider.Google.IsDeactivated";
                public const string Twitter = "ExternalLoginProvider.Twitter.Tenant";
                public const string Twitter_IsDeactivated = "ExternalLoginProvider.Twitter.IsDeactivated";
                public const string Microsoft = "ExternalLoginProvider.Microsoft.Tenant";
                public const string Microsoft_IsDeactivated = "ExternalLoginProvider.Microsoft.IsDeactivated";
                public const string OpenIdConnect = "ExternalLoginProvider.OpenIdConnect.Tenant";
                public const string OpenIdConnect_IsDeactivated = "ExternalLoginProvider.OpenIdConnect.IsDeactivated";
                public const string WsFederation = "ExternalLoginProvider.WsFederation.Tenant";
                public const string WsFederation_IsDeactivated = "ExternalLoginProvider.WsFederation.IsDeactivated";
            }
        }
    }
}
