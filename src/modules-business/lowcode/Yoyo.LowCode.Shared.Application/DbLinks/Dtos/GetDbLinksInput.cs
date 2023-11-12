using Abp.Runtime.Validation;
using Yoyo.Pro.Dtos;

namespace Yoyo.LowCode.DbLinks.Dtos
{
    /// <summary>
    /// 获取数据连接的传入参数Dto
    /// </summary>
    public class GetDbLinksInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "SortCode desc";
            }
        }

        //// custom codes

        //// custom codes end
    }
}
