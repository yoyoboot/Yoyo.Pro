// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class FlowControlMapping : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 流量控制策略ID
        /// </summary>
        public Guid FlowID { get; set; }

        /// <summary>
        /// 接口中心API ID
        /// </summary>
        public Guid AgentID { get; set; }

        /// <summary>
        /// 接口中心API Name
        /// </summary>
        public string AgentName { get; set; }
    }
}
