// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.CommonDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public interface IApplyAppService : IApplicationService
    {
        /// <summary>
        /// 获取API网关应用列表
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ApplyListDto>> GetApplyList(GetApplyPagesInput input);

        /// <summary>
        /// 通过指定id获取 获取API网关应用列表
        /// </summary>
        Task<ApplyListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 通过指定code获取 获取API网关应用列表
        /// </summary>
        Task<ApplyListDto> GetByCode(string code);

        /// <summary>
        /// 添加或者修改 API网关应用 的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateApply input);

        /// <summary>
        /// 删除 API网关应用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除 API网关应用
        /// </summary>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 修改连接应用网关配置数据（连接应用地址、身份验证模式、健康检测配置）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateApplySetting(UpdateApplySettingInput input);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Task<List<TestingApplyHealthDto>> GetApplyHearth();
    }
}
