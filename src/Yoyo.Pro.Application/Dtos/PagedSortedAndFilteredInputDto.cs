

namespace Yoyo.Pro.Dtos
{
    /// <summary>
    /// 支持分页、排序和FitlerText的Dto
    /// </summary>
    public class PagedSortedAndFilteredInputDto : PagedAndSortedInputDto
    {
        /// <summary>
        /// 筛选文本
        /// </summary>
        public virtual string FilterText { get; set; }
    }
}
