// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.FlowControlDto
{
    public class FlowControlEditDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 乐观锁Token
        /// </summary>
        public string ConcurrencyToken { get; set; }

        /// <summary>
        /// 流量控制策略名称
        /// </summary>
        [MaxLength(200)]
        public string FlowName { get; set; }

        /// <summary>
        /// 单位时间
        /// </summary>
        public int UnitTime { get; set; }

        /// <summary>
        /// 单位（秒 分 时 天）
        /// </summary>
        [MaxLength(50)]
        public string UnitType { get; set; }

        /// <summary>
        /// API流量限制
        /// </summary>
        public int TrafficRestrictions { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
