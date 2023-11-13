// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Runtime.Validation;

namespace Yoyo.LowCode.Dtos
{
    /// <summary>
    /// 查询的模糊搜索输入
    /// </summary>
    public class LowCodeGetQueryFilterInput : IShouldNormalize
    {
        /// <summary>
        /// 模糊查询
        /// </summary>
        public virtual string FilterText { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual string Sorting { get; set; }

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public virtual void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = LowCodeConfigs.Dto.NdoSortByName;
            }
        }
    }
}
