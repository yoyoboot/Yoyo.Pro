// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.LowCode.CustomPages.TemplateEntities
{
    [Comment("全部组件")]
    public class BaseTemplateAllModule : Entity<Guid>
    {
        [Comment("单行文本")]
        public string SingleLineText { get; set; }

        [MaxLength(LowCodeConsts.RemarkMaxCount)]
        [Comment("多行文本")]
        public string MultiLineText { get; set; }

        [Comment("数值输入")]
        public int? NumericInput { get; set; }

        [Comment("开关")]
        public string Switch { get; set; }

        [Comment("单选框")]
        public int? RadioBox { get; set; }

        [Comment("多选框")]
        public string MultiCheckBox { get; set; }

        [Comment("唯一多选框")]
        public int? UniqueCheckBox { get; set; }

        [Comment("下拉选择")]
        public string DropDownSelection { get; set; }

        [Comment("级联选择")]
        public string CascadeSelection { get; set; }

        [Comment("时间选择")]
        public string TimeSelection { get; set; }

        [Comment("开始时间")]
        public string StartTime { get; set; }

        [Comment("结束时间")]
        public string EndTime { get; set; }

        [Comment("日期选择")]
        public DateTime? DateSelection { get; set; }

        [Comment("开始日期")]
        public DateTime? StartDate { get; set; }

        [Comment("结束日期")]
        public DateTime? EndDate { get; set; }

        [Comment("树选择")]
        public string TreeSelection { get; set; }

        [Comment("颜色选择")]
        public string ColorSelection { get; set; }

        [Comment("评分")]
        public int? Score { get; set; }

        [Comment("滑块")]
        public int? Slider { get; set; }

        [Comment("文件上传")]
        public string FileUpload { get; set; }

        [Comment("图片上传")]
        public string ImageUpload { get; set; }

        [Comment("富文本")]
        public string RichText { get; set; }

        [Comment("条形码")]
        public string BarCode { get; set; }

        [Comment("二维码")]
        public string QRCode { get; set; }

        [Comment("手写板")]
        public string HandwritingPad { get; set; }

        [Comment("NDO选择")]
        public string NDOSelection { get; set; }

        [Comment("RDO选择头")]
        public string RDOSelectionHead { get; set; }

        [Comment("RDO选择尾")]
        public string RDOSelectionTail { get; set; }

        [Comment("角色选择")]
        public string RoleSelection { get; set; }

        [Comment("用户选择")]
        public string UserSelection { get; set; }

        [Comment("外键")]
        public string ForeignKeyId { get; set; }

        #region 数据联动

        [Comment("设备组")]
        public string DeviceGroups { get; set; }

        [Comment("设备")]
        public string Dquipments { get; set; }

        #endregion 数据联动
    }
}
