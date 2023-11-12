// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Yoyo.LowCode.Databases.DomainService;
using Yoyo.LowCode.Databases.Dtos;
using Yoyo.LowCode.LowCodeViewModels.DomainService;
using Yoyo.LowCode.LowCodeViewModels.Dtos;
using Yoyo.LowCode.Models;

namespace Yoyo.LowCode.LowCodeViewModels
{
    public class LowCodeViewModelsAppService : LowCodeSharedAppServiceBase, ILowCodeViewModelsAppService
    {
        private readonly IViewModelManager _ModelManager;

        private readonly ILowCodeModelRelationManager _ModelRelationManager;

        private readonly IDatabaseManager _databaseManager;

        private readonly ILowCodeFieldManager _fieldManager;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IWebHostEnvironment _env;

        public LowCodeViewModelsAppService(IViewModelManager viewPageManager,
            ILowCodeModelRelationManager LowCodeModelRelationManager,
            IDatabaseManager databaseManager,
            ILowCodeFieldManager fieldCollectionManager,
            IUnitOfWorkManager unitOfWorkManager,
            IWebHostEnvironment env = null)
        {
            _ModelManager = viewPageManager;
            _ModelRelationManager = LowCodeModelRelationManager;
            _databaseManager = databaseManager;
            _fieldManager = fieldCollectionManager;
            _unitOfWorkManager = unitOfWorkManager;
            _env = env;
        }

        /// <summary>
        /// 获取所有表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<TableListOutput>> GetTableAsTable(GetDatabaseTableListInput input)
        {
            var ret = await GetTableList(input);

            if (input.IsFilterDefault)
            {
                var path = Path.Combine(_env.WebRootPath ?? string.Empty, "lowcode", "SystemTable.json");
                if (!File.Exists(path))
                {
                    return null;
                }

                var listViewJson = await File.ReadAllTextAsync(path);
                JObject jObject = JObject.Parse(listViewJson);
                List<string> list = jObject["exclude"].ToObject<List<string>>();
                foreach (var item in list)
                {
                    ret.RemoveAll(x => x.Table == item);
                }
            }
            return ObjectMapper.Map<List<TableListOutput>>(ret);
        }

        /// <summary>
        /// 为下拉列表获取表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<TableListSelectOutput>> GetTableAsSelect(GetDatabaseTableListInput input)
        {
            var ret = await GetTableList(input);
            return ObjectMapper.Map<List<TableListSelectOutput>>(ret);
        }

        /// <summary>
        /// 通过主表名获取所有子表名称
        /// </summary>
        /// <param name="mainTableName"></param>
        /// <returns></returns>
        public async Task<List<TableListSelectOutput>> GetTableByAssociation(string mainTableName)
        {
            var data = await _ModelRelationManager.QueryAsNoTracking
                .WhereIf(mainTableName.IsNullOrWhiteSpace(), x => x.MainModelName == mainTableName).ToListAsync();
            var outPutLsit = new List<TableListSelectOutput>();
            foreach (var item in data)
            {
                var outPut = new TableListSelectOutput();
                outPut.TableName = item.ChildModelName;
                outPut.TableDesc = item.ChildModelName;
                outPutLsit.Add(outPut);
            }
            return outPutLsit;
        }

        /// <summary>
        /// 获取所有表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected async Task<List<DbTableModel>> GetTableList(GetDatabaseTableListInput input)
        {
            var data = await _databaseManager.GetTableList(input.DbLinkId);

            data.ForEach(ret =>
            {
                ret.TableName ??= " ";
            });

            input.FilterText = input.FilterText?.ToLower();
            var ret = data
                .WhereIf(!string.IsNullOrEmpty(input.FilterText),
                    d => d.Table.ToLower().Contains(input.FilterText) ||
                         d.TableName.Contains(input.FilterText)).ToList();

            return ret;
        }

        /// <summary>
        /// 为表格获取表字段数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<LowCodeFieldListDto>> GetTableFieldAsTable(GetTableFieldListInput input)
        {
            var entity = await GetTableFieldList(input);

            return ObjectMapper.Map<List<LowCodeFieldListDto>>(entity);
        }

        /// <summary>
        /// 为下拉列表获取表字段数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<LowCodeTableFieldsSelectorOutput>> GetTableFieldAsSelect(GetTableFieldListInput input)
        {
            var entity = await GetTableFieldList(input);

            return ObjectMapper.Map<List<LowCodeTableFieldsSelectorOutput>>(entity);
        }

