// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.TemplateData.Dtos;

namespace Yoyo.LowCode.TemplateData
{
    public interface ITemplateDataAppService : IApplicationService
    {
        /// <summary>
        /// 获取功能设计的表单模板集合
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<BaseTemplateDataListDto>> BaseTemplateDataGetList();

        /// <summary>
        /// 通过指定id获取指定设计模板信息
        /// </summary>
        Task<BaseTemplateDataListDto> GetById(EntityDto<Guid> input);
    }
}
