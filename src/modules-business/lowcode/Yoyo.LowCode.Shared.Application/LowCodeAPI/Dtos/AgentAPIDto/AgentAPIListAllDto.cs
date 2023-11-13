// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Yoyo.LowCode.LowCodeAPI.Dtos.AppAuthDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.FlowControlDto;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.AgentAPIDto
{
    public class AgentAPIListAllDto
    {
        public Guid id { get; set; }

        public string Name { get; set; }

        public bool IsCheck { get; set; }

        public AppAuthMappingListDto AppAuthMapping { get; set; }

        public FlowControlMappingListDto FlowMapping { get; set; }
    }
}
