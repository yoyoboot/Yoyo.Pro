// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.LowCodeAPI.DomainService;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyAuthenticationDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class ApplyAuthenticationAppService : LowCodeSharedAppServiceBase, IApplyAuthenticationAppService
    {
        #region Private Fields

        private readonly IApplyAuthenticationManager _authenticationManager;

        #endregion Private Fields

        #region Public Constructors

        public ApplyAuthenticationAppService(IApplyAuthenticationManager applyAuthentication)
        {
            _authenticationManager = applyAuthentication;
        }

        #endregion Public Constructors

        /// <summary>
        /// 批量删除 身份鉴权数据
        /// </summary>
        public async Task BatchDelete(List<Guid> input)
        {
            await _authenticationManager.BatchDelete(input);
        }

        /// <summary>
        /// 添加或者修改 身份鉴权数据 的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Abp.UI.UserFriendlyException"></exception>
        public async Task CreateOrUpdate(CreateOrUpdateApplyAuthentication input)
        {
            var applyTypeList = await _authenticationManager.QueryAsNoTracking.Where(t => t.Id != input.ApplyAuthenticationEditDto.Id && t.ApplyID == input.ApplyAuthenticationEditDto.ApplyID).ToListAsync();

            if (applyTypeList.Count > 0)
            {
                throw new Abp.UI.UserFriendlyException(L($"当前应用下已有身份验证规则，不允许增加多条身份验证规则"));
            }

            var entity = ObjectMapper.Map<ApplyAuthentication>(input.ApplyAuthenticationEditDto);
            ///新增还是修改
            if (input.ApplyAuthenticationEditDto.Id.HasValue)
            {
                await _authenticationManager.UpdateAsync(entity);
            }
            else
            {
                await _authenticationManager.CreateAsync(entity);
            }
        }

        /// <summary>
        /// 删除 身份鉴权数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _authenticationManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 通过指定id获取 获取API网关应用 下的身份鉴权数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApplyAuthenticationListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _authenticationManager
          .QueryAsNoTracking
          .FirstOrDefaultAsync(x => x.Id == input.Id);

            var dto = ObjectMapper.Map<ApplyAuthenticationListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 通过ApplyID获取身份鉴权数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApplyAuthenticationListDto> GetByApplyID(EntityDto<Guid> input)
        {
            var entity = await _authenticationManager
          .QueryAsNoTracking
          .FirstOrDefaultAsync(x => x.ApplyID == input.Id);

            var dto = ObjectMapper.Map<ApplyAuthenticationListDto>(entity);
            return dto;
        }
    }
}
