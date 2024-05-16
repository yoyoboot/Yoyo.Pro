// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.Dtos;
using Yoyo.LowCode.UserQueryModule.UserQueryDefinition.Dtos;
using Yoyo.Pro.Dtos;

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition
{
    public interface IBaseUserQueryAppService : IApplicationService
    {
        /// <summary>
		/// 获取的分页列表
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<BaseUserQueryListDto>> GetNdoPaged(GetBaseUserQueryInput input);

        /// <summary>
        /// 获取服务下拉列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<BaseUserQueryListDto>> GetNdoCombox(LowCodeGetQueryFilterInput input);

        Task<List<LowCodeNdoDto>> GetNdo(LowCodeGetQueryFilterInput input);

        /// <summary>
        /// 获取查询参数的类型的下拉列表
        /// </summary>
        /// <returns></returns>
        Task<List<ComboxDto<string>>> GetUserQueryParamterTypeCombox();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetBaseUserQueryForEditOutput> GetForEdit(NullableIdDto<Guid> input);

        /// <summary>
        /// 添加或者修改的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Guid> CreateOrUpdate(BaseUserQueryCreateOrUpdateInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 测试UserQuery工作是否正常
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<BaseUserQueryTestOutput> Test(BaseUserQueryTestInput input);

        //// custom codes

        //// custom codes end
    }
}
