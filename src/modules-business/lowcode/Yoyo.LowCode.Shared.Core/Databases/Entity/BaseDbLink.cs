// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.Entities.Auditing;
using DatabaseType = SqlSugar.DbType;

namespace Yoyo.LowCode.Databases.Entity
{
    /// <summary>
    /// 数据连接
    /// </summary>
    public class BaseDbLink : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 连接名称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 连接驱动
        /// </summary>
        public DatabaseType DbType { get; set; }

        /// <summary>
        /// 主机名称
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 服务名称（ORACLE 用的）
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(LowCodeConsts.RemarkMaxCount)]
        public string Description { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        public long? SortCode { get; set; }
    }
}
