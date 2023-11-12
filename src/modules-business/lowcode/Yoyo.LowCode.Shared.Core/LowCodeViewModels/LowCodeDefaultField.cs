// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.LowCodeViewModels
{
    /// <summary>
    /// 系统字段
    /// </summary>
    public class LowCodeDefaultField : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string FieldDesc { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        public int? DataLength { get; set; }

        /// <summary>
        /// 是否允许空
        /// </summary>
        public bool IsAllowNull { get; set; }
    }
}
