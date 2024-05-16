using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Yoyo.LowCode.DynamicMenus.Dtos
{
    /// <summary>
    /// 动态菜单的列表DTO
    /// <see cref="BaseDynamicMenu"/>
    /// </summary>
    public class LowCodeDynamicMenuListDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 系统菜单
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "编码不能为空")]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 组件名称
        /// </summary>
        [MaxLength(BaseDynamicMenuConsts.TextMaxCount)]
        public string Name { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string PermissionCode { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 连接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 模板路径
        /// </summary>
        public string TemplatePath { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public LowCodeDynamicMenuOpenEnum OpenEnum { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool Hide { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        //// custom codes

        //// custom codes end
    }
}
