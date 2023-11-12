// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Minio.DataModel;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.CommonDto
{
    /// <summary>
    /// 流量策略数据
    /// </summary>
    public class CacheItemDto : Item
    {
        //流量控制模式的 次数值
        public int value { get; set; }

        public List<JudgeFusingDto> fusingModeLog { get; set; }

        public List<TestingApplyHealthDto> testingApplyHealths { get; set; }
    }

    /// <summary>
    /// 熔断策略缓存
    /// </summary>
    public class JudgeFusingDto
    {
        public DateTime HappenTime { get; set; }
        public int value { get; set; }
    }

    /// <summary>
    /// 连接健康检测
    /// </summary>
    public class TestingApplyHealthDto
    {
        /// <summary>
        /// 连接器ID
        /// </summary>
        public Guid ApplyID { get; set; }

        /// <summary>
        /// 是否连接正常
        /// </summary>
        public bool IsConnection { get; set; }

        public string Message { get; set; }

        public int StatusCode { get; set; }
    }
}
