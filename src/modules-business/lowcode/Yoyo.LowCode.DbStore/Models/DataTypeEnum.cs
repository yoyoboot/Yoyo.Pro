// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace Yoyo.LowCode.Models
{
    public enum DataTypeEnum
    {
        [Description("Guid")]
        Guid = 1,

        [Description("字符串-Nvarchar")]
        Nvarchar = 2,

        [Description("整形-Int")]
        Int = 3,

        [Description("长整形-BigInt")]
        Bigint = 4,

        [Description("日期-Datetime")]
        Datetime = 5,

        [Description("日期-Datetime2")]
        Datetime2 = 6,

        [Description("布尔值-Bit")]
        Bit = 7,

        [Description("小数-Float")]
        Float = 8,

        [Description("小数-Decimal")]
        Decimal = 9,

        [Description("超长字符串-Nvarchar(Max)")]
        Maxnvarchar = 10,

        [Description("字节-Blob")]
        Blobnvarchar = 11,

        [Description("Raw")]
        Raw = 12
    }
}
