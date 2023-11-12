// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Yoyo.LowCode.AutomaticTables.Dtos.Enums;

namespace Yoyo.LowCode.AutomaticTables.Dtos
{
    public class CreateOrUpdateTableData
    {
        public class TableFieldsItem
        {
            /// <summary>
            /// 字段名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 实体名--表名
            /// </summary>
            public string Entity { get; set; }

            /// <summary>
            /// 字段值
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// 字段类型
            /// </summary>
            public ComponentType ComponentType { get; set; }

            /// <summary>
            /// 是否是自定义字段
            /// </summary>
            public bool IsCustomFields { get; set; }
        }

        public class TableGridsItem
        {
            /// <summary>
            /// 实体名--表名
            /// </summary>
            public string Entity { get; set; }

            /// <summary>
            /// 列数据
            /// </summary>
            public List<TableFieldsItem> Cloumns { get; set; }
        }

        public class CreateOrUpdateTableDto
        {
            /// <summary>
            /// 主表名称
            /// </summary>
            public string MainTable { get; set; }

            /// <summary>
            /// 字段
            /// </summary>
            public List<TableFieldsItem> Fields { get; set; }

            /// <summary>
            /// 子列表
            /// </summary>
            public List<TableGridsItem> Grids { get; set; }
        }
    }
}
