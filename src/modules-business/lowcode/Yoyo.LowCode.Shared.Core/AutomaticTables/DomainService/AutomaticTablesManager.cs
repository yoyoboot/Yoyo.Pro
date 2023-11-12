// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using SqlSugar;
using Yoyo.LowCode.AutomaticTables.Dtos;
using Yoyo.LowCode.AutomaticTables.Dtos.Enums;
using Yoyo.LowCode.Databases.DomainService;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.LowCode.LowCodeViewModels;
using Yoyo.LowCode.LowCodeViewModels.DomainService;
using Yoyo.LowCode.Models;
using static Yoyo.LowCode.AutomaticTables.Dtos.CreateOrUpdateTableData;

namespace Yoyo.LowCode.AutomaticTables.DomainService
{
    public class AutomaticTablesManager : IAutomaticTablesManager
    {
        private readonly IDatabaseManager _databaseManager;
        private readonly ILowCodeModelRelationManager _lowCodeModelRelation;
        private readonly ILowCodeFieldManager _lowCodeFieldManager;

        public AutomaticTablesManager(IDatabaseManager databaseManager, ILowCodeModelRelationManager lowCodeModelRelation, ILowCodeFieldManager lowCodeFieldManager)
        {
            _databaseManager = databaseManager;
            _lowCodeModelRelation = lowCodeModelRelation;
            _lowCodeFieldManager = lowCodeFieldManager;
        }

        /// <summary>
        /// 生成建表语句和关联关系集合
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pageId"></param>
        /// <param name="isAuto"></param>
        /// <returns></returns>
        public async Task<TrasferimentoDto> CreateSqlStatement(CreateOrUpdateTableDto input, Guid pageId, bool isAutoRelation)
        {
            //主表主键
            var mainTablePrimary = "";

            //关联关系集合
            var relationList = new List<LowCodeModelRelation>();

            //表字段集合
            var fieldList = new List<LowCodeField>();

            //创建表sql
            var sql = new StringBuilder();

            //读取数据库的所有表
            var tablelist = await _databaseManager.GetTableList(null);

            var oldRelationList = new List<LowCodeModelRelation>();

            await MaintainOldTables(input.MainTable, input.MainTable);

            //根据表名分组
            var groupMainTableEntity = input.Fields.Where(x => x.IsCustomFields == false).GroupBy(x => x.Entity).ToList();

            foreach (var item in groupMainTableEntity)
            {
                //处理字段，生成建表语句
                //生成关联关系
                await ProcessSqlOrList(pageId, input.MainTable, item.ToList(), item.Key,
                    tablelist, mainTablePrimary, sql, relationList, fieldList, ObjectRelationEnum.OneOnOne, isAutoRelation);

                await GetOldRelation(pageId, oldRelationList, input.MainTable, item.Key);
            }

            foreach (var gridsItem in input.Grids)
            {
                await ProcessSqlOrList(pageId, input.MainTable, gridsItem.Cloumns.Where(x => x.IsCustomFields == false).ToList(),
                    gridsItem.Entity, tablelist, mainTablePrimary, sql, relationList, fieldList, ObjectRelationEnum.OneOnMany, isAutoRelation);

                await GetOldRelation(pageId, oldRelationList, input.MainTable, gridsItem.Entity);
            }
            var unionRelations = relationList.Union(oldRelationList, new LowCodeModelRelationComparer())
                 .Distinct(new LowCodeModelRelationComparer()).ToList();

            var dto = new TrasferimentoDto();
            dto.BaseCustomPageId = pageId;
            switch (JudgeDatabase())
            {
                case 0:
                    dto.Sql = sql.ToString();
                    break;

                case 1:
                    dto.Sql = sql.ToString().Replace("Uniqueidentifier", "RAW(16)")
                        .Replace("Nvarchar(MAX)", "CLOB")
                        .Replace("Nvarchar", "NVARCHAR2")
                        .Replace("Datetime2", "TIMESTAMP(7)")
                        .Replace("Int", "NUMBER(10,0)")
                        .Replace("Bit", "NUMBER(10,0)");
                    break;

                case 2:
                    dto.Sql = sql.ToString().Replace("Uniqueidentifier", "Char(36)")
                        .Replace("Nvarchar(MAX)", "Longtext")
                        .Replace("Nvarchar", "Varchar")
                        .Replace("Datetime2", "Datetime(6)");
                    break;

                default:
                    break;
            }
            dto.LowCodeFields = fieldList;
            if (isAutoRelation)
            {
                dto.LowCodeModelRelations = unionRelations;
            }
            return dto;
        }

