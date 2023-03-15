using Abp.Application.Services.Dto;

namespace Yoyo.Pro.DtoS
{
    public interface ICurrentPagedResultRequest : ILimitedResultRequest
    {

        /// <summary>
        /// 当前页
        /// </summary>
        int CurrentPage { get; set; }

    }
}
