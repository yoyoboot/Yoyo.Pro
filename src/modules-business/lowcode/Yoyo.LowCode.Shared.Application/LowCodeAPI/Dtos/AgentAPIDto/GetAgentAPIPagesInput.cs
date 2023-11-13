// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Abp.Runtime.Validation;
using Yoyo.Pro.Dtos;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.AgentAPIDto
{
    public class GetAgentAPIPagesInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

        public string AgentName { get; set; }

        //AppAuthMappingListDto 应用授权ID
        public Guid AppAuthID { get; set; }

        //FlowControlMapping 流量策略ID
        public Guid FlowID { get; set; }

        /// <summary>
        /// Mapping 的关系类型（1 授权，2流量）
        /// </summary>
        public int CheckType { get; set; }
    }
}
