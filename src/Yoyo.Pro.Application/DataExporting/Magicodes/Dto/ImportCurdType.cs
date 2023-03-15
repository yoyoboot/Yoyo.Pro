using System.ComponentModel;

namespace Yoyo.Pro.DataExporting.Magicodes.Dto
{
    /// <summary>
    /// 导入的增删改查类型
    /// </summary>
    public enum ImportCurdType
    {
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete,
        /// <summary>
        /// 新增
        /// </summary>
        [Description("新增")]
        Add,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Update
    }
}
