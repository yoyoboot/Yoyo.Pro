// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Runtime.Validation;
using Yoyo.Pro.Dtos;

namespace Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition.Dtos
{
    /// <summary>
    /// 分页UserQueryGroup
    /// </summary>
    public class GetBaseUserQueryGroupInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
