// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class AgentAPI : LowCodeFullAuditedEntity<Guid>
    {
        #region AgentAPI基础信息

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
        /// 是否启用安全认证
        /// </summary>
        public bool IsSafetySertification { get; set; }

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

        /// <summary>
        /// 发布状态
        /// </summary>
        public bool EnableStatus { get; set; }

        #endregion AgentAPI基础信息

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

        #region 应用接口映射 (https)

        /// <summary>
        /// 应用分类名称（Http SqlServer Mock）
        /// </summary>
        [MaxLength(200)]
        public string ServiceName { get; set; }

        /// <summary>
        /// 应用分类Code（Http SqlServer Mock）
        /// </summary>
        public int ServiceCode { get; set; }

        /// <summary>
        /// 映射应用名称
        /// </summary>
        [MaxLength(200)]
        public string ApplyName { get; set; }

        /// <summary>
        /// 映射应用Code
        /// </summary>
        [MaxLength(200)]
        public string ApplyCode { get; set; }

        /// <summary>
        /// 映射应用对应API
        /// </summary>

        public Guid ApplyAPIID { get; set; }

        /// <summary>
        /// 映射应用对应API名称 （或者  sql连接器数据源名称）
        /// </summary>
        [MaxLength(200)]
        public string ApplyAPIName { get; set; }

        /// <summary>
        /// 映射应用对应API地址（或者  sql连接器数据源类型）
        /// </summary>
        [MaxLength(200)]
        public string ApplyAPIPath { get; set; }

        /// <summary>
        /// 映射应用对应API请求方式 名称（根据选择的API带过post get delete）
        /// </summary>
        [MaxLength(50)]
        public string ApplyAPIMethodName { get; set; }

        /// <summary>
        /// 映射应用对应API请求方式 Code（根据选择的API带过来post get delete）
        /// </summary>
        [MaxLength(50)]
        public string ApplyAPIMethodCode { get; set; }

        /// <summary>
        /// 入参请求模式 Name（入参透传  入参映射）
        /// </summary>
        [MaxLength(50)]
        public string ParameterModeName { get; set; }

        /// <summary>
        /// 入参请求模式 Code（入参透传  入参映射）
        /// </summary>
        [MaxLength(50)]
        public string ParameterModeCode { get; set; }

        /// <summary>
        /// 入参映射 json
        /// </summary>
        public string ParameterMappingJson { get; set; }

        #endregion 应用接口映射 (https)

        #region 返回数据配置

        public string MockData { get; set; }

        #endregion 返回数据配置

        #region 数据库配置

        /// <summary>
        /// 配置的数据库查询语句
        /// </summary>
        [MaxLength(5000)]
        public string SqlQuery { get; set; }

        /// <summary>
        /// 排序字段 { name,sort}json格式
        /// </summary>
        [MaxLength(500)]
        public string SortJson { get; set; }

        /// <summary>
        /// 是否启用分页
        /// </summary>
        public bool IsPaging { get; set; }

        /// <summary>
        /// 起始页面（从0开始，从1开始）
        /// </summary>
        public int StartPage { get; set; }

        #endregion 数据库配置

        #region webservice配置

        /// <summary>
        /// 在创建api时 根据连接器获取到的service对应配置信息
        /// </summary>
        public string WebServiceSetting { get; set; }

        #endregion webservice配置
    }
}
