// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;
using Yoyo.Pro.Dtos;

namespace Yoyo.LowCode.Renders.Dtos
{
    public class GetRenderPageInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        [Required]
        public Guid CustomPageId { get; set; }

        /// <summary>
        /// 是否需要分页
        /// </summary>
        public bool IsPaged { get; set; }

        public string MaintableName { get; set; }

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = LowCodeConsts.Sort;
            }
        }

        //// custom codes

        //// custom codes end
    }
}
