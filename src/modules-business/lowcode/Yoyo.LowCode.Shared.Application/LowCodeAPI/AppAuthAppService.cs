// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.LowCodeAPI.DomainService;
using Yoyo.LowCode.LowCodeAPI.Dtos.AppAuthDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class AppAuthAppService : LowCodeSharedAppServiceBase, IAppAuthAppService
    {
        #region Private Fields

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IAppAuthManager _authManager;
        private readonly IAppAuthMappingManager _appAuthMappingManager;

        #endregion Private Fields

        #region Public Constructors

        public AppAuthAppService(IAppAuthManager authManager,
            IAppAuthMappingManager authMappingManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _authManager = authManager;
            _appAuthMappingManager = authMappingManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        #endregion Public Constructors

        /// <summary>
        /// 批量删除应用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            await _authManager.BatchDelete(input);
        }

        /// <summary>
        ///  新增 或者修改 接口中心授权应用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CreateOrUpdate(CreateOrUpdateAppAuth input)
        {
            var appAuthList = await _authManager.QueryAsNoTracking.Where(t => t.Id != input.AppAuthEditDto.Id).ToListAsync();

            if (appAuthList.Any(t => t.AppName == input.AppAuthEditDto.AppName))
            {
                throw new Abp.UI.UserFriendlyException(L($"当前应用名称{input.AppAuthEditDto.AppName}已被使用，请重新输入名称"));
            }

            var entity = ObjectMapper.Map<AppAuth>(input.AppAuthEditDto);
            ///新增还是修改
            if (input.AppAuthEditDto.Id.HasValue)
            {
                await _authManager.UpdateAsync(entity);
            }
            else
            {
                entity.AppKey = GenerateKey(256);
                await _authManager.CreateAsync(entity);
            }
        }

        /// <summary>
        /// 删除应用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _authManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 分页获取接口中心授权应用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<AppAuthListDto>> GetAppAuthList(GetAppAuthPagesInput input)
        {
            var query = _authManager.Query
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>
                  a.AppName.Contains(input.FilterText)
              ).WhereIf(input.StartTime.HasValue, a => a.CreationTime >= input.StartTime
              ).WhereIf(input.EndTime.HasValue, a => a.CreationTime <= input.EndTime);

            var count = await query.CountAsync();

            var DataList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<AppAuthListDto>>(DataList);

            return new PagedResultDto<AppAuthListDto>(count, DataListDtos);
        }

        /// <summary>
        ///  根据ID查询 接口应用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<AppAuthListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _authManager
            .QueryAsNoTracking
            .FirstOrDefaultAsync(x => x.Id == input.Id);

            var dto = ObjectMapper.Map<AppAuthListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 新增修改 AppAuthMapping
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CreateOrUpdateForAppAuthMapping(CreateOrUpdateAppAuthMapping input)
        {
            var appAuthID = input.AppAuthID;
            var ids = _appAuthMappingManager.QueryAsNoTracking.Where(t => t.AppAuthID == appAuthID).Select(t => t.Id).ToList();

            var entity = ObjectMapper.Map<List<AppAuthMapping>>(input.AppAuthMappingEditDto);
            foreach (var item in entity)
            {
                item.Id = Guid.NewGuid();
                item.IsDeleted = false;
            }
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                await _appAuthMappingManager.BatchDelete(ids);
                await _appAuthMappingManager.CreateAsync(entity);
                unitOfWork.Complete();
            }
        }

        /// <summary>
        /// 获取接口中心应用下  AppAuthMapping 列表
        /// </summary>
        /// <param name="AppAuthID"></param>
        /// <returns></returns>
        public async Task<List<AppAuthMappingListDto>> GetAppAuthMappingList(GetAppAuthMappingPagesInput input)
        {
            var DataList = await _appAuthMappingManager.Query
                .Where(t => t.AppAuthID == input.AppAuthID)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<AppAuthMappingListDto>>(DataList);

            return DataListDtos;
        }

        /// <summary>
        /// 生成一个随机密钥
        /// </summary>
        /// <param name="keySize"></param>
        /// <returns></returns>
        private string GenerateKey(int keySize)
        {
            using var randomNumberGenerator = new RNGCryptoServiceProvider();
            var key = new byte[keySize / 8];
            randomNumberGenerator.GetBytes(key);
            return Convert.ToBase64String(key);
        }

        /// <summary>
        /// 校验 key密钥是否拥有这个api授权(0 校验成功，1 key值有误，2 应用下没有这个授权)
        /// </summary>
        /// <param name="AppKey"></param>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public async Task<int> CheckAppAuthYoyoApiGatewayToken(string AppKey, Guid AgentID)
        {
            var entity = await _authManager
                         .QueryAsNoTracking
                         .FirstOrDefaultAsync(x => x.AppKey == AppKey);
            //key不存在
            if (entity == null)
            {
                return 1;
            }

            //key +  agent 是否有这个agentapi授权数据
            var MappingList = await _appAuthMappingManager.Query
                .Where(t => t.AppAuthID == entity.Id && t.AgentID == AgentID)
                .ToListAsync();
            if (MappingList.Count > 0)
            {
                return 0;
            }
            return 2;
        }
    }
}
