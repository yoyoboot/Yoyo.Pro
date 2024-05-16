// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Yoyo.Pro.EnumHelper;

namespace Yoyo.LowCode.LowCodeViewModels.Enum
{
    public enum ViewTypeEnum
    {
        [Description("表单")]
        [EnumName("表单")]
        Form = 0,

        [Description("报表")]
        [EnumName("报表")]
        Journaling = 1,
    }
}
