// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yoyo.LowCode.Databases.Dtos
{
    public class CreateOrUpdateTableInput
    {
        /// <summary>
        ///     连接ID
        /// </summary>
        public Guid? DbLinkId { get; set; }

        /// <summary>
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public TableInfo TableInfo { get; set; }

        /// <summary>
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public List<TableFieldListItem> TableFieldList { get; set; }
    }

    /// <summary>
    ///     表信息
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        ///     旧表名称
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        ///     新表名称
        /// </summary>
        [Required(ErrorMessage = "表名不能为空")]
        public string NewTable { get; set; }

        /// <summary>
        ///     表说明
        /// </summary>

        [Required(ErrorMessage = "表说明不能为空")]
        public string TableName { get; set; }
    }

    /// <summary>
    ///     字段信息
    /// </summary>
    public class TableFieldListItem
    {
        /// <summary>
        ///     是否允许为空
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public int AllowNull { get; set; }

        /// <summary>
        ///     长度
        /// </summary>
        public string DataLength { get; set; }

        /// <summary>
        ///     类型
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public string DataType { get; set; }

        /// <summary>
        ///     列名
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public string Field { get; set; }

        /// <summary>
        ///     字段注释
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public string FieldName { get; set; }

        /// <summary>
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public int Index { get; set; }

        [Required(ErrorMessage = "必填")] public int PrimaryKey { get; set; }
    }
}
