// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    /// <summary>
    /// 连接应用API
    /// </summary>
    public interface IApplyAPIAppService : IApplicationService
    {
        /// <summary>
        /// 获取API网关应用 下的API列表
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ApplyAPIListDto>> GetApplyAPIListByApplyID(GetApplyAPIPagesInput input);

        /// <summary>
        /// 通过指定id获取 获取API网关应用 下的API接口数据
        /// </summary>
        Task<ApplyAPIListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 添加或者修改 API接口数据 的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateApplyAPI input);

        /// <summary>
        /// 删除 API接口数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除 API接口数据
        /// </summary>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 新增修改连接应用下API配置数据（api信息 query header requestbody responsebody）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAPISetting(CreateOrUpdateApplyAPI input);

        /// <summary>
        /// 导入swagger文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="applyID"></param>
        /// <returns></returns>
        Task<List<ImprotSwaggerData>> ImprotSwagger(string url, Guid applyID);

        /// <summary>
        /// 确认导入
        /// </summary>
        /// <param name="swagger"></param>
        /// <param name="applyID"></param>
        /// <returns></returns>
        Task<bool> SureImprotSwagger(List<ImprotSwaggerData> swagger, Guid applyID);
    }
}
