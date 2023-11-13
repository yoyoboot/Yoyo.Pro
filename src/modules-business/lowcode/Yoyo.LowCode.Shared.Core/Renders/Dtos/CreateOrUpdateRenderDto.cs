// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Yoyo.LowCode.Renders.Dtos
{
    public class CreateOrUpDate
    {
        public class FieldsItem
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
        }

        public class RowsItem
        {
            /// <summary>
            /// 列数据
            /// </summary>
            public List<FieldsItem> Cloumns { get; set; }
        }

        public class GridsItem
        {
            /// <summary>
            /// 实体名--表名
            /// </summary>
            public string Entity { get; set; }

            /// <summary>
            /// 行数据
            /// </summary>
            public List<RowsItem> Rows { get; set; }
        }

        public class CreateOrUpdateRenderDto
        {
            public string MainTable { get; set; }

            /// <summary>
            /// 字段
            /// </summary>
            public List<FieldsItem> Fields { get; set; }

            /// <summary>
            /// 子列表
            /// </summary>
            public List<GridsItem> Grids { get; set; }
        }
    }
}
