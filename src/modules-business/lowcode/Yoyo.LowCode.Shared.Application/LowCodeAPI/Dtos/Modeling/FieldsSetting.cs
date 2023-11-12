// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.Modeling
{
    //
    // 摘要:
    //     数据api参数
    public class FieldsSetting
    {
        //
        // 摘要:
        //     字段集合
        public List<DataApiField> Fields { get; set; }

        //
        // 摘要:
        //     主键字段
        public string IdField { get; set; }
    }
}
