// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Yoyo.LowCode.ComField.Dtos
{
    public class ComFieldSelectorOutput
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        ///     字段注释
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        ///     列名
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        ///     类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        ///     长度
        /// </summary>
        public string DataLength { get; set; }

        /// <summary>
        ///     允许空
        /// </summary>
        public int? AllowNull { get; set; }
    }
}
