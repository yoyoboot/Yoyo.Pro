// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiParameterDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    /// <summary>
    /// 连接应用&接口中心 query  header参数配置表
    /// </summary>
    public interface IApplyApiParameterAppService : IApplicationService
    {
        /// <summary>
        ///  添加或者修改 API接口参数ApplyBodyData的配置
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task CreateOrUpdateForApplyBodyData(CreateOrUpdateApplyBodyData input);

        /// <summary>
        /// 批量删除 API接口参数ApplyQHData配置
        /// </summary>
        Task BatchDeleteForApplyBodyData(List<Guid> input);

        /// <summary>
        /// 获取应用下API接口参数ApplyBodyData 列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ApplyBodyDataListDto>> GetApplyBodyDataList(GetPublicPagesInput input);

        /// <summary>
        ///  添加或者修改 API接口参数ApplyBodyData的配置
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task CreateOrUpdateForApplyQHData(CreateOrUpdateApplyQHData input);

        /// <summary>
        /// 批量删除 API接口参数ApplyQHData配置
        /// </summary>
        Task BatchDeleteForApplyQHData(List<Guid> input);

        /// <summary>
        /// 获取应用下API接口参数ApplyQHData配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ApplyQHDataListDto>> GetApplyQHDataDataList(GetPublicPagesInput input);

        /// <summary>
        /// 获取API的ApiQuery参数信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<ApplyQHDataListDto>> GetApiQueryData(EntityDto<Guid> input);

        /// <summary>
        /// 获取API的ApiHeader参数信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<ApplyQHDataListDto>> GetApiHeaderData(EntityDto<Guid> input);

        /// <summary>
        /// 获取API的RequestBody参数信息
        /// </summary>
        /// <param name="input">API ID</param>
        /// <param name="isPackage">是否返回组装后的数据</param>
        /// <returns></returns>
        Task<List<ApplyBodyDataListDto>> GetApiRequestBodyData(EntityDto<Guid> input, bool isPackage);

        /// <summary>
        /// 获取API的ResponseBody参数信息
        /// </summary>
        /// <param name="input">API ID</param>
        /// <param name="isPackage">是否返回组装后的数据</param>
        /// <returns></returns>
        Task<List<ApplyBodyDataListDto>> GetApiResponseBodyData(EntityDto<Guid> input, bool isPackage);

        /// <summary>
        ///  添加或者修改 API参数Mapping关系
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task CreateOrUpdateForParameterMapping(CreateOrUpdateParameterMapping input);

        /// <summary>
        /// 批量删除  API参数Mapping关系
        /// </summary>
        Task BatchDeleteForParameterMapping(List<Guid> input);

        /// <summary>
        /// 获取 API参数Mapping关系 列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ParameterMappingListDto>> GetParameterMappingList(GetPublicPagesInput input);
    }
}
