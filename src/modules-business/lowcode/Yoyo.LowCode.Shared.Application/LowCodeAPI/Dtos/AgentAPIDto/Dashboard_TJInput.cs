// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Yoyo.LowCode.LowCodeAPI.Dtos.AgentAPIDto
{
    public class Dashboard_TJInput
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 日期范围 1  当天  7最近一周 30 最近一个月
        /// </summary>
        public int dateType { get; set; }
    }
}
