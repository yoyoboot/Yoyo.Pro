// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.Entities.Auditing;
using Yoyo.LowCode.LowCodeViewModels.Enum;
using Yoyo.Pro.EnumHelper;

namespace Yoyo.LowCode.CustomPages
{
    /// <summary>
    /// 页面配置
    /// </summary>
    public class BaseCustomPage : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 视图模型名称
        /// </summary>
        [MaxLength(BaseCustomPageConsts.TitleMaxCount)]
        public string ViewName { get; set; }

        /// <summary>
        /// 视图模型描述
        /// </summary>
        [MaxLength(LowCodeConsts.RemarkMaxCount)]
        public string ViewDesc { get; set; }

        public string ViewTypeString { get; set; }

        private ViewTypeEnum _viewType;

        /// <summary>
        /// 视图模型类型
        /// </summary>
        [Required]
        [Comment("模型类型")]
        public ViewTypeEnum ViewType
        {
            get
            {
                return _viewType;
            }
            set
            {
                _viewType = value;
                ViewTypeString = _viewType.ToNameValue();
            }
        }

        /// <summary>
        /// 表格配置
        /// </summary>
        public string TableConfigJson { get; set; }

        /// <summary>
        /// 列配置
        /// </summary>
        public string ColumnConfigJson { get; set; }

        /// <summary>
        /// 表单Json
        /// </summary>
        public string FormConfigJson { get; set; }

        /// <summary>
        /// 手机端json
        /// </summary>
        public string PhoneConfigJson { get; set; }

        /// <summary>
        /// 页面状态
        /// </summary>
        public PageStatusEnum PageStatus { get; set; }
    }
}
