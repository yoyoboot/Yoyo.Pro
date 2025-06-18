// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp;

namespace Yoyo.Pro.ExternalAuth
{
    public static class ExternalAuthExtensions
    {
        public static string ReplaceUrlTenancyName(this string url, string tenancyName)
        {
            if (url.HasValue() && url.Contains("*") && tenancyName.HasValue())
            {
                return url.Replace("*", tenancyName.ToLower());
            }

            return url;
        }
    }
}
