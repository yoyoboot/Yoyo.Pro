// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using Yoyo.LowCode.Databases.DomainService;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.LowCode.LowCodeViewModels;
using Yoyo.LowCode.LowCodeViewModels.DomainService;
using Yoyo.LowCode.Renders.Dtos;
using static Yoyo.LowCode.Renders.Dtos.CreateOrUpDate;
using DatabaseType = SqlSugar.DbType;

namespace Yoyo.LowCode.Renders
{
    public class RenderManager : IRenderManager
    {
        #region Public Constructors

        public IAbpSession AbpSession { get; set; }

        private readonly ILowCodeFieldManager _codeFieldManager;

        public RenderManager(
            IDatabaseManager databaseManager,
            IGuidGenerator guidGenerator
,
            ILowCodeFieldManager codeFieldManager)
        {
            _databaseManager = databaseManager;
            _guidGenerator = guidGenerator;
            AbpSession = NullAbpSession.Instance;
            Logger = NullLogger.Instance;
            _codeFieldManager = codeFieldManager;
        }

        #endregion Public Constructors

        #region Private Fields

        public ILogger Logger { get; set; }
        private readonly IDatabaseManager _databaseManager;

        private readonly IGuidGenerator _guidGenerator;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// 添加修改表数据
        /// </summary>
        /// <param name="tableRelationList"></param>
        /// <param name="data"></param>
        /// <param name="afterExecuteSql"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> CreateOrUpdate(List<LowCodeModelRelation> tableRelationList,
    CreateOrUpdateRenderDto data, string afterExecuteSql = "", bool childIsDelete = true)
        {
            //条件参数列表
            var parametersList = new List<SugarParameter>();

            //拼接SQL语句
            var allSql = new StringBuilder();

            //删除SQL语句
            var deleteSql = new StringBuilder();

            //oracle 批量执行sql语句
            var HeadPlSql = "begin\r\n";
            var footerPlSql = "\r\nend;";

            using var db = _databaseManager.GetDatabase(null);

            //判断数据库类型
            var dbType = db.CurrentConnectionConfig.DbType;

            //主表ID
            var insertMainId = _guidGenerator.Create().ToString();
            //以表名分组
            var tableGroup = data.Fields.Where(x => !x.Entity.IsNullOrEmpty())
                .GroupBy(x => x.Entity).ToList();

            var tableGroupList = new List<IGrouping<string, FieldsItem>>();

            var tempMain = tableGroup.FirstOrDefault(x => x.Key.ToLower() == data.MainTable.ToLower());

            //当主表有数据，并且主键有值的情况下，把主表主键的值赋值个主键ID
            var mainprimaryKey = await _codeFieldManager.QueryAsNoTracking
                .Where(x => x.TableName.ToLower() == data.MainTable.ToLower() && x.IsPrimaryKey).FirstOrDefaultAsync();
            foreach (var item in tempMain)
            {
                if (item.Entity.ToLower() == mainprimaryKey.TableName.ToLower() &&
                    item.Name == mainprimaryKey.FieldName &&
                    !item.Value.IsNullOrWhiteSpace())
                {
                    insertMainId = item.Value;
                }
            }

            if (tempMain != null)
            {
                tableGroupList.Add(tempMain);
            }
            var tempNoMain = tableGroup.Where(x => x.Key != data.MainTable).ToList();
            tempNoMain.ForEach(item =>
            {
                tableGroupList.Add(item);
            });

            foreach (var tableItem in tableGroupList)
            {
                var obj = await ProcessExtraFields(data.MainTable, tableRelationList, tableItem.ToList(),
                    tableItem.Key, dbType, parametersList, insertMainId);

                if (obj == null)
                {
                    continue;
                }

                allSql.Append(obj.mainSql).Append(obj.childSql);
            }

            //一对多子表
            var childGroup = data.Grids;
            if (childGroup.Count > 0)
            {
                foreach (var childTable in childGroup)
                {
                    var inputIds = new List<string>();

                    foreach (var item in childTable.Rows)
                    {
                        var obj = await ProcessExtraFields(data.MainTable, tableRelationList, item.Cloumns, childTable.Entity,
                            dbType, parametersList, insertMainId);
                        if (obj == null)
                        {
                            continue;
                        }
                        allSql.Append(obj.mainSql).Append(obj.childSql);
                        inputIds.Add(obj.deleteId);
                    }

                    //获取表字段
                    var childFields = await _codeFieldManager.QueryAsNoTracking
                        .Where(x => x.TableName.ToLower() == childTable.Entity.ToLower()).ToListAsync();
                    // 主键字段
                    var primaryKey = childFields.FirstOrDefault(x => x.IsPrimaryKey)?.FieldName;

                    //表的关联关系
                    var tableRelation = tableRelationList
                        .FirstOrDefault(x => (x.MainModelName == data.MainTable && x.ChildModelName == childTable.Entity));

                    //判断子表中是否含有IsDeleted字段
                    var isDeletedOrUpdate = childFields.Any(x => x.FieldName == "IsDeleted");
                    string childDeleteSql;
                    if (isDeletedOrUpdate)
                    {
                        childDeleteSql =
                            $"select {primaryKey} from {childTable.Entity} where {tableRelation.ChildModelField} = '{insertMainId}' and IsDeleted='false'";
                    }
                    else
                    {
                        childDeleteSql =
                            $"select {primaryKey} from {childTable.Entity} where {tableRelation.ChildModelField} = '{insertMainId}'";
                    }

                    var deleteItemId = new List<string>();
                    var primaryKeyInfo = childFields.FirstOrDefault(x => x.IsPrimaryKey);
                    primaryKeyInfo.DataType = primaryKeyInfo.DataType.ToLower();

                    #region 处理Id Type

                    switch (primaryKeyInfo.DataType)
                    {
                        case "guid":
                        case "uniqueidentifier":
                            await ToGuidConversion<Guid>(childDeleteSql, db, deleteItemId);
                            break;

                        case "int":
                            await ToGuidConversion<int>(childDeleteSql, db, deleteItemId);
                            break;

                        case "long":
                            await ToGuidConversion<long>(childDeleteSql, db, deleteItemId);
                            break;

                        case "raw":
                            await ToGuidConversion<object>(childDeleteSql, db, deleteItemId);
                            break;

                        case "string":
                            await ToGuidConversion<string>(childDeleteSql, db, deleteItemId);
                            break;

                        default:
                            await ToGuidConversion<string>(childDeleteSql, db, deleteItemId);
                            break;
                    }

                    #endregion 处理Id Type

                    var fieldsToSql = new FieldsToSql();
                    foreach (var fieldsItem in childFields)
                    {
                        ComFieldsToSqlHandle(fieldsItem.FieldName, fieldsToSql, dbType.ToString());
                    }

                    var newline = deleteItemId.Except(inputIds);
                    foreach (var item in newline)
                    {
                        if (isDeletedOrUpdate)
                        {
                            deleteSql.AppendFormat(
                                $"update {childTable.Entity} set {fieldsToSql.UpdateData.Trim(',')} where {primaryKey}='{item}';");
                        }
                        else
                        {//比较原有数据和新数据的差别
                            if (childIsDelete)
                            {
                                deleteSql.AppendFormat($"delete from {childTable.Entity} where {primaryKey}='{item}';");
                            }
                        }
                    }
                }
            }
            string finalSql = "";
            try
            {
                db.Ado.Open();

                db.Ado.BeginTran();
                if (dbType == DatabaseType.Oracle)
                {
                    finalSql = HeadPlSql + allSql.ToString() + deleteSql + afterExecuteSql + footerPlSql;

                    await db.Ado.ExecuteCommandAsync(finalSql, parametersList);
                }
                else if (dbType == DatabaseType.MySql || dbType == DatabaseType.SqlServer)
                {
                    finalSql = allSql.ToString() + deleteSql + afterExecuteSql;
                    await db.Ado.ExecuteCommandAsync(finalSql);
                }
                Logger.Info($"LowCode RenderSQL:{finalSql}");
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                throw new UserFriendlyException($"StartSql-------{finalSql}--------EndSql", ex);
            }
            finally
            {
                db.Ado.Close();
            }
            return finalSql;
        }

