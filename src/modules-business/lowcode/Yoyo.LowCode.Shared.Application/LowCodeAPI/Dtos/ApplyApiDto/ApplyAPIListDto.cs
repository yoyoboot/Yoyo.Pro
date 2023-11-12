// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiDto
{
    public class ApplyAPIListDto : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 网关连接器应用ID
        /// </summary>
        public Guid ApplyID { get; set; }

        #region API基础信息

        /// <summary>
        /// API接口名称
        /// </summary>
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// API请求方式 Name
        /// </summary>
        [MaxLength(50)]
        public string MethodName { get; set; }

        /// <summary>
        /// API请求方式 Code
        /// </summary>
        [MaxLength(50)]
        public string MethodCode { get; set; }

        /// <summary>
        /// API请求路径
        /// </summary>
        [MaxLength(200)]
        public string Path { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        [MaxLength(200)]
        public string LableName { get; set; }

        /// <summary>
        /// 标签Code
        /// </summary>
        [MaxLength(200)]
        public string LableCode { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [MaxLength(2000)]
        public string Remarks { get; set; }

        #endregion API基础信息

        #region 请求数据配置

        /// <summary>
        /// 请求参数QueryJSON
        /// </summary>
        [MaxLength(2000)]
        public string QueryJson { get; set; }

        /// <summary>
        /// 请求参数HeadersJSON
        /// </summary>
        [MaxLength(2000)]
        public string HeadersJson { get; set; }

        /// <summary>
        /// 请求参数RequestBody类型（json xml）
        /// </summary>
        [MaxLength(200)]
        public string RequestBodyDataTypeName { get; set; }

        /// <summary>
        /// 请求参数RequestBody类型（json xml）
        /// </summary>
        [MaxLength(200)]
        public string RequestBodyDataTypeCode { get; set; }

        /// <summary>
        /// 请求参数RequestBodyJson
        /// </summary>
        public string RequestBodyJson { get; set; }

        #endregion 请求数据配置

        #region 返回数据配置

        /// <summary>
        /// 返回参数ResponseBody类型（json xml）
        /// </summary>
        [MaxLength(200)]
        public string ResponseBodyDataTypeName { get; set; }

        /// <summary>
        /// 返回参数ResponseBody类型（json xml）
        /// </summary>
        [MaxLength(200)]
        public string ResponseBodyDataTypeCode { get; set; }

        /// <summary>
        /// 返回数据 ResponseBodyJson
        /// </summary>
        public string ResponseBodyJson { get; set; }

        #endregion 返回数据配置

        /// <summary>
        /// 数据来源（导入）
        /// </summary>
        [MaxLength(50)]
        public string DataSource { get; set; }
    }
}
