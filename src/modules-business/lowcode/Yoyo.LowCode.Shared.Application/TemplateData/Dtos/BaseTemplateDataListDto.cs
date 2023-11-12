// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.CustomPages;
using Yoyo.LowCode.Dtos;

namespace Yoyo.LowCode.TemplateData.Dtos
{
    public class BaseTemplateDataListDto : LowCodeFullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        [MaxLength(BaseCustomPageConsts.TitleMaxCount)]
        public string TemplateName { get; set; }

        /// <summary>
        /// 表格模板json数据
        /// </summary>
        public string TableTemplateJson { get; set; }

        /// <summary>
        /// 行json数据
        /// </summary>
        public string ColumnTemplateJson { get; set; }

        /// <summary>
        /// 表单模板json数据
        /// </summary>
        public string FormTemplateJson { get; set; }

        /// <summary>
        /// 模板注释
        /// </summary>
        [MaxLength(LowCodeConsts.RemarkMaxCount)]
        public string Description { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImagesUrl { get; set; }
    }
}
