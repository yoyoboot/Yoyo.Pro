// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Runtime.Validation;
using Yoyo.Pro.Dtos;

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition.Dtos
{
    /// <summary>
    /// 分页UserQuery
    /// </summary>
    public class GetBaseUserQueryInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                // Sorting =  MedProConsts.DefaultSortByCreationTimeDesc;
                Sorting = "Id";
            }
        }

        //// custom codes

        //// custom codes end
    }
}