        /// <summary>
        /// 获取老的关联关系
        /// </summary>
        /// <param name="oldRelationList"></param>
        /// <param name="mainTable"></param>
        /// <param name="childTable"></param>
        /// <returns></returns>
        private async Task GetOldRelation(Guid pageId, List<LowCodeModelRelation> oldRelationList, string mainTable, string childTable)
        {
            var relat = await _lowCodeModelRelation.QueryAsNoTracking.
                WhereIf(pageId != Guid.Empty, x => x.BaseCustomPageId == pageId).
                WhereIf(pageId == Guid.Empty, x => x.BaseCustomPageId == pageId || x.BaseCustomPageId == null)
                .Where(x => x.MainModelName.ToLower() == mainTable.ToLower()
                && x.ChildModelName.ToLower() == childTable.ToLower()).FirstOrDefaultAsync();
            if (relat != null)
            {
                oldRelationList.Add(relat);
            }
        }

        /// <summary>
        /// 生成表和关联关系
        /// </summary>
        /// <param name="trasferimentoDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task FinalExecution(TrasferimentoDto trasferimentoDto)
        {
            if (trasferimentoDto.BaseCustomPageId.HasValue)
            {
                foreach (var item in trasferimentoDto.LowCodeModelRelations)
                {
                    item.BaseCustomPageId = trasferimentoDto.BaseCustomPageId;
                }
            }
            try
            {
                using var db = _databaseManager.GetDatabase(null);
                if (!trasferimentoDto.Sql.ToString().IsNullOrWhiteSpace())
                {
                    //创建表
                    await db.Ado.ExecuteCommandAsync(trasferimentoDto.Sql.ToString());
                }
                //添加字段维护表数据
                await _lowCodeFieldManager.BlukOperateFieldAsync(trasferimentoDto.LowCodeFields);

                //添加关联关系
                await _lowCodeModelRelation.BlukOperateRelationAsync(trasferimentoDto.LowCodeModelRelations);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"Executed Sql-------{trasferimentoDto.Sql}--------End", ex);
            }
        }

