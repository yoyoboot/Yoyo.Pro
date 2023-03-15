// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using System.Threading;

namespace Abp.Threading
{
    public static class CancellationTokenProviderExtensions
    {
        public static CancellationToken FallbackToProvider(this ICancellationTokenProvider provider, CancellationToken prefferedValue = default)
        {
            return prefferedValue == default || prefferedValue == CancellationToken.None
                ? provider.Token
                : prefferedValue;
        }
    }

}
