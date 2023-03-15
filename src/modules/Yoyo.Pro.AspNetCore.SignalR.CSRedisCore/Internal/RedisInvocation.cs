// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Yoyo.Pro.Internal
{
    internal readonly struct RedisInvocation
    {
        /// <summary>
        /// Gets a list of connections that should be excluded from this invocation.
        /// May be null to indicate that no connections are to be excluded.
        /// </summary>
        public IReadOnlyList<string> ExcludedConnectionIds { get; }

        /// <summary>
        /// Gets the message serialization cache containing serialized payloads for the message.
        /// </summary>
        public SerializedHubMessage Message { get; }

        public RedisInvocation(SerializedHubMessage message, IReadOnlyList<string> excludedConnectionIds)
        {
            Message = message;
            ExcludedConnectionIds = excludedConnectionIds;
        }

        public static RedisInvocation Create(string target, object[] arguments, IReadOnlyList<string> excludedConnectionIds = null)
        {
            return new RedisInvocation(
                new SerializedHubMessage(new InvocationMessage(target, null, arguments)),
                excludedConnectionIds);
        }
    }
}
