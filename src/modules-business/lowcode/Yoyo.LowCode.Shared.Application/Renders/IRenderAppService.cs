// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.Dtos;
using Yoyo.LowCode.Renders.Dtos;

namespace Yoyo.LowCode.Renders
{
    public interface IRenderAppService : IApplicationService
    {
        /// <summary>
        ///     获取动态菜单的分页列表集合
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        Task<PagedResultDto<object>> GetPaged(GetRenderPageInput pageInput);

        /// <summary>
        ///     根据ID获取数据
        /// </summary>
        Task<string> GetById(GetRenderInput input);

        /// <summary>
        ///     添加或修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> CreateOrUpdate(RenderCreateOrUpdateInput input);

        /// <summary>
        ///     删除
        /// </summary>
        /// <returns></returns>
        Task Delete(GetRenderInput input);

        /// <summary>
        ///     批量删除
        /// </summary>
        Task BatchDelete(BatchDeleteDto input);

        /// <summary>
        ///     上传文件
        /// </summary>
        Task<LowCodeSysFileListDto> UploadFile();

        Task<LowCodeSysFileListDto> GetFileById(Guid id);
    }
}