        /// <summary>
        /// 判断是否是主键外键
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private bool WhetherIsPrimaryForeignKey(List<LowCodeField> fields, string fieldName)
        {
            var isTrue = fields.Where(x => (x.IsPrimaryKey || x.IsForeignkey) && x.FieldName == fieldName).ToList().Count > 0;

            return isTrue;
        }

        /// <summary>
        /// 通过字段生成sql语句
        /// </summary>
        /// <param name="mainTableName">主表名称</param>
        /// <param name="tableRelationList">关联关系集合</param>
        /// <param name="tableItems">字段集合</param>
        /// <param name="tableItemsKey">读取集合的表名称</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="parametersList">Oracle的parameter</param>
        /// <param name="defaultGuid">默认主表Id</param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        private async Task<ProcessSqlDto> ProcessExtraFields(string mainTableName, List<LowCodeModelRelation> tableRelationList,
            List<FieldsItem> tableItems, string tableItemsKey, DatabaseType dbType, List<SugarParameter> parametersList, string defaultGuid)
        {
            var sqlDto = new ProcessSqlDto();
            sqlDto.mainSql = new StringBuilder();
            sqlDto.childSql = new StringBuilder();

            //获取表字段
            var fieldsAsync = await _codeFieldManager.QueryAsNoTracking
                .Where(x => x.TableName.ToLower() == tableItemsKey.ToLower()).ToListAsync();

            var fields = fieldsAsync.GroupBy(x => new { x.TableName, x.FieldName })
               .Select(g => g.First()).AsEnumerable().ToList();
            //通过判断表中是否含有IsDeleted字段来验证是否软删除
            var isDelete = IsAudit(fields);

            if (fields == null)
            {
                throw new UserFriendlyException("错误", $"{tableItemsKey}表不存在");
            }

            // 主键字段
            var primaryKey = fields.FirstOrDefault(x => x.IsPrimaryKey)?.FieldName;
            if (primaryKey == null)
            {
                throw new UserFriendlyException("错误", $"{tableItemsKey}表不存在主键");
            }

            //表的关联关系
            var tableRelation = tableRelationList.FirstOrDefault
            (x => (x.MainModelName.ToLower() == mainTableName.ToLower()
                   && x.ChildModelName.ToLower() == tableItemsKey.ToLower()));

            if (tableRelation == null)
            {
                return null;
            }

            if (tableRelation != null)
            {
                //表字段数据
                var tableFieldsList = fields.ToDictionary(x => x.FieldName);

                //拼接sql内容
                var fieldsToSql = new FieldsToSql();

                //条件语句
                var sqlWhere = "";

                foreach (var item in tableItems)
                {
                    //判断字段是否存在
                    tableFieldsList.TryGetValue(item.Name, out var isAny);
                    if (isAny == null)
                    {
                        continue;
                    }

                    //判断字段类型
                    var dateType = tableFieldsList[item.Name]?.DataType.ToLower();

                    //获取修改where部分语句
                    if (tableFieldsList[item.Name].IsPrimaryKey && !item.Value.IsNullOrWhiteSpace())
                    {
                        if (dbType == DatabaseType.Oracle)
                        {
                            item.Value = GuidConversionRaw(item.Value, false);
                        }
                        sqlDto.deleteId = item.Value;
                        //update的限制条件
                        sqlWhere = $"where {item.Name} = '{item.Value}'";

                        continue;
                    }

                    // 新增、修改时剔除外键字段
                    // 判断字段是否是常用字段，是否追加成SQL
                    //剔除为空的字段

                    if (WhetherIsPrimaryForeignKey(fields, item.Name)
                        || item.Name.IsNullOrEmpty()
                        || item.Value.IsNullOrEmpty()
                        || ExcludeFields(item.Name))
                    {
                        continue;
                    }

                    //判断是否逻辑删除
                    if (isDelete)
                    {
                        //循环插入表中的字段
                        foreach (var fieldsItem in fields)
                        {
                            ComFieldsToSqlHandle(fieldsItem.FieldName, fieldsToSql, dbType.ToString());
                        }

                        //插入一次后改为False，防止多次重复插入
                        isDelete = false;
                    }
                    //处理Oracle超长字符
                    ClobOrBlobHandle(item, dateType, parametersList);

                    //拼接字段成Sql语句
                    FieldsToSqlHandle(item, dateType, fieldsToSql, dbType.ToString());
                }

                if (fieldsToSql.UpdateData.IsNullOrEmpty() || fieldsToSql.AddCoulum.IsNullOrEmpty())
                {
                    return null;
                }

                //通过判断数据是否有判断条件来进行添加或修改
                if (!string.IsNullOrEmpty(sqlWhere))
                {
                    //去除SQL语句最后的逗号
                    fieldsToSql.UpdateData =
                        fieldsToSql.UpdateData.Remove(
                            fieldsToSql.UpdateData.LastIndexOf(",", StringComparison.Ordinal), 1);
                    //修改的sql语句
                    sqlDto.mainSql.AppendFormat("update {0} set {1} {2};", tableItemsKey, fieldsToSql.UpdateData, sqlWhere);
                    if (dbType == DatabaseType.Oracle)
                    {
                        defaultGuid = GuidConversionRaw(defaultGuid, false);
                    }
                }
                else
                {
                    //拼接sql
                    if (tableItemsKey == mainTableName) // 主表
                    {
                        if (dbType == DatabaseType.Oracle)
                        {
                            defaultGuid = GuidConversionRaw(defaultGuid, true);
                        }
                        sqlDto.mainSql.AppendFormat("insert into {0} ({3},{1}) values('{4}',{2});",
                            mainTableName, fieldsToSql.AddCoulum.Trim(','),
                            fieldsToSql.AddValue.Trim(','),
                            primaryKey, defaultGuid);
                    }
                    else // 子表
                    {
                        var childId = _guidGenerator.Create().ToString();
                        if (dbType == DatabaseType.Oracle)
                        {
                            childId = GuidConversionRaw(childId, true);
                        }
                        sqlDto.childSql.AppendFormat("insert into {0} ({3},{1},{5}) values('{4}',{2},'{6}');",
                            tableItemsKey, fieldsToSql.AddCoulum.Trim(','), fieldsToSql.AddValue.Trim(','),
                            primaryKey, childId, tableRelation.ChildModelField, defaultGuid);
                    }
                }

                return sqlDto;
            }
            return null;
        }

        /// <summary>
        /// 处理主键Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="db"></param>
        /// <param name="deleteItemId"></param>
        /// <returns></returns>
        private async Task ToGuidConversion<T>(string sql, SqlSugarClient db, List<string> deleteItemId)
        {
            if (typeof(T) == typeof(object))
            {
                var tempDeleteItemId = await db.Ado.SqlQueryAsync<object>(sql);
                foreach (var item in tempDeleteItemId)
                {
                    foreach (var items in (IDictionary<string, object>)item)
                    {
                        var str = BitConverter.ToString((byte[])items.Value);
                        str = str.Replace("-", string.Empty);
                        deleteItemId.Add(str);
                    }
                }
            }
            else
            {
                var tempDeleteItemId = await db.Ado.SqlQueryAsync<T>(sql);
                foreach (var item in tempDeleteItemId)
                {
                    deleteItemId.Add(Convert.ToString(item));
                }
            }
        }

        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <param name="dbTableRelations"></param>
        /// <param name="mainTableName"></param>
        /// <param name="id"></param>
        /// <param name="afterExecuteSql"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task Delete(List<LowCodeModelRelation> dbTableRelations, string mainTableName, string id, string afterExecuteSql = "")
        {
            if (dbTableRelations != null)
            {
                using var db = _databaseManager.GetDatabase(null);

                var mainTableRelations = dbTableRelations.ToList();

                if (db.CurrentConnectionConfig.DbType == DatabaseType.Oracle)
                {
                    id = GuidConversionRaw(id, false);
                }

                foreach (var item in mainTableRelations)
                {
                    string querySql;
                    var fields = await _codeFieldManager.QueryAsNoTracking
                        .Where(x => x.TableName.ToLower() == item.ChildModelName.ToLower()).ToListAsync();
                    if (fields == null)
                    {
                        throw new UserFriendlyException("错误", $"{item.ChildModelName}表不存在");
                    }

                    var primaryKeyName = fields.FirstOrDefault(x => x.IsPrimaryKey)?.FieldName;
                    var isDeletedOrUpdate = fields.Any(x => x.FieldName == "IsDeleted");
                    //拼接sql内容
                    var fieldsToSql = new FieldsToSql();
                    foreach (var fieldsItem in fields)
                    {
                        ComFieldsToSqlHandle(fieldsItem.FieldName, fieldsToSql, db.CurrentConnectionConfig.DbType.ToString());
                    }

                    if (isDeletedOrUpdate)
                    {
                        querySql =
                            $"update {item.ChildModelName} set {fieldsToSql.DeleteData.Trim(',')} where {item.ChildModelField} = '{id}'";
                    }
                    else
                    {
                        querySql = $"delete from {item.ChildModelName} where {item.ChildModelField} = '{id}'";
                    }

                    try
                    {
                        db.Ado.Open();

                        db.Ado.BeginTran();

                        await db.Ado.ExecuteCommandAsync(querySql + afterExecuteSql);

                        db.Ado.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        db.Ado.RollbackTran();
                        throw new UserFriendlyException($"DeleteSqlStart------{querySql + afterExecuteSql}-----DeleteSqlEnd", ex);
                    }
                    finally
                    {
                        db.Ado.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 通过Id获取表数据
        /// </summary>
        /// <param name="dbTableRelations"></param>
        /// <param name="mainTable"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<string> GetById(List<LowCodeModelRelation> dbTableRelations, string mainTable, string id, string mainTableSelectSql = "")
        {
            //存储所有sql语句
            var allSql = new StringBuilder();
            using var db = _databaseManager.GetDatabase(null);
            if (db.CurrentConnectionConfig.DbType == DatabaseType.Oracle)
            {
                id = GuidConversionRaw(id, false);
            }
            else if (db.CurrentConnectionConfig.DbType == DatabaseType.MySql)
            {
                foreach (var item in dbTableRelations)
                {
                    item.ChildModelName.ToLower();
                    item.MainModelName.ToLower();
                    mainTable.ToLower();
                }
            }
            var mainTableRelations = dbTableRelations.ToList();

            var mainTableRelation = dbTableRelations.FirstOrDefault(x => x.ChildModelName == mainTable);

            var isFieldDeletion = false;
            try
            {
                if (mainTableRelation != null)
                {
                    var result = new JObject();
                    foreach (var item in mainTableRelations)
                    {
                        if (item.MainModelField.IsNullOrEmpty() ||
                            item.MainModelName.IsNullOrEmpty() ||
                            item.ChildModelField.IsNullOrEmpty() ||
                            item.ChildModelName.IsNullOrEmpty())
                        {
                            isFieldDeletion = true;
                        }
                        string querySql;
                        var fields = await _codeFieldManager.QueryAsNoTracking
                            .Where(x => x.TableName.ToLower() == item.ChildModelName.ToLower()).ToListAsync();
                        if (fields == null)
                        {
                            throw new UserFriendlyException("错误", $"{item.ChildModelName}表不存在");
                        }

                        // var primaryKeyName = fields.FirstOrDefault(x => x.IsPrimaryKey)?.FieldName;
                        var isDeletedOrUpdate = fields.Any(x => x.FieldName == "IsDeleted");
                        if (isDeletedOrUpdate)
                        {
                            querySql =
                                $"select * from {item.ChildModelName} where {item.ChildModelField} = '{id}' and IsDeleted='false'";
                        }
                        else
                        {
                            querySql = $"select * from {item.ChildModelName} where {item.ChildModelField} = '{id}'";
                        }

                        if (!mainTableSelectSql.IsNullOrEmpty())
                        {
                            if (item.ChildModelName.ToLower() == mainTable.ToLower())
                            {
                                querySql = mainTableSelectSql;
                            }
                        }
                        allSql.Append(querySql);

                        var entityJob = new JObject();
                        if (item.ObjectRelation == ObjectRelationEnum.OneOnMany)
                        {
                            var isExitEntity = await _codeFieldManager.QueryAsNoTracking
                                .Where(x => x.TableName.ToLower() == item.ChildModelName.ToLower() &&
                                            x.FieldName == "CreationTime").FirstOrDefaultAsync();
                            if (isExitEntity != null)
                            {
                                querySql += " order by CreationTime";
                            }
                            var data = await db.Ado.SqlQueryAsync<object>(querySql);
                            if (data == null)
                            {
                                continue;
                            }
                            var entity = JsonConvert.SerializeObject(data);
                            entityJob.Add(item.ChildModelName, JArray.Parse(entity));
                        }
                        else
                        {
                            var data = await db.Ado.SqlQuerySingleAsync<object>(querySql);
                            if (data == null)
                            {
                                continue;
                            }
                            var entity = JsonConvert.SerializeObject(data);
                            entityJob.Add(item.ChildModelName, JObject.Parse(entity));
                        }

                        result.Add(entityJob.First);
                    }

                    return JsonConvert.SerializeObject(result);
                }
            }
            catch (Exception ex)
            {
                if (isFieldDeletion)
                {
                    throw new UserFriendlyException("错误", $"请完善关联关系配置！！！");
                }
                else
                {
                    throw new UserFriendlyException($"SelectSqlStart------{allSql}-----SelectSqlEnd", ex);
                }
            }
            return null;
        }

        public async Task<PagedResultDto<object>> GetPaged(List<LowCodeModelRelation> dbTableRelations,
            string mainTableName,
            int maxResultCount, int skipCount,
            bool isPaged = true)
        {
            using var db = _databaseManager.GetDatabase(null);

            if (mainTableName != null)
            {
                var fields = await _codeFieldManager.QueryAsNoTracking
                    .Where(x => x.TableName.ToLower() == mainTableName.ToLower()).ToListAsync();
                var isDeletedOrUpdate = fields.Any(x => x.FieldName == "IsDeleted");
                string querySql;
                if (isDeletedOrUpdate)
                {
                    querySql = $"select * from {mainTableName} where IsDeleted = 'false'";
                }
                else
                {
                    querySql = $"select * from {mainTableName}";
                }

                if (isPaged)
                {
                    RefAsync<int> totalCount = 0;

                    if (skipCount == 0)
                    {
                        skipCount = 1;
                    }
                    else
                    {
                        skipCount /= maxResultCount;

                        skipCount++;
                    }

                    var ret = await db.SqlQueryable<object>(string.Format(querySql))
                        .ToPageListAsync(skipCount, maxResultCount, totalCount);
                    return new PagedResultDto<object>(totalCount, ret);
                }
                else
                {
                    var ret = await db.SqlQueryable<object>(string.Format(querySql))
                        .ToListAsync();
                    return new PagedResultDto<object>(ret.Count, ret);
                }
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     处理字段成SQL
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dataType"></param>
        /// <param name="tableInfo"></param>
        private void FieldsToSqlHandle(FieldsItem item, string dataType, FieldsToSql tableInfo, string DbType)
        {
            switch (dataType.Trim())
            {
                case "int":
                case "bigint":
                case "binary":
                case "image":
                case "varbinary(max)":
                case "rowversion":
                case "varbinary":
                case "datetimeoffset":
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                case "uniqueidentifier":
                case "float":
                case "guid":
                case "raw":
                    tableInfo.AddCoulum += $"{item.Name},";
                    tableInfo.AddValue += item.Value.IsNullOrEmpty() ? "NULL," : $"'{item.Value}',";
                    tableInfo.UpdateData += item.Value.IsNullOrEmpty()
                         ? $"{item.Name} = NULL,"
                         : $"{item.Name} = '{item.Value}',";
                    break;

                case "bit":
                    tableInfo.AddCoulum += $"{item.Name},";
                    if (DbType == DatabaseType.MySql.ToString())
                    {
                        tableInfo.AddValue += item.Value.IsNullOrEmpty() ? "NULL," : $"{item.Value.ToLower()},";
                        tableInfo.UpdateData += item.Value == null
                             ? $"{item.Name} = NULL,"
                             : $"{item.Name} = {item.Value.ToLower()},";
                    }
                    else
                    {
                        tableInfo.AddValue += item.Value.IsNullOrEmpty() ? "NULL," : $"{(item.Value.ToLower() == "true" ? "1" : "0")},";
                        tableInfo.UpdateData += item.Value.IsNullOrEmpty()
                             ? $"{item.Name} = NULL,"
                             : $"{item.Name} = {(item.Value.ToLower() == "true" ? "1" : "0")},";
                    }
                    break;

                case "char":
                case "nchar":
                case "text":
                case "ntext":
                case "varchar":
                case "nvarchar":
                case "longtext":
                case "maxnvarchar":
                case "xml":
                    if (item.Value != null && item.Value.Contains('\''))
                    {
                        item.Value = item.Value.Replace("'", "''");
                    }
                    tableInfo.AddCoulum += $"{item.Name},";
                    tableInfo.AddValue += item.Value.IsNullOrEmpty() ? "NULL," : $"N'{item.Value}',";
                    tableInfo.UpdateData += item.Value.IsNullOrEmpty()
                             ? $"{item.Name} = NULL,"
                             : $"{item.Name} = N'{item.Value}', ";
                    break;

                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                case "timestamp(7)":
                    tableInfo.AddCoulum += $"{item.Name},";
                    //当时间控件不传值时赋值NULL，否则会超出日期类型限制
                    if (DbType == DatabaseType.Oracle.ToString())
                    {
                        var times = $"{Convert.ToDateTime(item.Value):yyyy-MM-dd HH:mm:ss}";
                        tableInfo.AddValue += item.Value.IsNullOrEmpty() ? "NULL," : "to_date('" + times + "', 'yyyy-MM-dd HH24:MI:SS'),";
                        tableInfo.UpdateData += item.Value.IsNullOrEmpty() ? $"{item.Name} = NULL," : $"{item.Name} = " + "to_date('" + times + "', 'yyyy-MM-dd HH24:MI:SS'),";
                        break;
                    }
                    tableInfo.AddValue += item.Value.IsNullOrEmpty()
                        ? "NULL,"
                        : $"'{Convert.ToDateTime(item.Value):yyyy-MM-dd HH:mm:ss}',";
                    tableInfo.UpdateData += item.Value.IsNullOrEmpty()
                        ? $"{item.Name} = NULL,"
                        : $"{item.Name} = '{Convert.ToDateTime(item.Value):yyyy-MM-dd HH:mm:ss}',";
                    break;

                case "clob":
                case "blob":
                    tableInfo.AddCoulum += $"{item.Name},";
                    tableInfo.AddValue += $":{item.Entity}{item.Name},";
                    tableInfo.UpdateData += $"{item.Name} = :{item.Entity}{item.Name}, ";
                    break;

                default:
                    tableInfo.AddCoulum += $"{item.Name},";
                    tableInfo.AddValue += $"'{item.Value}',";
                    tableInfo.UpdateData += $"{item.Name} = '{item.Value}', ";
                    break;
            }
        }

        /// <summary>
        ///     处理常用字段成SQL
        /// </summary>
        /// <param name="field"></param>
        /// <param name="tableInfo"></param>
        private void ComFieldsToSqlHandle(string field, FieldsToSql tableInfo, string DbType)
        {
            switch (field)
            {
                case "CreationTime":
                    tableInfo.AddCoulum += "CreationTime,";
                    tableInfo.AddValue += $"'{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff}',";
                    break;

                case "CreatorUserId":
                    tableInfo.AddCoulum += "CreatorUserId,";
                    tableInfo.AddValue += $"'{AbpSession.UserId}',";
                    break;

                case "CreatorUserName":
                    tableInfo.AddCoulum += "CreatorUserName,";
                    tableInfo.AddValue += $"'{AbpSession.GetUserName()}',";
                    break;

                case "LastModificationTime":
                    tableInfo.UpdateData +=
                        $"LastModificationTime = '{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff}',";
                    break;

                case "LastModifierUserId":
                    tableInfo.UpdateData += $"LastModifierUserId = '{AbpSession.UserId}',";
                    break;

                case "LastModifierUserName":
                    tableInfo.UpdateData += $"LastModifierUserName = '{AbpSession.GetUserName()}',";
                    break;

                case "IsDeleted":
                    tableInfo.AddCoulum += "IsDeleted,";
                    if (DbType == DatabaseType.MySql.ToString())
                    {
                        tableInfo.AddValue += "false,";
                        tableInfo.DeleteData += "IsDeleted=true,";
                    }
                    else
                    {
                        tableInfo.AddValue += "'false',";
                        tableInfo.DeleteData += "IsDeleted='true',";
                    }
                    break;

                case "DeleterUserId":
                    tableInfo.DeleteData += $"DeleterUserId='{AbpSession.UserId}',";
                    break;

                case "DeletionTime":
                    tableInfo.DeleteData += $"DeletionTime='{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff}',";
                    break;

                case "DeleterUserName":
                    tableInfo.DeleteData += $"DeleterUserName='{AbpSession.GetUserName()}',";
                    break;
            }
        }

        /// <summary>
        ///     判断是否是常用字段
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool ExcludeFields(string field)
        {
            switch (field)
            {
                case "CreationTime":
                case "CreatorUserId":
                case "CreatorUserName":
                case "LastModificationTime":
                case "LastModifierUserId":
                case "LastModifierUserName":
                case "IsDeleted":
                case "DeleterUserId":
                case "DeletionTime":
                case "DeleterUserName":
                    return true;

                default:
                    return false;
            }
        }

        private bool IsAudit(List<LowCodeField> fields)
        {
            //通过判断表中是否含有IsDeleted字段来验证是否软删除
            var isDelete = fields.Any(x => x.FieldName == "IsDeleted" ||
                                                        x.FieldName == "CreationTime" ||
                                                        x.FieldName == "CreatorUserId" ||
                                                        x.FieldName == "CreatorUserName" ||
                                                        x.FieldName == "LastModifierUserId" ||
                                                        x.FieldName == "DeleterUserId" ||
                                                        x.FieldName == "DeletionTime" ||
                                                        x.FieldName == "DeleterUserName" ||
                                                        x.FieldName == "LastModifierUserName" ||
                                                        x.FieldName == "LastModificationTime");

            return isDelete;
        }

        #region Guid与Raw转换

        /// <summary>
        /// </summary>
        /// <param name="guid">文本</param>
        /// <param name="isConversion">true表示Guid转Raw，false反之</param>
        /// <returns></returns>
        private string GuidConversionRaw(string guid, bool isConversion)
        {
            if (isConversion)
            {
                return BitConverter.ToString(new Guid(guid).ToByteArray()).Replace("-", "");
            }
            else
            {
                string str;
                if (!Guid.TryParse(guid, out Guid guidStr))
                {
                    var decBytes = Convert.FromBase64String(guid);
                    str = BitConverter.ToString(decBytes).Replace("-", string.Empty);
                }
                else
                {
                    str = BitConverter.ToString(new Guid(guid).ToByteArray()).Replace("-", "");
                }
                return str;
            }
        }

        #endregion Guid与Raw转换

        #region Oracle字段处理

        /// <summary>
        ///处理Clob和Blob
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dateType">字段类型</param>
        /// <param name="paramtersList"></param>
        private void ClobOrBlobHandle(FieldsItem item, string dateType, List<SugarParameter> paramtersList)
        {
            if (dateType == "clob")
            {
                var paramters = new SugarParameter(":" + item.Entity + item.Name, item.Value, System.Data.DbType.Binary);
                paramters.IsClob = true;
                paramtersList.Add(paramters);
            }
            //处理二进制,数据量不能超过4000
            if (dateType == "blob")
            {
                if (item.Value != null)
                {
                    var paramters = new SugarParameter(":" + item.Entity + item.Name, Encoding.Unicode.GetBytes(item.Value), System.Data.DbType.Binary);
                    paramters.IsNvarchar2 = true;
                    paramters.Size = -1;
                    paramtersList.Add(paramters);
                }
            }
            //if (dateType == "byte[]")
            //{
            //    字节类型数据转换
            //    var newData = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
            //    foreach (var newItem in (IDictionary<string, object>)data)
            //    {
            //        if (newItem.Value != null && newItem.Key != primaryKeyName && newItem.Value.GetType().FullName == "System.Byte[]")
            //        {
            //            string str = Encoding.Unicode.GetString((byte[])newItem.Value);
            //            newData.Add(newItem.Key, str);
            //        }
            //        else
            //        {
            //            newData.Add(newItem.Key, newItem.Value);
            //        }
            //    }
            //}
        }

        #endregion Oracle字段处理

        #endregion Private Methods
    }
}
