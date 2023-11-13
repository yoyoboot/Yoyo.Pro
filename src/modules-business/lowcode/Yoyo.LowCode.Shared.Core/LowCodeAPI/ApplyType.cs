// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.LowCodeAPI
{
    /// <summary>
    /// 网关应用类型
    /// </summary>
    public class ApplyType : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 应用分类名称(内置http:1001  Sqlserver:1002)
        /// </summary>
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// 应用分类编码(内置http:1001  Sqlserver:1002)
        /// </summary>
        public int Code { get; set; }
    }
}
