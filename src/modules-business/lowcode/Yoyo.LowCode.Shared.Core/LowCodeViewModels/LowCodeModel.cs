// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.LowCodeViewModels
{
    /// <summary>
    /// 模型信息表
    /// </summary>
    public class LowCodeModel : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 表描述
        /// </summary>
        public string ModelDesc { get; set; }

        /// <summary>
        /// 是否是系统表
        /// </summary>
        public bool IsSystem { get; set; } = false;
    }
}
