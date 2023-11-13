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
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.LowCodeAPI.DomainService;
using Yoyo.LowCode.LowCodeAPI.Dtos.FlowControlDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class FlowControlAppService : LowCodeSharedAppServiceBase, IFlowControlAppService
    {
        #region Private Fields

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IFlowControlManager _flowManager;
        private readonly IFlowControlMappingManager _flowMappingManager;

        #endregion Private Fields

        #region Public Constructors

        public FlowControlAppService(IFlowControlManager flowManager,
            IFlowControlMappingManager flowMappingManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _flowManager = flowManager;
            _flowMappingManager = flowMappingManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        #endregion Public Constructors

        /// <summary>
        /// 批量删除 流量控制策略
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            await _flowManager.BatchDelete(input);
        }

        /// <summary>
        ///  新增 或者修改 流量控制策略
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CreateOrUpdate(CreateOrUpdateFlowControl input)
        {
            var flowControlList = await _flowManager.QueryAsNoTracking.Where(t => t.Id != input.FlowControlEditDto.Id).ToListAsync();

            if (flowControlList.Any(t => t.FlowName == input.FlowControlEditDto.FlowName))
            {
                throw new Abp.UI.UserFriendlyException(L($"当前流量控制策略名称{input.FlowControlEditDto.FlowName}已被使用，请重新输入名称"));
            }

            var entity = ObjectMapper.Map<FlowControl>(input.FlowControlEditDto);
            ///新增还是修改
            if (input.FlowControlEditDto.Id.HasValue)
            {
                await _flowManager.UpdateAsync(entity);
            }
            else
            {
                await _flowManager.CreateAsync(entity);
            }
        }

        /// <summary>
        /// 删除流量控制策略
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _flowManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 分页获取 流量控制策略
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<FlowControlListDto>> GetFlowControlList(GetFlowControlPagesInput input)
        {
            var query = _flowManager.Query
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>
                  a.FlowName.Contains(input.FilterText)
              ).WhereIf(input.StartTime.HasValue, a => a.CreationTime >= input.StartTime
              ).WhereIf(input.EndTime.HasValue, a => a.CreationTime <= input.EndTime);

            var count = await query.CountAsync();

            var DataList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<FlowControlListDto>>(DataList);

            return new PagedResultDto<FlowControlListDto>(count, DataListDtos);
        }

        /// <summary>
        ///  根据ID查询 接口应用策略
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<FlowControlListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _flowManager
            .QueryAsNoTracking
            .FirstOrDefaultAsync(x => x.Id == input.Id);

            var dto = ObjectMapper.Map<FlowControlListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 新增修改 流量控制策略对应接口关系
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CreateOrUpdateForFlowControlMapping(CreateOrUpdateFlowControlMapping input)
        {
            var flowID = input.FlowID;
            var ids = _flowMappingManager.QueryAsNoTracking.Where(t => t.FlowID == flowID).Select(t => t.Id).ToList();

            var entity = ObjectMapper.Map<List<FlowControlMapping>>(input.FlowControlMappingEditDto);
            foreach (var item in entity)
            {
                item.Id = Guid.NewGuid();
                item.IsDeleted = false;
            }
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                await _flowMappingManager.BatchDelete(ids);
                await _flowMappingManager.CreateAsync(entity);
                unitOfWork.Complete();
            }
        }

        /// <summary>
        /// 获取接口中心流量控制策略  FlowControlMapping 列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<FlowControlMappingEditDto>> GetFlowControlMappingList(GetFlowControlMappingPagesInput input)
        {
            var DataList = await _flowMappingManager.Query
                .Where(t => t.FlowID == input.FlowID)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<FlowControlMappingEditDto>>(DataList);

            return DataListDtos;
        }

        /// <summary>
        /// 查询 AgentAPI启用的流量策略
        /// </summary>
        /// <param name="AppKey"></param>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public async Task<FlowControl> GetFlowControlByAgentID(Guid AgentID)
        {
            var mapping = await _flowMappingManager
                         .QueryAsNoTracking
                         .FirstOrDefaultAsync(x => x.AgentID == AgentID);

            //没有这个接口对应的流量策略
            if (mapping == null)
            {
                return null;
            }

            return await _flowManager.Query
                .Where(t => t.Id == mapping.FlowID)
                .FirstOrDefaultAsync();
        }
    }
}
