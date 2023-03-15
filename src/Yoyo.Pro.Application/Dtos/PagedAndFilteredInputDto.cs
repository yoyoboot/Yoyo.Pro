using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Yoyo.Pro.Consts;

namespace Yoyo.Pro.Dtos
{
    /// <summary>
    /// 支持分页和FitlerText字段的Dto
    /// </summary>
    public class PagedAndFilteredInputDto : IPagedResultRequest
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

        /// <summary>
        /// 过滤文本信息
        /// </summary>
        public virtual string FilterText { get; set; }


        public PagedAndFilteredInputDto()
        {
            MaxResultCount = AbpProConsts.DefaultPageSize;
        }
    }
}
