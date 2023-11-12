// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.Dtos;
using Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition.DomainService;
using Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition.Dtos;
using Yoyo.LowCode.UserQueryModule.UserQueryRelationalDefinition.DomainService;

namespace Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition
{
    /// <summary>
    /// 用户组查询
    /// </summary>
    public class BaseUserQueryGroupAppService : LowCodeSharedAppServiceBase, IBaseUserQueryGroupAppService
    {
        #region Private Fields

        private readonly IUserQueryGroupEntityManager _userQueryGroupEntityManager;
        private readonly IUserQueryGroupManager _userQueryGroupManager;
        private readonly IUserQueryGroupSubManager _userQueryGroupSubManager;

        #endregion Private Fields

        #region Public Constructors

        public BaseUserQueryGroupAppService(IUserQueryGroupManager userQueryGroupManager, IUserQueryGroupEntityManager userQueryGroupEntityManager, IUserQueryGroupSubManager userQueryGroupSubManager)
        {
            _userQueryGroupManager = userQueryGroupManager;
            _userQueryGroupEntityManager = userQueryGroupEntityManager;
            _userQueryGroupSubManager = userQueryGroupSubManager;
        }

        #endregion Public Constructors

        #region 查询

        //[AbpAuthorize(UserQueryGroupPermissions.UserQueryGroup_Create, UserQueryGroupPermissions.UserQueryGroup_Edit)]
        public async Task<GetBaseUserQueryGroupForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetBaseUserQueryGroupForEditOutput();

            if (input.Id.HasValue)
            {
                var entity = await _userQueryGroupManager.QueryAsNoTracking
                    .FirstOrDefaultAsync(x => x.Id == input.Id.Value);

                output.EntityDto = ObjectMapper.Map<BaseUserQueryGroupEditDto>(entity);
                //操作人信息
                output.OperationTime = entity.LastModificationTime ?? entity.CreationTime;
                output.OperationName = entity.LastModifierUserName ?? entity.CreatorUserName;
            }
            else
            {
                output.EntityDto = new BaseUserQueryGroupEditDto();
            }

            return output;
        }

        [HttpPost]
        public async Task<List<BaseUserQueryGroupListDto>> GetNdoCombox(LowCodeGetQueryFilterInput input)
        {
            var ret = await _userQueryGroupManager.QueryAsNoTracking
                .WhereIf(!input.FilterText.IsNullOrEmpty(),
                    p => p.Name.Contains(input.FilterText))
                .Select(x => new BaseUserQueryGroupListDto { Id = x.Id, Name = x.Name, CreationTime = x.CreationTime })
                .OrderBy(input.Sorting)
                .ToListAsync();

            return ret;
        }

        //[AbpAuthorize(UserQueryGroupPermissions.UserQueryGroup_Query)]
        public async Task<PagedResultDto<BaseUserQueryGroupListDto>> GetNdoPaged(GetBaseUserQueryGroupInput input)
        {
            var query = _userQueryGroupManager.QueryAsNoTracking
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Name.Contains(input.FilterText));