        public async Task<bool> TableIsExists(List<TableInfoList> list)
        {
            var data = await _databaseManager.GetTableList(null);
            foreach (var item in list)
            {
                var isExis = data.Find(x => x.Table.ToUpper().Trim() == item.tableName.ToUpper().Trim());
                if (isExis == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取表的主键字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> GetTablePrimaryKey(GetTableFieldListInput input)
        {
            var list = await GetTableFieldList(input);
            if (list.Count != 0)
            {
                return list.Find(x => x.IsPrimaryKey).FieldName;
            }
            return null;
        }

        /// <summary>
        /// 获取指定表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected async Task<List<LowCodeField>> GetTableFieldList(GetTableFieldListInput input)
        {
            var relation = await _ModelRelationManager.QueryAsNoTracking
                .Where(x => x.ChildModelName == input.Table).FirstOrDefaultAsync();

            var oldfield = await _fieldManager.QueryAsNoTracking.
                Where(x => x.IsForeignkey && x.TableName == input.Table).FirstOrDefaultAsync();

            if (relation != null && oldfield != null)
            {
                oldfield.LowCodeModelRelation = relation;

                await _fieldManager.Update(oldfield);
            }

            var entity = await _databaseManager.GetFieldList(input.DbLinkId, input.Table);

            var oldfieldList = ObjectMapper.Map<List<LowCodeField>>(entity);

            var fieldList = await _fieldManager.Query.Include(x => x.LowCodeModelRelation).
                Where(x => x.TableName == input.Table).ToListAsync();

            foreach (var item in oldfieldList)
            {
                item.TableName = input.Table;
                var isexit = fieldList.FirstOrDefault(x => x.FieldName == item.FieldName);
                if (isexit == null)
                {
                    fieldList.Add(item);
                }
            }
            return fieldList;
        }

        /// <summary>
        /// 获取表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<object>> GetTableData(GetTableDataInput input)
        {
            return await _databaseManager.GetData(input.DbLinkId, input.Table, input.MaxResultCount, input.SkipCount);
        }

        /// <summary>
        /// 清空表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteTableData(DeleteTableDataInput input)
        {
            await _databaseManager.DeleteTableData(input.DbLinkId, input.TableName);
        }

        /// <summary>
        /// 添加表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task CreateTable(LowCodeCreateOrUpdateTableInput input)
        {
            #region 表名重复校验

            var tables = await _databaseManager.GetTableList(input.DbLinkId);

            var isExistTable = tables.Any(x => x.Table == input.NewTableInfo.NewTableName);

            if (isExistTable)
            {
                throw new UserFriendlyException(L("Error"), "表已存在");
            }

            #endregion 表名重复校验

            var tableInfo = ObjectMapper.Map<DbTableModel>(input.NewTableInfo);
            tableInfo.Table = input.NewTableInfo.NewTableName;

            var tableFieldList = ObjectMapper.Map<List<DbTableFieldModel>>(input.TableFieldList);

            using (var unitOfWork = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                await _databaseManager.Create(input.DbLinkId, tableInfo, tableFieldList);

                await CreateOrUpdateLowCodeModel(input.NewTableInfo);

                //维护表、字段创建信息
                await CreateField(input.TableFieldList);

                unitOfWork.Complete();
            }
        }

        /// <summary>
        /// 修改表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task UpdateTable(LowCodeCreateOrUpdateTableInput input)
        {
            var tableInfo = ObjectMapper.Map<DbTableModel>(input.NewTableInfo);
            tableInfo.Table = input.NewTableInfo.NewTableName;

            var tableFieldList = ObjectMapper.Map<List<DbTableFieldModel>>(input.TableFieldList);

            var tables = await _databaseManager.GetTableList(input.DbLinkId);
            var isExistTable = tables.Any(x => x.Table == input.NewTableInfo.OldTableName);

            if (isExistTable)
            {
                // Todo 获取不允许修改的表
                var sysTable = new List<string>();
                sysTable.Add("AbpAuditLogs".ToLower());
                var exists = sysTable.Contains(input.NewTableInfo.OldTableName.ToLower());
                if (exists)
                {
                    throw new UserFriendlyException(L("Error"), "系统自带表，不允许修改");
                }

                using (var unitOfWork = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                {
                    await _databaseManager.Update(input.DbLinkId, input.NewTableInfo.OldTableName, tableInfo, tableFieldList);

                    //维护表、字段创建信息
                    await ProcessEntrysField(input.NewTableInfo, input.TableFieldList);

                    await CreateOrUpdateLowCodeModel(input.NewTableInfo);

                    await ProcessingRedundantTableInformation(input.NewTableInfo.OldTableName);

                    unitOfWork.Complete();
                }
            }
            else
            {
                throw new UserFriendlyException(L("Error"), $"表{input.NewTableInfo.OldTableName}不存在");
            }
        }

        /// <summary>
        /// 删除多余的字段信息和模型信息，关联关系
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        private async Task ProcessingRedundantTableInformation(string tableName)
        {
            var entity = await _databaseManager.GetTableList(null);
            entity.Where(x => x.Table == tableName).SingleOrDefault();
            if (entity == null)
            {
                await _fieldManager.BulkDeleteAsync(tableName);
                await _ModelManager.BulkDeleteAsync(tableName);
                await _ModelRelationManager.BulkDeleteAsync(tableName);
            }
        }

        /// <summary>
        /// 添加字段维护表数据
        /// </summary>
        /// <param name="fieldList"></param>
        /// <returns></returns>
        protected virtual async Task CreateField(List<LowCodeFieldEditDto> fieldList)
        {
            var list = ObjectMapper.Map<List<LowCodeField>>(fieldList);

            foreach (var item in list)
            {
                await _fieldManager.CreateAsync(item);
            }
        }

        /// <summary>
        /// 比较字段维护表的数据
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <param name="lowCodeFieldEditsList"></param>
        /// <returns></returns>
        protected async Task ProcessEntrysField(NewTableInfo tableInfo, List<LowCodeFieldEditDto> lowCodeFieldEditsList)
        {
            var oldData = await _fieldManager.QueryAsNoTracking
                .Include(x => x.LowCodeModelRelation)
                .Where(x => x.TableName == tableInfo.OldTableName).ToListAsync();

            var newData = ObjectMapper.Map<List<LowCodeField>>(lowCodeFieldEditsList);

            // 删除项操作
            var deleted = oldData.Except(newData, new DifferentModel());
            foreach (var item in deleted)
            {
                await _fieldManager.Delete(item.Id);
                if (item.LowCodeModelRelation != null)
                {
                    await _ModelRelationManager.Delete(item.LowCodeModelRelation.Id);
                }
            }

            // 更新项
            var updated = newData.Intersect(oldData, new DifferentModel()).ToList();
            foreach (var item in updated)
            {
                var oldItem = oldData.Find(x => x.Id == item.Id);

                ObjectMapper.Map(ObjectMapper.Map<LowCodeFieldEditDto>(item), oldItem);

                if (oldItem.LowCodeModelRelation != null)
                {
                    var relation = await CreateOrUpdateModelRelation(item.LowCodeModelRelation, item.LowCodeModelRelation.Id);
                    oldItem.LowCodeModelRelation = relation;
                }

                await _fieldManager.Update(oldItem);
            }

            // 新增项
            var inserted = newData.Where(x => x.Id == default).ToList();
            foreach (var item in inserted)
            {
                await _fieldManager.Create(item);
            }
        }

        /// <summary>
        /// 添加或修改表关联关系
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<LowCodeModelRelation> CreateOrUpdateModelRelation(LowCodeModelRelation entity, Guid? id)
        {
            var oldModelRelation = _ModelRelationManager.QueryAsNoTracking.FirstOrDefault(x => x.Id == id);

            if (oldModelRelation != null)
            {
                ObjectMapper.Map(ObjectMapper.Map<LowCodeModelRelationEditDto>(entity), oldModelRelation);
                await _ModelRelationManager.Update(oldModelRelation);
                return oldModelRelation;
            }

            await _ModelRelationManager.Create(entity);

            return entity;
        }

        /// <summary>
        /// 添加或修改表名维护表
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        protected async Task CreateOrUpdateLowCodeModel(NewTableInfo tableInfo)
        {
            var newLowCodeModel = new LowCodeModel();
            var entity = _ModelManager.QueryAsNoTracking.FirstOrDefault(x => x.ModelName == tableInfo.OldTableName);
            if (entity == null)
            {
                newLowCodeModel.ModelName = tableInfo.NewTableName;
                newLowCodeModel.ModelDesc = tableInfo.TableDesc;
                await _ModelManager.CreateAsync(newLowCodeModel);
            }
            else
            {
                entity.ModelName = tableInfo.NewTableName;
                entity.ModelDesc = tableInfo.TableDesc;
                await _ModelManager.UpdateAsync(entity);
            }
        }

        /// <summary>
        /// 修改字段时比较
        /// </summary>
        public class DifferentModel : IEqualityComparer<LowCodeField>
        {
            public bool Equals(LowCodeField x, LowCodeField y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode([DisallowNull] LowCodeField obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
    }
}
