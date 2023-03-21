// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yoyo.Pro.Entities
{
    /// <summary>
    /// 并发实体接口
    /// </summary>
    public interface IConcurrency
    {
        /// <summary>
        /// 并发令牌，最长32位
        /// </summary>
        [MaxLength(32)]
        string ConcurrencyToken { get; set; }
    }
}
