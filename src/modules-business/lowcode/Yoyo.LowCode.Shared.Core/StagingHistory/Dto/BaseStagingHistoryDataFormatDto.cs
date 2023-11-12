// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Yoyo.LowCode.StagingHistory.Dto
{
    public class DataFormatDto
    {
        public class BaseStagingFieldsItem
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
            public object Value { get; set; }
        }

        public class BaseStagingRowsItem
        {
            /// <summary>
            /// 列数据
            /// </summary>
            public List<BaseStagingFieldsItem> Cloumns { get; set; }
        }

        public class BaseStagingGridsItem
        {
            /// <summary>
            /// 实体名--表名
            /// </summary>
            public string Entity { get; set; }

            /// <summary>
            /// 行数据
            /// </summary>
            public List<BaseStagingRowsItem> Rows { get; set; }
        }

        public class BaseStagingHistoryDataFormatDto
        {
            /// <summary>
            /// 字段
            /// </summary>
            public List<BaseStagingFieldsItem> Fields { get; set; }

            /// <summary>
            /// 子列表
            /// </summary>
            public List<BaseStagingGridsItem> Grids { get; set; }
        }
    }
}
