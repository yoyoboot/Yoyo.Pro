using System;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.LowCodeViewModels.Enum;

namespace Yoyo.LowCode.CustomPages.Dtos
{
    /// <summary>
    /// 功能设计的列表DTO
    /// <see cref="BaseCustomPage"/>
    /// </summary>
    public class CustomPageEditDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

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

        /// <summary>
        /// 视图模型类型
        /// </summary>
        public ViewTypeEnum ViewType { get; set; }

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

        //// custom codes

        //// custom codes end
    }
}
