// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.UI;
using Yoyo.LowCode.Databases.DomainService;
using Yoyo.LowCode.Databases.Dtos;
using Yoyo.LowCode.Models;

namespace Yoyo.LowCode.Databases
{
    /// <summary>
    /// 数据建模
    /// </summary>
    [AbpAuthorize]
    public class DatabaseAppService : LowCodeSharedAppServiceBase, IDatabaseAppService
    {
        private readonly IDatabaseManager _databaseManager;

        public DatabaseAppService(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
            LocalizationSourceName = LowCodeConsts.LocalizationSourceName;
        }

        public async Task<List<DatabaseTableListOutput>> GetTableList(GetDatabaseTableListInput input)
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
            return ObjectMapper.Map<List<DatabaseTableListOutput>>(ret);
        }

        public async Task<List<DbTableFieldModel>> GetTableFieldList(GetTableFieldListInput input)
        {
            return await _databaseManager.GetFieldList(input.DbLinkId, input.Table);
        }

        public async Task<List<TableFieldsSelectorOutput>> GetTableFieldSelect(GetTableFieldListInput input)
        {
            var fields = await _databaseManager.GetFieldList(input.DbLinkId, input.Table);

            return ObjectMapper.Map<List<TableFieldsSelectorOutput>>(fields);
        }

        public async Task<PagedResultDto<object>> GetTableData(GetTableDataInput input)
        {
            return await _databaseManager.GetData(input.DbLinkId, input.Table, input.MaxResultCount, input.SkipCount);
        }

        public async Task DeleteTable(DeleteTableInput input)
        {
            await _databaseManager.Delete(input.DbLinkId, input.Table);
        }

        public async Task CreateTable(CreateOrUpdateTableInput input)
        {
            var tables = await _databaseManager.GetTableList(input.DbLinkId);

            var isExistTable = tables.Any(x => x.Table == input.TableInfo.NewTable);

            if (isExistTable)
            {
                throw new UserFriendlyException(L("Error"), "表已存在");
            }

            var tableInfo = ObjectMapper.Map<DbTableModel>(input.TableInfo);
            tableInfo.Table = input.TableInfo.NewTable;

            var tableFieldList = ObjectMapper.Map<List<DbTableFieldModel>>(input.TableFieldList);

            await _databaseManager.Create(input.DbLinkId, tableInfo, tableFieldList);
        }

        public async Task UpdateTable(CreateOrUpdateTableInput input)
        {
            var tableInfo = ObjectMapper.Map<DbTableModel>(input.TableInfo);
            tableInfo.Table = input.TableInfo.NewTable;

            var tableFieldList = ObjectMapper.Map<List<DbTableFieldModel>>(input.TableFieldList);

            var tables = await _databaseManager.GetTableList(input.DbLinkId);
            var isExistTable = tables.Any(x => x.Table == input.TableInfo.Table);

            if (isExistTable)
            {
                // Todo 获取不允许修改的表
                var sysTable = new List<string>();
                sysTable.Add("AbpAuditLogs".ToLower());
                var exists = sysTable.Contains(input.TableInfo.Table.ToLower());
                if (exists)
                {
                    throw new UserFriendlyException(L("Error"), "系统自带表，不允许修改");
                }

                await _databaseManager.Update(input.DbLinkId, input.TableInfo.Table, tableInfo, tableFieldList);
            }
            else
            {
                throw new UserFriendlyException(L("Error"), $"表{input.TableInfo.Table}不存在");
            }
        }
    }
}
