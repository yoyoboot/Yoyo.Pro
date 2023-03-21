// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Yoyo.Pro.Security
{
    /// <summary>
    /// 响应头的增删集合
    /// </summary>
    public class SecurityHeadersPolicy
    {
        public IDictionary<string, string> SetHeaders { get; }
            = new Dictionary<string, string>();

        public ISet<string> RemoveHeaders { get; }
            = new HashSet<string>();
    }
}
