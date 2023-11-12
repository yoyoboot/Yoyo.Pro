// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.LowCodeAPI
{
    /// <summary>
    /// 系统业务下拉分类 通用管理
    /// </summary>
    public class CommonTypeTable : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 虚拟表名称
        /// </summary>
        [MaxLength(200)]
        public string TableName { get; set; }

        /// <summary>
        /// 应用分类名称
        /// </summary>
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// 应用分类编码
        /// </summary>
        [MaxLength(200)]
        public string Code { get; set; }
    }
}
