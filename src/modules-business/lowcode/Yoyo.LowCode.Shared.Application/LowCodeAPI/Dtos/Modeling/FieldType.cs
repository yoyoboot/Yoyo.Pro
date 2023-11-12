// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.Modeling
{
    //
    // 摘要:
    //     数据类型
    public enum FieldType
    {
        //
        // 摘要:
        //     Guid
        [Description("guid")]
        Guid,

        //
        // 摘要:
        //     Int
        [Description("int")]
        Int,

        //
        // 摘要:
        //     String
        [Description("string")]
        String,

        //
        // 摘要:
        //     Decimal
        [Description("decimal")]
        Decimal,

        //
        // 摘要:
        //     DateTime
        [Description("datetime")]
        DateTime,

        //
        // 摘要:
        //     Picture
        [Description("string")]
        Picture,

        //
        // 摘要:
        //     File
        [Description("string")]
        File
    }
}
