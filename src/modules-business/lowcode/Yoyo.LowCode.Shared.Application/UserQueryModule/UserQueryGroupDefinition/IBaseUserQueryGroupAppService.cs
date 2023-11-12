// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.Dtos;
using Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition.Dtos;

namespace Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition
{
    public interface IBaseUserQueryGroupAppService : IApplicationService
    {
        /// <summary>
		/// 获取的分页列表
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<BaseUserQueryGroupListDto>> GetNdoPaged(GetBaseUserQueryGroupInput input);

        /// <summary>
        /// 获取服务下拉列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<BaseUserQueryGroupListDto>> GetNdoCombox(LowCodeGetQueryFilterInput input);

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetBaseUserQueryGroupForEditOutput> GetForEdit(NullableIdDto<Guid> input);

        /// <summary>
        /// 添加或者修改的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Guid> CreateOrUpdate(BaseUserQueryGroupCreateOrUpdateInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        //// custom codes

        //// custom codes end
    }
}
