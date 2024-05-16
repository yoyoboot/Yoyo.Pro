// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yoyo.LowCode.CustomPages.Dtos
{
    public class LowCodePpcDataPointDto
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// 数据采集项名称
        /// </summary>
        [Required]
        public string DataPointName { get; set; }

        /// <summary>
        /// 关联 <see cref="ProcessParameterCard"/>表
        /// </summary>
        public Guid? ProcessParameterCardId { get; set; }

        /// <summary>
        /// 数据显示类型
        /// </summary>
        public LowCodeDataPointDataType DataType { get; set; }

        /// <summary>
        /// 是否在界面上显示上下限
        /// </summary>
        public bool DisplayLimits { get; set; }

        /// <summary>
        /// 是否允许超越上下限
        /// </summary>
        public bool IsLimitOverrideAllowed { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 采集项下限
        /// </summary>
        public double? LowerLimit { get; set; }

        /// <summary>
        /// 采集项上限
        /// </summary>
        public double? UpperLimit { get; set; }

        /// <summary>
        /// 真
        /// </summary>
        public string BooleanTrue { get; set; }

        /// <summary>
        /// 假
        /// </summary>
        public string BooleanFalse { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// 采集项排序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 默认固定值
        /// </summary>
        public string FixedValue { get; set; }
    }

    public enum LowCodeDataPointDataType
    {
        [Description("Boolean(布尔值)")] Boolean = 1,

        [Description("Decimal(十进制)")] Decimal = 2,

        [Description("Fixed(固定值)")] Fixed = 3,

        [Description("Float(浮点)")] Float = 4,

        [Description("Integer（整数)")] Integer = 5,

        [Description("String（字符串）")] String = 6,

        [Description("Timestamp（时间戳）")] Timestamp = 7,
    }
}
