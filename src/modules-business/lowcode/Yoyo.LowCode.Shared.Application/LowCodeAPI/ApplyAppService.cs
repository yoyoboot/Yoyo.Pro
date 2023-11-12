// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.LowCodeAPI.DomainService;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.CommonDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class ApplyAppService : LowCodeSharedAppServiceBase, IApplyAppService
    {
        #region Private Fields

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ICacheManager _cacheManager;

        private readonly IApplyManager _applyManager;
        private readonly IApplyHealthTestingManager _applyHealthTestingManager;
        private readonly IApplyAuthenticationManager _authenticationManager;

        #endregion Private Fields

        #region Public Constructors

        public ApplyAppService(IApplyManager applyTypeManager,
            IApplyHealthTestingManager applyHealthTestingManager,
            IApplyAuthenticationManager authenticationManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager)
        {
            _applyManager = applyTypeManager;
            _applyHealthTestingManager = applyHealthTestingManager;
            _authenticationManager = authenticationManager;
            _unitOfWorkManager = unitOfWorkManager;
            _cacheManager = cacheManager;
        }

        #endregion Public Constructors

        /// <summary>
        /// 根据ID 批量删除网关应用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            await _applyManager.BatchDelete(input);
        }

        /// <summary>
        /// 新增修改网关应用单表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Abp.UI.UserFriendlyException"></exception>
        public async Task CreateOrUpdate(CreateOrUpdateApply input)
        {
            var applyTypeList = await _applyManager.QueryAsNoTracking.Where(t => t.Id != input.ApplyEditDto.Id).ToListAsync();

            if (applyTypeList.Any(t => t.ApplyCode == input.ApplyEditDto.ApplyCode))
            {
                throw new Abp.UI.UserFriendlyException(L($"应用代码{input.ApplyEditDto.ApplyCode}已被使用，请重新输入代码"));
            }
            if (applyTypeList.Any(t => t.ApplyName == input.ApplyEditDto.ApplyName))
            {
                throw new Abp.UI.UserFriendlyException(L($"应用名称{input.ApplyEditDto.ApplyName}已被使用，请重新输入名称"));
            }
            var entity = ObjectMapper.Map<Apply>(input.ApplyEditDto);
            ///新增还是修改
            if (input.ApplyEditDto.Id.HasValue)
            {
                await _applyManager.UpdateAsync(entity);
            }
            else
            {
                await _applyManager.CreateAsync(entity);
            }
        }

        /// <summary>
        /// 根据ID删除网关应用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _applyManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 获取API网关应用列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ApplyListDto>> GetApplyList(GetApplyPagesInput input)
        {
            var query = _applyManager.Query
               .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>
                   a.ApplyName.Contains(input.FilterText) ||
                   a.ApplyCode.Contains(input.FilterText)
               ).WhereIf(!input.ServiceName.IsNullOrWhiteSpace(), a =>
                    a.ApplyTypeName == (input.ServiceName));

            var count = await query.CountAsync();

            var applyList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var applyListDtos = ObjectMapper.Map<List<ApplyListDto>>(applyList);

            return new PagedResultDto<ApplyListDto>(count, applyListDtos);
        }

        /// <summary>
        /// 根据ID获取APP网关应用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApplyListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _applyManager
              .QueryAsNoTracking
              .FirstOrDefaultAsync(x => x.Id == input.Id);

            var dto = ObjectMapper.Map<ApplyListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 根据Code获取APP网关应用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApplyListDto> GetByCode(string code)
        {
            var entity = await _applyManager
              .QueryAsNoTracking
              .FirstOrDefaultAsync(x => x.ApplyCode == code);

            var dto = ObjectMapper.Map<ApplyListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 修改连接应用网关配置数据（连接应用地址、身份验证模式、健康检测配置）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Abp.UI.UserFriendlyException"></exception>
        [HttpPost]
        public async Task UpdateApplySetting(UpdateApplySettingInput input)
        {
            var applyEditList = ObjectMapper.Map<Apply>(input.ApplyEditDto);
            var applyAuthenticationentity = ObjectMapper.Map<ApplyAuthentication>(input.ApplyAuthenticationEditDto);
            var applyHealthTestingentity = ObjectMapper.Map<ApplyHealthTesting>(input.ApplyHealthTestingEditDto);

            if (string.IsNullOrWhiteSpace(applyHealthTestingentity.CtiTypeName))
            {
                applyHealthTestingentity.CtiTypeName = "秒";
                applyHealthTestingentity.CtiTypeCode = "秒";
            }
            if (string.IsNullOrWhiteSpace(applyHealthTestingentity.SITypeName))
            {
                applyHealthTestingentity.SITypeName = "秒";
                applyHealthTestingentity.SITypeCode = "秒";
            }
            if (string.IsNullOrWhiteSpace(applyHealthTestingentity.DITypeName))
            {
                applyHealthTestingentity.DITypeName = "秒";
                applyHealthTestingentity.DITypeCode = "秒";
            }
            if (string.IsNullOrWhiteSpace(applyHealthTestingentity.TOTypeName))
            {
                applyHealthTestingentity.TOTypeName = "秒";
                applyHealthTestingentity.TOTypeCode = "秒";
            }
            if (string.IsNullOrWhiteSpace(applyHealthTestingentity.RITypeName))
            {
                applyHealthTestingentity.RITypeName = "秒";
                applyHealthTestingentity.RITypeCode = "秒";
            }
            if (string.IsNullOrWhiteSpace(applyHealthTestingentity.CFTypeName))
            {
                applyHealthTestingentity.CFTypeName = "分钟";
                applyHealthTestingentity.CFTypeCode = "分钟";
            }

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                await _applyManager.UpdateAsync(applyEditList);

                ///保存 身份验证模式信息
                if (input.ApplyAuthenticationEditDto.Id.HasValue)
                {
                    await _authenticationManager.UpdateAsync(applyAuthenticationentity);
                }
                else
                {
                    await _authenticationManager.CreateAsync(applyAuthenticationentity);
                }

                ///保存 连接健康配置信息
                if (input.ApplyHealthTestingEditDto.Id.HasValue)
                {
                    await _applyHealthTestingManager.UpdateAsync(applyHealthTestingentity);
                }
                else
                {
                    await _applyHealthTestingManager.CreateAsync(applyHealthTestingentity);
                }

                unitOfWork.Complete();
            }
        }

        public async Task<List<TestingApplyHealthDto>> GetApplyHearth()
        {
            var applyEntity = await _applyManager.QueryAsNoTracking.ToListAsync();
            var applyHeath = await _applyHealthTestingManager.QueryAsNoTracking
                         .Where(t => applyEntity.Select(m => m.Id).Contains(t.ApplyID))
                         //.Where(t => t.HealthTestingModeName == "HTTP URL模式")
                         .ToListAsync();

            List<TestingApplyHealthDto> healthList = new List<TestingApplyHealthDto>();
            foreach (var item in applyHeath)
            {
                string heathKey = "ApplyHealthTesting-" + item.ApplyID.ToString();
                //找到缓存中的异常记录
                ITypedCache<string, CacheItemDto> API_FC_Cache = _cacheManager.GetCache(heathKey).AsTyped<string, CacheItemDto>();

                var isSetting = API_FC_Cache.TryGetValue(heathKey, out CacheItemDto cache_limit);
                if (isSetting)
                {
                    foreach (var healthDto in cache_limit.testingApplyHealths)
                    {
                        healthList.Add(healthDto);
                    }
                }
            }
            return healthList;
        }
    }
}
