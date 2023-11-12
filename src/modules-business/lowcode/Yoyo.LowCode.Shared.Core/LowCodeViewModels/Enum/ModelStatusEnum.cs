// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Yoyo.Pro.EnumHelper;

namespace Yoyo.LowCode.LowCodeViewModels.Enum
{
    public enum ModelStatusEnum
    {
        [Description("正常")]
        [EnumName("正常")]
        Regular = 0,

        [Description("待激活")]
        [EnumName("待激活")]
        TobeActivated = 1,

        [Description("废弃")]
        [EnumName("废弃")]
        Scrap = 2,
    }
}
