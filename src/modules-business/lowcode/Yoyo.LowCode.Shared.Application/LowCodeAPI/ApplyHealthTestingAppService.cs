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
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyHealthTestingDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class ApplyHealthTestingAppService : LowCodeSharedAppServiceBase, IApplyHealthTestingAppService
    {
        #region Private Fields

        private readonly IApplyHealthTestingManager _applyHealthTestingManager;

        #endregion Private Fields

        #region Public Constructors

        public ApplyHealthTestingAppService(IApplyHealthTestingManager applyHealthTesting)
        {
            _applyHealthTestingManager = applyHealthTesting;
        }

        #endregion Public Constructors

        /// <summary>
        /// 批量删除 接口健康检测规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            await _applyHealthTestingManager.BatchDelete(input);
        }

        /// <summary>
        /// 添加或者修改 接口健康检测规则 的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Abp.UI.UserFriendlyException"></exception>
        public async Task CreateOrUpdate(CreateOrUpdateApplyHealthTesting input)
        {
            var applyTypeList = await _applyHealthTestingManager.QueryAsNoTracking.Where(t => t.Id != input.ApplyHealthTestingEditDto.Id && t.ApplyID == input.ApplyHealthTestingEditDto.ApplyID).ToListAsync();

            if (applyTypeList.Count > 0)
            {
                throw new Abp.UI.UserFriendlyException(L($"当前应用下已有接口健康检测规则，不允许增加多条健康检测规则"));
            }

            var entity = ObjectMapper.Map<ApplyHealthTesting>(input.ApplyHealthTestingEditDto);
            ///新增还是修改
            if (input.ApplyHealthTestingEditDto.Id.HasValue)
            {
                await _applyHealthTestingManager.UpdateAsync(entity);
            }
            else
            {
                await _applyHealthTestingManager.CreateAsync(entity);
            }
        }

        /// <summary>
        /// 删除 接口健康检测规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _applyHealthTestingManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 通过指定id获取 获取API网关应用 下的接口健康检测规则 数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApplyHealthTestingListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _applyHealthTestingManager
               .QueryAsNoTracking
               .FirstOrDefaultAsync(x => x.Id == input.Id);

            var dto = ObjectMapper.Map<ApplyHealthTestingListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 通过指定ApplyID获取 获取API网关应用 下的接口健康检测规则 数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApplyHealthTestingListDto> GetByApplyID(EntityDto<Guid> input)
        {
            var entity = await _applyHealthTestingManager
               .QueryAsNoTracking
               .FirstOrDefaultAsync(x => x.ApplyID == input.Id);

            var dto = ObjectMapper.Map<ApplyHealthTestingListDto>(entity);
            return dto;
        }
    }
}
