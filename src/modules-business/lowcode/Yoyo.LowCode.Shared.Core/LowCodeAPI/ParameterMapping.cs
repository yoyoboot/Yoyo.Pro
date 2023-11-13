// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class ParameterMapping : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 接口中心APIID
        /// </summary>
        public Guid AgentAPIID { get; set; }

        /// <summary>
        /// 参数来源 连接中心接口参数 或者 自定义(ApplyAPI Customize)
        /// </summary>
        [MaxLength(200)]
        public string Source { get; set; }

        /// <summary>
        /// 应用API参数名称 Parameter Name
        /// </summary>
        [MaxLength(200)]
        public string ParameterName { get; set; }

        /// <summary>
        /// 应用API参数类型 Parameter DataType（header query body path,sql_query,sql_page）
        /// </summary>
        [MaxLength(50)]
        public string ParameterDataType { get; set; }

        /// <summary>
        /// 映射接口中心APIID参数    或者  填写固定值(选择   固定值)
        /// </summary>
        [MaxLength(50)]
        public string MappingMode { get; set; }

        /// <summary>
        /// 固定值
        /// </summary>
        [MaxLength(2000)]
        public string FixedValue { get; set; }

        /// <summary>
        /// 映射字段名称
        /// </summary>
        [MaxLength(200)]
        public string MappingName { get; set; }

        /// <summary>
        /// 映射字段类型（header query body path）
        /// </summary>
        [MaxLength(50)]
        public string MappingDataType { get; set; }

        /// <summary>
        /// 映射字段路径
        /// </summary>
        [MaxLength(200)]
        public string MappingPath { get; set; }
    }
}