            var totalCount = await query.CountAsync();
            var totalList = await query
                .Select(x => new BaseUserQueryGroupListDto { Id = x.Id, Name = x.Name, CreationTime = x.CreationTime })
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<BaseUserQueryGroupListDto>(totalCount, totalList);
        }

        #endregion 查询

        #region 增删改

        //[AbpAuthorize(UserQueryGroupPermissions.UserQueryGroup_Create, UserQueryGroupPermissions.UserQueryGroup_Edit)]
        public async Task<Guid> CreateOrUpdate(BaseUserQueryGroupCreateOrUpdateInput input)
        {
            if (input.EntityDto.Id.HasValue)
            {
                return await this.Update(input.EntityDto);
            }

            return await this.Create(input.EntityDto);
        }

        //[AbpAuthorize(UserQueryGroupPermissions.UserQueryGroup_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            var entity = await _userQueryGroupManager.QueryAsNoTracking
                .Include(x => x.Entitys)
                .Include(x => x.SubGroups)
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            // 这个方法会删除与之相关的联系表 2个联系表
            await _userQueryGroupManager.DeleteNdoAndEntry(input.Id);
        }

        #endregion 增删改

        #region 新增、修改 内部方法

        /// <summary>
        /// 新增
        /// </summary>
        //[AbpAuthorize(UserQueryGroupPermissions.UserQueryGroup_Create)]
        protected virtual async Task<Guid> Create(BaseUserQueryGroupEditDto input)
        {
            // 新增前的逻辑判断，是否允许新增，到领域服务中进行完善，不再这里进行判断，原因是为了复用。
            var entity = ObjectMapper.Map<BaseUserQueryGroup>(input);

            //调用领域服务
            await _userQueryGroupManager.Create(entity);

            return entity.Id;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        //[AbpAuthorize(UserQueryGroupPermissions.UserQueryGroup_Edit)]
        protected virtual async Task<Guid> Update(BaseUserQueryGroupEditDto input)
        {
            // 更新前的逻辑判断，是否允许更新，到领域服务中进行完善，不再这里进行判断，原因是为了复用。
            if (input.Id != null)
            {
                var entity = await _userQueryGroupManager.QueryAsNoTracking
                    .Include(x => x.Entitys)
                    .Include(x => x.SubGroups)
                    .FirstOrDefaultAsync(x => x.Id == input.Id.Value);

                // 处理关联的userquery
                var userQueryGroupEntitys = await ProcessList(entity.Entitys, input.Entitys, (createData) =>
                {
                    return this._userQueryGroupEntityManager.Create(createData);
                }, (updateData) =>
                {
                    return this._userQueryGroupEntityManager.Update(updateData);
                }, (delData) =>
                {
                    return this._userQueryGroupEntityManager.Delete(delData);
                });
                // 处理关联的userquerygroup
                var userQueryGroupSubs = await ProcessList(entity.SubGroups, input.SubGroups, (createData) =>
                {
                    return this._userQueryGroupSubManager.Create(createData);
                }, (updateData) =>
                {
                    return this._userQueryGroupSubManager.Update(updateData);
                }, (delData) =>
                {
                    return this._userQueryGroupSubManager.Delete(delData);
                });

                //将input属性的值赋值到entity中
                ObjectMapper.Map(input, entity);

                entity.Entitys = userQueryGroupEntitys;
                entity.SubGroups = userQueryGroupSubs;

                // 提交到数据库
                await _userQueryGroupManager.Update(entity);

                return entity.Id;
            }

            return Guid.Empty;
        }

        /// <summary>
        /// 处理子列表
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TEntityDto">Dto类型</typeparam>
        /// <param name="entitys">当前数据库中的数据</param>
        /// <param name="entityDtos">请求输入dto数据</param>
        /// <param name="createFunc">创建方法</param>
        /// <param name="updateFunc">修改方法</param>
        /// <param name="deleteFunc">删除方法</param>
        /// <returns></returns>
        private async Task<List<TEntity>> ProcessList<TEntity, TEntityDto>(IEnumerable<TEntity> entitys, IEnumerable<TEntityDto> entityDtos, Func<TEntity, Task> createFunc, Func<TEntity, Task> updateFunc, Func<TEntity, Task> deleteFunc)
            where TEntityDto : EntityDto<Guid?>
            where TEntity : Entity<Guid>
        {
            var resList = new List<TEntity>();

            var oldList = entitys;
            var newList = ObjectMapper.Map<List<TEntity>>(entityDtos);

            // 删除
            var deleteList = oldList.Except(newList);
            foreach (var delItem in deleteList)
            {
                await deleteFunc.Invoke(delItem);
            }

            // 修改
            var updateList = entityDtos.Where(_ => _.Id != default);
            foreach (var updateItem in updateList)
            {
                var oldItem = oldList.Where(_ => _.Id == updateItem.Id).FirstOrDefault();
                ObjectMapper.Map(updateItem, oldItem);
                await updateFunc.Invoke(oldItem);
                resList.Add(oldItem);
            }

            // 添加
            var addList = newList.Where(_ => _.Id == default);
            foreach (var addItem in addList)
            {
                await createFunc.Invoke(addItem);
                resList.Add(addItem);
            }
            return resList;
        }

        #endregion 新增、修改 内部方法
    }
}
