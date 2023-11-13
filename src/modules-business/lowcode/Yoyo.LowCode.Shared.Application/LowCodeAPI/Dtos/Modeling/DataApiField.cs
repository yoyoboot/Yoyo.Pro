// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.Modeling
{
    //
    // 摘要:
    //     字段名
    [Serializable]
    public class DataApiField
    {
        //
        // 摘要:
        //     字段名
        public string FieldName { get; set; }

        //
        // 摘要:
        //     字段中文名
        public string DisplayName { get; set; }

        //
        // 摘要:
        //     是否必填
        public bool IsRequired { get; set; }

        //
        // 摘要:
        //     字段类型
        public string FieldType { get; set; }

        public List<DataApiField> Children { get; set; }
    }
}
