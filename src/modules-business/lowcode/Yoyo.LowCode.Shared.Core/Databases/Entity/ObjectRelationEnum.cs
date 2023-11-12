// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Yoyo.Pro.EnumHelper;

namespace Yoyo.LowCode.Databases.Entity
{
    /// <summary>
    /// 关系类型
    /// </summary>
    public enum ObjectRelationEnum
    {
        [Description("一对一")]
        [EnumName("一对一")]
        OneOnOne,

        [Description("一对多")]
        [EnumName("一对多")]
        OneOnMany,
    }
}
