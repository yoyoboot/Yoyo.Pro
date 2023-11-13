// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiParameterDto;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiDto
{
    public class ImprotSwaggerData : ApplyAPIListDto
    {
        public List<ApplyQHDataListDto> qhDataListDtos { get; set; }

        public List<ApplyBodyDataListDto> requestBodyList { get; set; }

        public List<ApplyBodyDataListDto> responseBodyList { get; set; }

        /// <summary>
        /// 是否已存在
        /// </summary>
        public bool IsRepeat { get; set; }

        /// <summary>
        /// 一共有多少重复
        /// </summary>
        public int CuntsRepeat { get; set; }
    }
}
