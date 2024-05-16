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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.Dtos;
using Yoyo.LowCode.UserQueryModule.UserQueryDefinition.DomainService;
using Yoyo.LowCode.UserQueryModule.UserQueryDefinition.Dtos;
using Yoyo.Pro.Dtos;

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition
{
    /// <summary>
    /// 用户查询
    /// </summary>
    public class BaseUserQueryAppService : LowCodeSharedAppServiceBase, IBaseUserQueryAppService
    {
        private readonly IUserQueryManager _userQueryManager;
        private readonly IUserQueryExecuter _userQueryExecuter;

        public BaseUserQueryAppService(IUserQueryManager userQueryManager, IUserQueryExecuter userQueryExecuter)
        {
            _userQueryManager = userQueryManager;
            _userQueryExecuter = userQueryExecuter;
        }

        #region 查询

        /// <inheritdoc/>
        //[AbpAuthorize(UserQueryPermissions.UserQuery_Query)]
        public async Task<PagedResultDto<BaseUserQueryListDto>> GetNdoPaged(GetBaseUserQueryInput input)
        {
            var query = _userQueryManager.QueryAsNoTracking
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Name.Contains(input.FilterText));

            var totalCount = await query.CountAsync();
            var totalList = await query
                .Select(x => new BaseUserQueryListDto { Id = x.Id, Name = x.Name, CreationTime = x.CreationTime, Description = x.Description })
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<BaseUserQueryListDto>(totalCount, totalList);
        }

        /// <inheritdoc/>
        [HttpPost]
        public async Task<List<BaseUserQueryListDto>> GetNdoCombox(LowCodeGetQueryFilterInput input)
        {
            var ret = await _userQueryManager.QueryAsNoTracking
                .WhereIf(!input.FilterText.IsNullOrEmpty(),
                    p => p.Name.Contains(input.FilterText))
                .Select(x => new BaseUserQueryListDto { Id = x.Id, Name = x.Name, CreationTime = x.CreationTime, Description = x.Description })
                .OrderBy(input.Sorting)
                .ToListAsync();

            return ret;
        }

        [HttpPost]
        public async Task<List<LowCodeNdoDto>> GetNdo(LowCodeGetQueryFilterInput input)
        {
            var ret = await _userQueryManager.QueryAsNoTracking
                .WhereIf(!input.FilterText.IsNullOrEmpty(),
                    p => p.Name.Contains(input.FilterText))
                .Select(x => new LowCodeNdoDto { Id = x.Id, Name = x.Name, CreationTime = x.CreationTime })
                .OrderBy(input.Sorting)
                .ToListAsync();

            return ret;
        }

        /// <inheritdoc/>
        public async Task<List<ComboxDto<string>>> GetUserQueryParamterTypeCombox()
        {
            await Task.Yield();

            return new List<ComboxDto<string>>()
            {
                new ComboxDto<string>(UserQueryConsts.UserQueryParamterType.Boolean,UserQueryConsts.UserQueryParamterType.Boolean),
                new ComboxDto<string>(UserQueryConsts.UserQueryParamterType.Decimal,UserQueryConsts.UserQueryParamterType.Decimal),
                new ComboxDto<string>(UserQueryConsts.UserQueryParamterType.Float,UserQueryConsts.UserQueryParamterType.Float),
                new ComboxDto<string>(UserQueryConsts.UserQueryParamterType.Integer,UserQueryConsts.UserQueryParamterType.Integer),
                new ComboxDto<string>(UserQueryConsts.UserQueryParamterType.String,UserQueryConsts.UserQueryParamterType.String),
                new ComboxDto<string>(UserQueryConsts.UserQueryParamterType.Timestamp,UserQueryConsts.UserQueryParamterType.Timestamp)
            };
        }

        /// <inheritdoc/>
        //[AbpAuthorize(UserQueryPermissions.UserQuery_Create, UserQueryPermissions.UserQuery_Edit)]
        public async Task<GetBaseUserQueryForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetBaseUserQueryForEditOutput();

            if (input.Id.HasValue)
            {
                var entity = await _userQueryManager.QueryAsNoTracking
                    .Include(x => x.QueryParamters)
                    .FirstOrDefaultAsync(x => x.Id == input.Id.Value);

                output.EntityDto = ObjectMapper.Map<BaseUserQueryEditDto>(entity);
                //操作人信息
                output.OperationTime = entity.LastModificationTime ?? entity.CreationTime;
                output.OperationName = entity.LastModifierUserName ?? entity.CreatorUserName;
            }
            else
            {
                output.EntityDto = new BaseUserQueryEditDto();
            }

            return output;
        }

        #endregion 查询

        #region 增删改

        /// <inheritdoc/>
        //[AbpAuthorize(UserQueryPermissions.UserQuery_Create, UserQueryPermissions.UserQuery_Edit)]
        public async Task<Guid> CreateOrUpdate(BaseUserQueryCreateOrUpdateInput input)
        {
            if (input.EntityDto.Id.HasValue)
            {
                return await this.Update(input.EntityDto);
            }

            return await this.Create(input.EntityDto);
        }

        /// <inheritdoc/>
        //[AbpAuthorize(UserQueryPermissions.UserQuery_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _userQueryManager.Delete(input.Id);
        }

        #endregion 增删改

        /// <inheritdoc/>
        //[AbpAuthorize(UserQueryPermissions.UserQuery_Create, UserQueryPermissions.UserQuery_Edit)]
        public async Task<BaseUserQueryTestOutput> Test(BaseUserQueryTestInput input)
        {
            var userQuery = ObjectMapper.Map<BaseUserQuery>(input.EntityDto);

            var dataTable = await _userQueryExecuter.Execute(userQuery);

            var outputDto = new BaseUserQueryTestOutput()
            {
                Columns = new List<string>(),
                Rows = new List<List<object>>()
            };
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                outputDto.Columns.Add(dataTable.Columns[i].ColumnName);
            }

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i];
                var rowData = new List<object>();
                foreach (var col in outputDto.Columns)
                {
                    rowData.Add(row[col]);
                }
                outputDto.Rows.Add(rowData);
            }

            return outputDto;
        }

        #region 新增、修改 内部方法

        /// <summary>
        /// 新增
        /// </summary>
        //[AbpAuthorize(UserQueryPermissions.UserQuery_Create)]
        protected virtual async Task<Guid> Create(BaseUserQueryEditDto input)
        {
            // 新增前的逻辑判断，是否允许新增，到领域服务中进行完善，不再这里进行判断，原因是为了复用。
            var entity = ObjectMapper.Map<BaseUserQuery>(input);

            //调用领域服务
            await _userQueryManager.Create(entity);

            return entity.Id;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        //[AbpAuthorize(UserQueryPermissions.UserQuery_Edit)]
        protected virtual async Task<Guid> Update(BaseUserQueryEditDto input)
        {
            // 更新前的逻辑判断，是否允许更新，到领域服务中进行完善，不再这里进行判断，原因是为了复用。
            if (input.Id != null)
            {
                var entity = await _userQueryManager.QueryAsNoTracking
                    .Include(x => x.QueryParamters)
                    .FirstOrDefaultAsync(x => x.Id == input.Id.Value);

                //将input属性的值赋值到entity中
                ObjectMapper.Map(input, entity);

                // 提交到数据库
                await _userQueryManager.Update(entity);

                return entity.Id;
            }
            return Guid.Empty;
        }

        #endregion 新增、修改 内部方法
    }
}
