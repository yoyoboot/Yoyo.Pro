using Abp.Application.Services.Dto;
using Yoyo.Pro.Consts;

namespace Yoyo.Pro.Dtos
{
    /// <summary>
    /// 支持分页和排序的Dto
    /// </summary>
    public class PagedAndSortedInputDto : PagedInputDto, ISortedResultRequest
    {
        /// <summary>
        /// 排序
        /// </summary>
        public virtual string Sorting { get; set; }

        public PagedAndSortedInputDto()
        {
            MaxResultCount = AbpProConsts.DefaultPageSize;
        }
    }
}
