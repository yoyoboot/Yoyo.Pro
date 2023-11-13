// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Yoyo.LowCode.LowCodeViewModels.Dtos
{
    public class TableListOutput
    {
        /// <summary>
        ///     表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     表说明
        /// </summary>
        public string TableDesc { get; set; }

        /// <summary>
        ///     表大小
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        ///     表记录数
        /// </summary>
        public double Sum { get; set; }
    }
}
