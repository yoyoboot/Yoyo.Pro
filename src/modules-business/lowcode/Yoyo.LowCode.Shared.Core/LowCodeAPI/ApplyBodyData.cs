// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class ApplyBodyData : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 应用下API主键 或者 接口中心API主键
        /// </summary>
        public Guid APIID { get; set; }

        /// <summary>
        /// 数据分类 RequestBody    ResponseBody
        /// </summary>
        [MaxLength(50)]
        public string DataType { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid? ParentID { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [MaxLength(200)]
        public string FileName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        [MaxLength(50)]
        public string FileType { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [MaxLength(2000)]
        public string DefaultValue { get; set; }

        /// <summary>
        /// 父子级路径
        /// </summary>
        [MaxLength(2000)]
        public string Path { get; set; }

        /// <summary>
        /// 数据来源（导入）
        /// </summary>
        [MaxLength(50)]
        public string DataSource { get; set; }
    }
}
