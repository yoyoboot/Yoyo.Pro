using System.Collections.Generic;

namespace Yoyo.Pro.Modules.DynamicView.Dtos
{
    /// <summary>
    /// 动态页面信息
    /// </summary>
    public class DynamicPageDto
    {
        /// <summary>
        /// 筛选条件
        /// </summary>
        public List<PageFilterItemDto> PageFilters { get; set; }

        /// <summary>
        /// 数据列
        /// </summary>
        public List<ColumnItemDto> Columns { get; set; }
    }
}
