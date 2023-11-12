// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.LowCodeAPI
{
    /// <summary>
    /// API网关应用信息
    /// </summary>
    public class Apply : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }

        /// <summary>
        /// 应用类型名称
        /// </summary>
        [MaxLength(200)]
        public string ApplyTypeName { get; set; }

        /// <summary>
        /// 应用类型Code
        /// </summary>
        public int ApplyTypeCode { get; set; }

        /// <summary>
        /// 连接器Code
        /// </summary>
        [MaxLength(200)]
        public string ApplyCode { get; set; }

        /// <summary>
        /// 连接器名称
        /// </summary>
        [MaxLength(200)]
        public string ApplyName { get; set; }

        /// <summary>
        /// 连接器备注信息
        /// </summary>
        [MaxLength(2000)]
        public string Remarks { get; set; }

        /// <summary>
        /// 数据库类型（sqlserver mysql）
        /// </summary>
        [MaxLength(50)]
        public string SqlType { get; set; }

        /// <summary>
        /// http  url
        /// </summary>
        [MaxLength(200)]
        public string HttpUrlTest { get; set; }

        /// <summary>
        /// http  url
        /// </summary>
        [MaxLength(200)]
        public string HttpUrlProduce { get; set; }

        /// <summary>
        /// sqlserver 服务器host
        /// </summary>
        [MaxLength(200)]
        public string SqlHostTest { get; set; }

        /// <summary>
        ///  sqlserver 服务器host
        /// </summary>
        public string SqlHostProduce { get; set; }

        /// <summary>
        /// sqlserver 登录名称
        /// </summary>
        [MaxLength(200)]
        public string SqlUserNameTest { get; set; }

        /// <summary>
        /// sqlserver 登录名称
        /// </summary>
        public string SqlUserNameProduce { get; set; }

        /// <summary>
        /// sqlserver 登录密码
        /// </summary>
        [MaxLength(200)]
        public string SqlPassWordTest { get; set; }

        /// <summary>
        /// sqlserver 登录密码
        /// </summary>
        public string SqlPassWordProduce { get; set; }

        /// <summary>
        /// sqlsever 数据库
        /// </summary>
        [MaxLength(200)]
        public string SqlDBTest { get; set; }

        /// <summary>
        /// sqlsever 数据库
        /// </summary>
        public string SqlDBProduce { get; set; }

        /// <summary>
        /// 数据库 端口
        /// </summary>
        public int SqlPortTest { get; set; }

        /// <summary>
        /// 数据库 端口
        /// </summary>
        public int SqlPortProduce { get; set; }

        /// <summary>
        /// web service 测试地址
        /// </summary>
        public string WebServiceUrlTest { get; set; }

        /// <summary>
        /// web service正式地址
        /// </summary>
        public string WebServiceUrlProduce { get; set; }
    }
}
