// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.CustomPages;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.TemplateData.Entity
{
    /// <summary>
    /// 模板管理
    /// </summary>
    [Comment("模板管理")]
    public class BaseTemplateData : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        [MaxLength(BaseCustomPageConsts.TitleMaxCount)]
        [Comment("模板名称")]
        public string TemplateName { get; set; }

        /// <summary>
        /// 表格模板json数据
        /// </summary>
        [Comment("表格Json")]
        public string TableTemplateJson { get; set; }

        /// <summary>
        /// 行json数据
        /// </summary>
        [Comment("行Json")]
        public string ColumnTemplateJson { get; set; }

        /// <summary>
        /// 表单模板json数据
        /// </summary>
        [Comment("表单模板json数据")]
        public string FormTemplateJson { get; set; }

        /// <summary>
        /// 模板注释
        /// </summary>
        [MaxLength(LowCodeConsts.RemarkMaxCount)]
        [Comment("模板注释")]
        public string Description { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        [Comment("图片地址")]
        public string ImagesUrl { get; set; }
    }
}
