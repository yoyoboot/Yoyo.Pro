using Abp.Runtime.Validation;
using Yoyo.Pro.Dtos;

namespace Yoyo.LowCode.DynamicMenus.Dtos
{
    /// <summary>
    /// 获取动态菜单的传入参数Dto
    /// </summary>
    public class GetDynamicMenusInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

        //// custom codes

        //// custom codes end
    }
}
