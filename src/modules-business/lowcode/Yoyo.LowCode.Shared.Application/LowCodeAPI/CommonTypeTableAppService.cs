// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.LowCodeAPI.DomainService;
using Yoyo.LowCode.LowCodeAPI.Dtos.CommonTypeTableDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class CommonTypeTableAppService : LowCodeSharedAppServiceBase, ICommonTypeTableAppService
    {
        #region Private Fields

        private readonly ICommonTypeTableManager _commonTypeTableManager;

        #endregion Private Fields

        #region Public Constructors

        public CommonTypeTableAppService(ICommonTypeTableManager commonTypeTable)
        {
            _commonTypeTableManager = commonTypeTable;
        }

        #endregion Public Constructors

        /// <summary>
        /// 批量删除 系统参数配置信息
        /// </summary>
        public async Task BatchDelete(List<Guid> input)
        {
            await _commonTypeTableManager.BatchDelete(input);
        }

        /// <summary>
        /// 添加或者修改 系统参数配置信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdate(CreateOrUpdateCommonTypeTable input)
        {
            var commonTypeTableList = await _commonTypeTableManager.QueryAsNoTracking.Where(t => t.TableName == input.CommonTypeTableEditDto.TableName
             && t.Id != input.CommonTypeTableEditDto.Id).ToListAsync();

            if (commonTypeTableList.Any(t => t.Name == input.CommonTypeTableEditDto.Name))
            {
                throw new Abp.UI.UserFriendlyException(L($"当前分组下参数名称{input.CommonTypeTableEditDto.Name}已被使用，请重新输入名称"));
            }

            if (commonTypeTableList.Any(t => t.Code == input.CommonTypeTableEditDto.Code))
            {
                throw new Abp.UI.UserFriendlyException(L($"当前分组下参数编码{input.CommonTypeTableEditDto.Name}已被使用，请重新输入编码"));
            }

            var entity = ObjectMapper.Map<CommonTypeTable>(input.CommonTypeTableEditDto);
            ///新增还是修改
            if (input.CommonTypeTableEditDto.Id.HasValue)
            {
                await _commonTypeTableManager.UpdateAsync(entity);
            }
            else
            {
                await _commonTypeTableManager.CreateAsync(entity);
            }
        }

        /// <summary>
        /// 删除 系统参数配置信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _commonTypeTableManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 获取系统参数配置信息（TableName）
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CommonTypeTableListDto>> GetApplyAPIList(GetCommonTypeTablePagesInput input)
        {
            var query = _commonTypeTableManager.Query
                 .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>
                   a.Name.Contains(input.FilterText) ||
                   a.Code.Contains(input.FilterText)
               ).WhereIf(!string.IsNullOrEmpty(input.TableName), a =>
                  a.TableName == input.TableName
              );

            var count = await query.CountAsync();

            var DataList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<CommonTypeTableListDto>>(DataList);

            return new PagedResultDto<CommonTypeTableListDto>(count, DataListDtos);
        }

        /// <summary>
        /// 通过指定id获取 系统参数配置信息
        /// </summary>
        public async Task<CommonTypeTableListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _commonTypeTableManager
           .QueryAsNoTracking
           .FirstOrDefaultAsync(x => x.Id == input.Id);

            var dto = ObjectMapper.Map<CommonTypeTableListDto>(entity);
            return dto;
        }
    }
}
