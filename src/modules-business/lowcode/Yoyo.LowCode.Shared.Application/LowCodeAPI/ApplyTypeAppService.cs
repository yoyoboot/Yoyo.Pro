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
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyTypeDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class ApplyTypeAppService : LowCodeSharedAppServiceBase, IApplyTypeAppService
    {
        #region Private Fields

        private readonly IApplyTypeManager _applyTypeManager;

        #endregion Private Fields

        #region Public Constructors

        public ApplyTypeAppService(IApplyTypeManager applyTypeManager)
        {
            _applyTypeManager = applyTypeManager;
        }

        #endregion Public Constructors

        /// <summary>
        /// 批量删除 应用分类
        /// </summary>
        /// <param name="input">Id的集合</param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            await _applyTypeManager.BatchDelete(input);
        }

        /// <summary>
        /// 添加或者修改 应用分类 的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdate(CreateOrUpdateApplyType input)
        {
            var applyTypeList = await _applyTypeManager.QueryAsNoTracking.Where(t => t.Id != input.ApplyTypeEditDto.Id).ToListAsync();

            if (applyTypeList.Any(t => t.Code == input.ApplyTypeEditDto.Code))
            {
                throw new Abp.UI.UserFriendlyException(L($"应用类别代码{input.ApplyTypeEditDto.Code}已被使用，请重新输入代码"));
            }
            if (applyTypeList.Any(t => t.Name == input.ApplyTypeEditDto.Name))
            {
                throw new Abp.UI.UserFriendlyException(L($"应用类别名称{input.ApplyTypeEditDto.Code}已被使用，请重新输入名称"));
            }
            var entity = ObjectMapper.Map<ApplyType>(input.ApplyTypeEditDto);
            ///新增还是修改
            if (input.ApplyTypeEditDto.Id.HasValue)
            {
                await _applyTypeManager.UpdateAsync(entity);
            }
            else
            {
                await _applyTypeManager.CreateAsync(entity);
            }
        }

        /// <summary>
        /// 删除 应用分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _applyTypeManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 应用分类类型 下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApplyTypeListDto>> SelectApplyType()
        {
            var entity = await _applyTypeManager.QueryAsNoTracking.ToListAsync();

            var applyTypeLis = ObjectMapper.Map<List<ApplyTypeListDto>>(entity);

            return applyTypeLis;
        }
    }
}
