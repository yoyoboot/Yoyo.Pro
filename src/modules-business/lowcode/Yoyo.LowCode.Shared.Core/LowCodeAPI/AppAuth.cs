// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.LowCodeAPI
{
    /// <summary>
    /// API 授权应用
    /// </summary>
    public class AppAuth : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        [MaxLength(200)]
        public string AppName { get; set; }

        /// <summary>
        /// 应用key
        /// </summary>
        [MaxLength(500)]
        public string AppKey { get; set; }

        /// <summary>
        /// 应用描述
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; }
    }
}
