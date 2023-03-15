
using System;

namespace Yoyo.Pro.Dtos
{
    /// <summary>
    /// Dto排序的辅助方法
    /// </summary>
    public static class DtoSortingHelper
    {
        /// <summary>
        /// 可以替换一些字符串为特定名称的排序的方法
        /// </summary>
        /// <param name="sorting"></param>
        /// <param name="replaceFunc"></param>
        /// <returns></returns>
        public static string ReplaceSorting(string sorting, Func<string, string> replaceFunc)
        {
            var sortFields = sorting.Split(',');
            for (var i = 0; i < sortFields.Length; i++)
            {
                sortFields[i] = replaceFunc(sortFields[i].Trim());
            }

            return string.Join(",", sortFields);
        }
    }
}
