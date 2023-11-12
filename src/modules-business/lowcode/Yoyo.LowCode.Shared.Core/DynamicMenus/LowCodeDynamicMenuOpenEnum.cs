using System.ComponentModel;
using Yoyo.Pro.EnumHelper;

namespace Yoyo.LowCode.DynamicMenus
{
    public enum LowCodeDynamicMenuOpenEnum
    {
        /// <summary>
        ///     功能
        /// </summary>
        [Description("功能")][EnumName("功能")] Function = 1,

        /// <summary>
        ///     组件
        /// </summary>
        [Description("组件")][EnumName("组件")] Component = 2,

        /// <summary>
        ///     iframe
        /// </summary>
        [Description("iframe")]
        [EnumName("iframe")]
        Iframe = 3,

        /// <summary>
        ///     外部连接
        /// </summary>
        [Description("外部链接")]
        [EnumName("外部链接")]
        Outer = 4
    }
}