        /// <summary>
        /// 处理SQL语句和List集合
        /// </summary>
        /// <param name="pageId">页面Id</param>
        /// <param name="mainTableName">主表名称</param>
        /// <param name="fieldsItem">字段集合</param>
        /// <param name="tableName">当前表名称</param>
        /// <param name="tableFieldlist">表字段信息</param>
        /// <param name="mainTablePrimary">主表主键</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="relationList">关联关系集合</param>
        /// <param name="fieldList">字段集合</param>
        /// <param name="relationEnum">关联关系</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task ProcessSqlOrList(Guid pageId, string mainTableName, List<TableFieldsItem> fieldsItem,
            string tableName, List<DbTableModel> tableFieldlist,
            string mainTablePrimary, StringBuilder sql,
            List<LowCodeModelRelation> relationList,
            List<LowCodeField> fieldList,
            ObjectRelationEnum relationEnum,
            bool isAutoRelation)
        {
            try
            {
                //维护旧表字段
                await MaintainOldTables(tableName, mainTableName);
                if (CheckStringChineseUn(tableName))
                {
                    throw new UserFriendlyException("表名中不能含有汉字！！！");
                }
                var tableSql = new StringBuilder();
                tableSql.Append($"CREATE TABLE {tableName} \r\n");
                tableSql.Append("(\r\n");
                //判断表是否存在
                var isExis = tableFieldlist.Find(x => x.Table.ToLower() == tableName.ToLower());
                //不存在走新增
                if (isExis == null)
                {
                    ProcessTableNotExist(fieldsItem, fieldList, mainTableName,
                        mainTablePrimary.IsNullOrWhiteSpace() == true ? "Id" : mainTablePrimary,
                        relationEnum, tableName, relationList, tableSql, pageId, isAutoRelation);
                    sql.Append(tableSql);
                }
                //存在走修改
                else
                {
                    //获取字段信息
                    var tableFieldItem = await _databaseManager.GetFieldList(null, tableName);

                    foreach (var obj in tableFieldItem)
                    {
                        if (obj.PrimaryKey == 1 && tableName == mainTableName)
                        {
                            mainTablePrimary = obj.Field;
                        }
                    }

                    StringBuilder temSql = new StringBuilder();
                    //添加新数据
                    await TableExistProcessField(fieldsItem, tableName, fieldList, temSql);

                    await TableExistProcessRelation(relationList, pageId, relationEnum, mainTableName, tableName, isAutoRelation);

                    sql.Append(temSql);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// 在表不存在的情况下，处理字段与关联关系
        /// </summary>
        /// <param name="fieldsItem"></param>
        /// <param name="fieldList">字段维护表数据集合</param>
        /// <param name="tableName">当前循环的表名</param>
        /// <param name="mainTable">主表名</param>
        /// <param name="mainTableField">主表主键</param>
        /// <param name="relationEnum">关联关系</param>
        /// <param name="entityName">子表</param>
        /// <param name="relationList">子表外键</param>
        /// <param name="tableSql">建表sql语句</param>
        private void ProcessTableNotExist(List<TableFieldsItem> fieldsItem, List<LowCodeField> fieldList,
            string mainTable, string mainTableField,
            ObjectRelationEnum relationEnum, string childTableName,
            List<LowCodeModelRelation> relationList, StringBuilder tableSql, Guid pageId, bool isAutoRelation)
        {
            tableSql.Append($"Id Uniqueidentifier NOT NULL primary key,\r\n");
            foreach (var item in fieldsItem)
            {
                if (CheckStringChineseUn(item.Name))
                {
                    throw new UserFriendlyException("字段名中不能含有汉字！！！");
                }
                var fieldInfo = ProcessFieldType(item.ComponentType);
                tableSql.Append($"{item.Name}{fieldInfo.DataType},\r\n");
                //循环添加字段维护表数据
                CreateLowcodeField(fieldList, item.Entity, item.Name, item.Name, fieldInfo.FieldType, fieldInfo.DataLength, true, false, false);
            }

            //添加表主键字段信息
            CreateLowcodeField(fieldList, childTableName, "Id", "主键", "Uniqueidentifier", null, false, true, false);
            //外键字段
            var foreignKeyField = $"{mainTable}Id";
            //获取关联关系

            if (mainTable != childTableName)
            {
                var relation = HandleRelations(pageId, mainTable, childTableName, relationEnum, mainTableField, foreignKeyField, isAutoRelation);
                if (relation != null)
                {
                    relationList.Add(relation);
                }
                tableSql.Append($"{foreignKeyField} Uniqueidentifier NULL,");
                //添加表外键字段信息
                CreateLowcodeField(fieldList, childTableName, foreignKeyField, "外键", "Uniqueidentifier", null, true, false, true);
            }
            else
            {
                //获取关联关系
                var relation = HandleRelations(pageId, mainTable, mainTable, relationEnum, mainTableField, mainTableField, isAutoRelation);
                relationList.Add(relation);
            }
            tableSql =
                    tableSql.Remove(
                        tableSql.ToString().LastIndexOf(",", StringComparison.Ordinal), 1);
            tableSql.Append("\r\n");
            tableSql.Append(");\r\n\r\n");
        }

        /// <summary>
        /// 处理关联关系
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="mainTableName"></param>
        /// <param name="childTableName"></param>
        /// <returns></returns>
        private LowCodeModelRelation HandleRelations(Guid pageId, string mainTableName, string childTableName,
            ObjectRelationEnum objectRelationEnum, string mainTableField, string foreignKeyField, bool isAutoRelation)
        {
            if (!isAutoRelation)
            {
                return null;
            }
            var relation = new LowCodeModelRelation();
            relation.BaseCustomPageId = pageId;
            relation.ObjectRelation = objectRelationEnum;
            relation.MainModelName = mainTableName;
            relation.MainModelField = mainTableField;
            relation.ChildModelName = childTableName;
            relation.ChildModelField = foreignKeyField;

            return relation;
        }

        /// <summary>
        /// 修改关联关系
        /// </summary>
        /// <param name="relationList">关联关系集合</param>
        /// <param name="pageId">页面Id</param>
        /// <param name="relationEnum">关联关系</param>
        /// <param name="mainTable">主表</param>
        /// <param name="foreignTable">子表</param>
        /// <param name="mainFieldEntity">主表字段集合</param>
        /// <param name="childFieldEntity">子表字段集合</param>
        /// <returns></returns>
        private async Task TableExistProcessRelation(List<LowCodeModelRelation> relationList, Guid pageId,
            ObjectRelationEnum relationEnum, string mainTable, string foreignTable,
            bool isAutoRelation)
        {
            if (!isAutoRelation)
            {
                return;
            }
            //在存在PageId时获取相关的，否则获取所有关联关系
            var allRelationList = await _lowCodeModelRelation.QueryAsNoTracking.
                WhereIf(pageId != Guid.Empty, x => x.BaseCustomPageId == pageId).
                WhereIf(pageId == Guid.Empty, x => x.BaseCustomPageId == pageId || x.BaseCustomPageId == null).ToListAsync();
            //获取表的字段信息
            var allFieldList = await _lowCodeFieldManager.QueryAsNoTracking.Where(x => x.TableName == foreignTable).ToListAsync();

            var mainTableField = _lowCodeFieldManager.QueryAsNoTracking.
                FirstOrDefault(x => x.IsPrimaryKey && x.TableName == foreignTable)?.FieldName;

            var foreignkey = allFieldList.FirstOrDefault(x => x.IsForeignkey)?.FieldName;

            if (foreignkey.IsNullOrEmpty())
            {
                foreignkey = allFieldList.FirstOrDefault(x => x.FieldName.StartsWith(mainTable)
                && x.FieldName.EndsWith("Id"))?.FieldName;
            }
            var defaultRelation = allRelationList.Find(x => x.MainModelName == mainTable && x.ChildModelName == mainTable);
            if (defaultRelation == null)
            {
                var relation = new LowCodeModelRelation();
                relation.BaseCustomPageId = pageId;
                relation.ObjectRelation = relationEnum;
                relation.MainModelName = mainTable;
                relation.MainModelField = mainTableField;
                relation.ChildModelName = mainTable;
                relation.ChildModelField = mainTableField;
                if (relationList.Find(x => x.MainModelName == mainTable && x.ChildModelName == mainTable) == null)
                {
                    relationList.Add(relation);
                }
            }
            var relationEntity = allRelationList.Find(
                x => x.MainModelName == mainTable && x.ChildModelName == foreignTable);

            if (relationEntity != null && relationEntity.ChildModelField.IsNullOrEmpty())
            {
                relationEntity.ChildModelField = foreignkey;
                await _lowCodeModelRelation.Update(relationEntity);
                relationList.Add(relationEntity);
            }

            if (relationEntity == null && foreignTable != mainTable)
            {
                var relation = new LowCodeModelRelation();
                relation.BaseCustomPageId = pageId;
                relation.ObjectRelation = relationEnum;
                relation.MainModelName = mainTable;
                relation.MainModelField = mainTableField;
                relation.ChildModelName = foreignTable;
                relation.ChildModelField = foreignkey;
                relationList.Add(relation);
            }
        }

        /// <summary>
        /// 在存在表情况下，删除和添加字段
        /// </summary>
        /// <param name="db"></param>
        /// <param name="mainTableName">主表名</param>
        /// <param name="input">表信息集合</param>
        /// <param name="key">表名</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task TableExistProcessField(List<TableFieldsItem> list, string key,
            List<LowCodeField> fieldList, StringBuilder sql)
        {
            using var db = _databaseManager.GetDatabase(null);

            //获取表字段集合
            var oldFieldList = await _databaseManager.GetFieldList(null, key);

            //添加字段
            foreach (var fieldItem in list)
            {
                //判断字段是否存在
                var fieldIsExis = oldFieldList.Find(x => x.Field == fieldItem.Name);
                var isExitLowcodeField = await _lowCodeFieldManager.QueryAsNoTracking.
                    FirstOrDefaultAsync(x => x.TableName == key && x.FieldName == fieldItem.Name);

                var fieldInfo = ProcessFieldType(fieldItem.ComponentType);
                var fieldSql = $"alter table {key} add {fieldItem.Name}{fieldInfo.DataType} default null;\r\n";
                LowCodeField field = new LowCodeField();
                field.TableName = key;
                field.FieldName = fieldItem.Name;
                field.FieldDesc = fieldItem.Name;
                field.DataType = fieldInfo.FieldType.Trim() == "Uniqueidentifier" ? "Guid" : fieldInfo.FieldType.Trim();
                field.DataLength = fieldInfo.DataLength;
                field.IsAllowNull = true;

                if (fieldIsExis == null && isExitLowcodeField != null)
                {
                    sql.Append(fieldSql);
                }
                else if (fieldIsExis != null && isExitLowcodeField == null)
                {
                    fieldList.Add(field);
                }
                else if (fieldIsExis == null && isExitLowcodeField == null)
                {
                    sql.Append(fieldSql);
                    fieldList.Add(field);
                }
            }
        }

        /// <summary>
        /// 添加字段维护表数据
        /// </summary>
        /// <param name="_fields"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldDesc"></param>
        /// <param name="dataType"></param>
        /// <param name="dataLength"></param>
        /// <param name="isAllowNull"></param>
        private void CreateLowcodeField(List<LowCodeField> _fields, string tableName, string fieldName,
            string fieldDesc, string dataType, int? dataLength, bool isAllowNull,
            bool isPrimaryKey, bool isForeignkey)
        {
            if (CheckStringChineseUn(fieldName))
            {
                throw new UserFriendlyException("字段名中不能含有汉字！！！");
            }
            LowCodeField field = new LowCodeField();
            field.TableName = tableName;
            field.FieldName = fieldName;
            field.FieldDesc = fieldDesc;
            field.DataType = dataType.Trim() == "Uniqueidentifier" ? "Guid" : dataType.Trim();
            field.DataLength = dataLength;
            field.IsAllowNull = isAllowNull;
            field.IsPrimaryKey = isPrimaryKey;
            field.IsForeignkey = isForeignkey;
            _fields.Add(field);
        }

        /// <summary>
        /// 维护旧表数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task MaintainOldTables(string key, string mainTableName)
        {
            //获取表字段集合
            var oldFieldList = await _databaseManager.GetFieldList(null, key);
            //维护旧表字段
            foreach (var item in oldFieldList)
            {
                LowCodeField field = new LowCodeField();
                field.TableName = key;
                field.FieldName = item.Field;
                field.FieldDesc = item.FieldName;
                field.DataType = item.DataType.Trim() == "Uniqueidentifier" ? "Guid" : item.DataType.Trim();
                field.DataLength = long.Parse(item.DataLength);
                field.IsAllowNull = item.AllowNull == 1 ? true : false;
                field.IsPrimaryKey = item.PrimaryKey == 1 ? true : false;
                field.IsForeignkey = item.Field == $"{mainTableName}_{key}_id" ? true : false;
                await _lowCodeFieldManager.MaintainFields(field);
            }
        }

        private class LowCodeModelRelationComparer : IEqualityComparer<LowCodeModelRelation>
        {
            public bool Equals(LowCodeModelRelation x, LowCodeModelRelation y)
            {
                if (x == null && y == null || x.ChildModelField.IsNullOrEmpty() || y.ChildModelField.IsNullOrEmpty())
                {
                    return true;
                }
                else if (x == null || y == null)
                {
                    return false;
                }
                else
                {
                    return x.MainModelName == y.MainModelName && x.ChildModelName != y.ChildModelName;
                }
            }

            public int GetHashCode(LowCodeModelRelation obj)
            {
                return obj.MainModelName.GetHashCode() ^ obj.ChildModelName.GetHashCode();
            }
        }

        /// <summary>
        /// 处理字段类型
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private FieldInfoDto ProcessFieldType(ComponentType component)
        {
            var fieldInfo = new FieldInfoDto();
            switch (component)
            {
                case ComponentType.ShortText:
                case ComponentType.TimeRange:
                case ComponentType.Time:
                case ComponentType.DateRange:
                case ComponentType.Dropdown:
                case ComponentType.DropdownMulti:
                case ComponentType.TreeSelect:
                case ComponentType.ColorSelect:
                case ComponentType.DropdownCascade:
                case ComponentType.Qrcode:
                case ComponentType.MultiplEnumerical:
                case ComponentType.Radio:
                case ComponentType.Checkbox:
                    fieldInfo.DataType = " Nvarchar(200) NULL";
                    fieldInfo.FieldType = " Nvarchar";
                    fieldInfo.DataLength = 200;
                    break;

                case ComponentType.LongText:
                case ComponentType.Attachment:
                case ComponentType.PictureAttachment:
                case ComponentType.RichEditor:
                case ComponentType.Image:
                case ComponentType.HandwritingBoard:
                    fieldInfo.DataType = " Nvarchar(MAX) NULL";
                    fieldInfo.FieldType = " Nvarchar";
                    fieldInfo.DataLength = -1;
                    break;

                case ComponentType.Number:
                case ComponentType.Barcode:
                case ComponentType.Slider:
                case ComponentType.Rate:
                    fieldInfo.DataType = " Int NULL";
                    fieldInfo.FieldType = " Int";
                    break;

                case ComponentType.Switch:
                    fieldInfo.DataType = " Bit NULL";
                    fieldInfo.FieldType = " Bit";
                    break;

                case ComponentType.Date:
                    fieldInfo.DataType = " Datetime2 NULL";
                    fieldInfo.FieldType = " Datetime2";
                    break;

                case ComponentType.RdoSelect:
                    fieldInfo.DataType = " Uniqueidentifier NULL";
                    fieldInfo.FieldType = " Guid";
                    break;

                default:
                    fieldInfo.DataType = " Nvarchar(200) NULL";
                    fieldInfo.FieldType = " Nvarchar";
                    fieldInfo.DataLength = 200;
                    break;
            }
            return fieldInfo;
        }

        public bool CheckStringChineseUn(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5|\u3002|\uff1f|\uff01|\uff0c|\u3001|\uff1b|\uff1a|\u201c|\u201d|\u2018|\u2019|\uff08|\uff09|\u300a|\u300b|\u3008|\u3009|\u3010|\u3011|\u300e|\u300f|\u300c|\u300d|\ufe43|\ufe44|\u3014|\u3015|\u2026|\u2014|\uff5e|\ufe4f|\uffe5]");
        }

        /// <summary>
        /// 判断数据库类型
        /// </summary>
        /// <returns></returns>
        private int JudgeDatabase()
        {
            using var db = _databaseManager.GetDatabase(null);
            using (db)
            {
                int dataBaseType = 0;
                switch (db.CurrentConnectionConfig.DbType.ToString())
                {
                    case "SqlServer":
                        dataBaseType = 0;
                        break;

                    case "Oracle":
                        dataBaseType = 1;
                        break;

                    case "MySql":
                        dataBaseType = 2;
                        break;

                    default:
                        throw new UserFriendlyException("数据库类型目前不支持");
                }
                return dataBaseType;
            }
        }
    }
}
