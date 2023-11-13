// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Yoyo.Pro.EnumHelper;

namespace Yoyo.LowCode.Databases.Entity
{
    /// <summary>
    /// 关联类型
    /// </summary>
    public enum TableRelationEnum
    {
        /// <summary>
        /// 主表
        /// </summary>
        [Description("主表")]
        [EnumName("主表")]
        Main,

        /// <summary>
        /// 子表
        /// </summary>
        [Description("子表")]
        [EnumName("子表")]
        Child
    }
}
