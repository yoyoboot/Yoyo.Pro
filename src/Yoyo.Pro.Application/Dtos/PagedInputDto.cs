using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Yoyo.Pro.Consts;

namespace Yoyo.Pro.Dtos
{
    /// <summary>
    /// 支持分页的Dto
    /// </summary>
    public class PagedInputDto : IPagedResultRequest
    {
        /// <summary>
        /// 最大的返回条数
        /// </summary>
        [Range(1, AbpProConsts.MaxPageSize)]
        public virtual int MaxResultCount { get; set; }


        /// <summary>
        /// 跳过的数据量
        /// </summary>
        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }

        public PagedInputDto()
        {
            MaxResultCount = AbpProConsts.DefaultPageSize;
        }
    }
}
