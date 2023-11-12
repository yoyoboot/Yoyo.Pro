// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.UI;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.LowCode.Extension;
using Yoyo.LowCode.Models;
using Yoyo.Pro.Domain;
using DatabaseType = SqlSugar.DbType;

namespace Yoyo.LowCode.Databases.DomainService
{
    public class DatabaseManager : BasicDomainService<BaseDbLink, Guid>, IDatabaseManager
    {
        private readonly IConfiguration _appConfiguration;

        private readonly IDbLinkManager _dbLinkManager;

        public DatabaseManager(IServiceProvider serviceProvider,
            IConfiguration appConfiguration,
            IDbLinkManager dbLinkManager) : base(
            serviceProvider)
        {
            _appConfiguration = appConfiguration;
            _dbLinkManager = dbLinkManager;
            LocalizationSourceName = LowCodeConsts.LocalizationSourceName;
        }

        /// <summary>
        ///     表列表
        /// </summary>
        /// <param name="linkId">数据连接</param>
        /// <returns></returns>
        public async Task<List<DbTableModel>> GetTableList(Guid? linkId)
        {
            using var db = GetDatabase(linkId);
            var strSql = DbSqlBuilder.DBTableSql(db.CurrentConnectionConfig.DbType);

            var reader = await db.Ado.GetDataReaderAsync(strSql);
            return reader.ToList<DbTableModel>();
        }

        /// <summary>
        ///     表字段
        /// </summary>
        /// <param name="linkId">数据连接</param>
        /// <param name="table">表名</param>
        /// <returns></returns>
        public async Task<List<DbTableFieldModel>> GetFieldList(Guid? linkId, string table)
        {
            using var db = GetDatabase(linkId);

            var strSql = DbSqlBuilder.DBTableField(db.CurrentConnectionConfig.DbType);
            if (db.CurrentConnectionConfig.DbType == DatabaseType.Oracle)
            {
                table = table.ToUpper();
            }

            var reader = await db.Ado.GetDataReaderAsync(string.Format(strSql, table));

            var entity = reader.ToList<DbTableFieldModel>();
            entity.ForEach((item) =>
            {
                switch (item.DataType.ToUpper())
                {
                    case "GUID":
                    case "UNIQUEIDENTIFIER":
                    case "CHAR":
                        item.DataTypeEnum = DataTypeEnum.Guid;
                        item.DataType = DataTypeEnum.Guid.ToString();
                        break;

                    case "RAW":
                        item.DataTypeEnum = DataTypeEnum.Raw;
                        item.DataType = DataTypeEnum.Raw.ToString();
                        break;

                    case "BIGINT":
                        item.DataTypeEnum = DataTypeEnum.Bigint;
                        item.DataType = DataTypeEnum.Bigint.ToString();
                        break;

                    case "INT":
                    case "NUMBER":
                        item.DataTypeEnum = DataTypeEnum.Int;
                        item.DataType = DataTypeEnum.Int.ToString();
                        break;

                    case "NVARCHAR":
                    case "NVARCHAR2":
                    case "VARCHAR":
                        if (item.DataLength == "-1")
                        {
                            item.DataTypeEnum = DataTypeEnum.Maxnvarchar;
                            item.DataType = DataTypeEnum.Maxnvarchar.ToString();
                            break;
                        }
                        item.DataTypeEnum = DataTypeEnum.Nvarchar;
                        item.DataType = DataTypeEnum.Nvarchar.ToString();
                        break;

                    case "TIMESTAMP(7)":
                    case "DATETIME":
                        item.DataTypeEnum = DataTypeEnum.Datetime;
                        item.DataType = DataTypeEnum.Datetime.ToString();
                        break;

                    case "DATETIME2":
                        item.DataTypeEnum = DataTypeEnum.Datetime2;
                        item.DataType = DataTypeEnum.Datetime2.ToString();
                        break;

                    case "CLOB":
                    case "LONGTEXT":
                        item.DataTypeEnum = DataTypeEnum.Maxnvarchar;
                        item.DataType = DataTypeEnum.Maxnvarchar.ToString();
                        break;

                    case "BLOB":
                        item.DataTypeEnum = DataTypeEnum.Blobnvarchar;
                        item.DataType = DataTypeEnum.Blobnvarchar.ToString();
                        break;

                    case "BIT":
                        item.DataTypeEnum = DataTypeEnum.Bit;
                        item.DataType = DataTypeEnum.Bit.ToString();
                        break;

                    case "FLOAT":
                        item.DataTypeEnum = DataTypeEnum.Float;
                        item.DataType = DataTypeEnum.Float.ToString();
                        break;

                    case "Decimal":
                        item.DataTypeEnum = DataTypeEnum.Decimal;
                        item.DataType = DataTypeEnum.Decimal.ToString();
                        break;

                    default:
                        break;
                }
            });
            return entity;
        }

        public List<DbTableFieldModel> GetFieldListByNoAsync(Guid? linkId, string table)
        {
            if (table.IsNullOrEmpty())
            {
                return new List<DbTableFieldModel>();
            }

            using var db = GetDatabase(linkId);

            var strSql = DbSqlBuilder.DBTableField(db.CurrentConnectionConfig.DbType);

            var reader = db.Ado.GetDataReader(strSql);

            return reader.ToList<DbTableFieldModel>();
        }

        /// <summary>
        ///     表数据
        /// </summary>
        /// <param name="link">数据连接</param>
        /// <param name="table">表名</param>
        /// <param name="input">请求参数</param>
        /// <returns></returns>
        public async Task<PagedResultDto<object>> GetData(Guid? linkId, string table, int maxResultCount, int skipCount)
        {
            var db = GetDatabase(linkId);
            var dbSql = $"SELECT * FROM {table} WHERE 1=1";

            using (db)
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

                var ret = await db.SqlQueryable<object>(string.Format(dbSql))
                    .ToPageListAsync(skipCount, maxResultCount, totalCount);

                return new PagedResultDto<object>(totalCount, ret);
            }
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="link">数据连接</param>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        public async Task<int> ExecuteSql(Guid? linkId, string strSql)
        {
            using var db = GetDatabase(linkId);
            return await db.Ado.ExecuteCommandAsync(strSql);
        }

        /// <summary>
        ///     删除表
        /// </summary>
        /// <param name="link">数据连接</param>
        /// <param name="table">表名</param>
        public async Task Delete(Guid? linkId, string table)
        {
            //数据库类型
            var dbType = DatabaseType.SqlServer;
            //var relation = await _dbTableRelationManager.QueryAsNoTracking
            //    .Include(x => x.BaseCustomPage)
            //    .FirstOrDefaultAsync(x => x.Table == table);
            //if (relation != null)
            //{
            //    throw new UserFriendlyException(L("请先解除关联"),
            //        $"{table}已被用于功能管理中的{relation.BaseCustomPage.Title}");
            //}
            //获取表中数据总条数SQL
            var IsCountsql = DataHelper.GetDataCount(dbType, table);
            using var db = GetDatabase(linkId);
            //获取表中数据总条数
            var IsCount = db.Ado.GetInt(IsCountsql);
            if (IsCount > 0)
            {
                throw new UserFriendlyException(L("PleaseEmptyTheTableDataYourselfFirst"), "请先自行清空表数据");
            }
            await db.Ado.ExecuteCommandAsync($"DROP TABLE {table}");
        }

        /// <summary>
        ///     创建表
        /// </summary>
        /// <param name="link">数据连接</param>
        /// <param name="tableModel">表对象</param>
        /// <param name="tableFieldList">字段对象</param>
        public async Task Create(Guid? linkId, DbTableModel tableModel, List<DbTableFieldModel> tableFieldList, string deleteTableSql = "")
        {
            try
            {
                using var db = GetDatabase(linkId);
                using (db)
                {
                    switch (db.CurrentConnectionConfig.DbType)
                    {
                        case DatabaseType.SqlServer:
                            await CreateTableSqlServer(db, tableModel, tableFieldList, deleteTableSql);
                            break;

                        case DatabaseType.MySql:
                            await CreateTableMySql(db, tableModel, tableFieldList, deleteTableSql);
                            break;

                        case DatabaseType.Oracle:
                            await CreateTableOracle(db, tableModel, tableFieldList, deleteTableSql);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///     修改表
        /// </summary>
        /// <param name="link">数据连接</param>
        /// <param name="id">主键值</param>
        /// <param name="tableModel">表对象</param>
        /// <param name="tableFieldList">字段对象</param>
        public async Task Update(Guid? linkId, string oldTable, DbTableModel tableModel,
            List<DbTableFieldModel> tableFieldList)
        {
            //数据库类型
            var dbType = DatabaseType.SqlServer;
            //获取表中数据总条数SQL
            var IsCountsql = DataHelper.GetDataCount(dbType, oldTable);
            var tableModelOld = (await GetTableList(linkId)).Find(x => x.Table == oldTable);
            if (tableModelOld != null)
            {
                var tableFieldListOld = await GetFieldList(linkId, tableModelOld.Table);
                var db = GetDatabase(linkId);
                //获取表中数据总条数
                var IsCount = db.Ado.GetInt(IsCountsql);
                if (IsCount > 0)
                {
                    throw new UserFriendlyException(L("PleaseEmptyTheTableDataYourselfFirst"), "请先自行清空表数据");
                }
                using (db)
                {
                    await Create(linkId, tableModel, tableFieldList, $"DROP TABLE {oldTable}");
                }
            }
        }

        /// <summary>
        ///     获取表数据
        /// </summary>
        /// <param name="link"></param>
        /// <param name="table"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public DataTable GetData(Guid? linkId, string table)
        {
            using var db = GetDatabase(linkId);
            var dbSql = new StringBuilder();
            dbSql.AppendFormat("SELECT * FROM {0} WHERE 1=1", table);

            var reader = db.Ado.GetDataTable(dbSql.ToString());

            return reader;
        }

        #region Method

        /// <summary>
        ///     SqlServer创建表单+注释
        /// </summary>
        /// <param name="db">连接Db</param>
        /// <param name="tableModel">表</param>
        /// <param name="tableFieldList">字段</param>
        private async Task CreateTableSqlServer(SqlSugarClient db, DbTableModel tableModel,
            List<DbTableFieldModel> tableFieldList, string deleteTableSql = "")
        {
            try
            {
                var DataTypeDic = Enum.GetValues(typeof(DataTypeEnum))
                    .Cast<DataTypeEnum>()
                    .ToDictionary(key => key.ToString(), value => value);
                var strSql = new StringBuilder();
                if (!string.IsNullOrEmpty(deleteTableSql))
                {
                    strSql.Append(deleteTableSql + " \r\n");
                }
                strSql.Append("CREATE TABLE " + tableModel.Table + " \r\n");
                strSql.Append("(\r\n");
                foreach (var item in tableFieldList)
                {
                    DataTypeDic.TryGetValue(item.DataType.Trim(), out var IsType);

                    if (IsType == DataTypeEnum.Guid)
                    {
                        strSql.Append(item.Field + " " + "Uniqueidentifier");
                    }
                    if (IsType == DataTypeEnum.Maxnvarchar)
                    {
                        strSql.Append(item.Field + " " + "Nvarchar(MAX)");
                    }
                    if (IsType == DataTypeEnum.Blobnvarchar)
                    {
                        strSql.Append(item.Field + " " + "Varbinary(MAX)");
                    }
                    if (IsType == DataTypeEnum.Nvarchar)
                    {
                        if (item.DataLength == "")
                        {
                            strSql.Append(item.Field + " " + "Nvarchar(200)");
                        }
                        else
                        {
                            strSql.Append(item.Field + " " + $"Nvarchar({item.DataLength})");
                        }
                    }
                    if (IsType == DataTypeEnum.Int ||
                        IsType == DataTypeEnum.Bigint ||
                        IsType == DataTypeEnum.Datetime ||
                        IsType == DataTypeEnum.Datetime2 ||
                        IsType == DataTypeEnum.Bit ||
                        IsType == DataTypeEnum.Float)
                    {
                        strSql.Append(item.Field + " " + item.DataType);
                    }
                    if (item.PrimaryKey == 1)
                    {
                        strSql.Append(" primary key ");
                    }

                    if (item.AllowNull == 0)
                    {
                        strSql.Append(" NOT NULL ");
                    }
                    else
                    {
                        strSql.Append(" NULL ");
                    }

                    strSql.Append(",");
                }

                strSql.Remove(strSql.Length - 1, 1);
                strSql.Append("\r\n");
                strSql.Append(")\r\n\r\n");
                await db.Ado.ExecuteCommandAsync(strSql.ToString());

                var strSql1 = new StringBuilder();
                strSql1.Append("declare @CurrentUser sysname\r\n");
                strSql1.Append("select @CurrentUser = SCHEMA_NAME()\r\n");
                strSql1.Append("execute sp_addextendedproperty 'MS_Description', '" + tableModel.TableName +
                               "','user', @CurrentUser, 'table', '" + tableModel.Table + "'\r\n");
                await db.Ado.ExecuteCommandAsync(strSql1.ToString());
                foreach (var item in tableFieldList)
                {
                    var strSql2 = new StringBuilder();
                    strSql2.Append("declare @CurrentUser sysname\r\n");
                    strSql2.Append("select @CurrentUser = SCHEMA_NAME()\r\n");
                    strSql2.Append("execute sp_addextendedproperty 'MS_Description', '" + item.FieldName +
                                   "', 'user', @CurrentUser, 'table', '" + tableModel.Table + "', 'column', '" +
                                   item.Field + "'\r\n");
                    await db.Ado.ExecuteCommandAsync(strSql2.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///     MySql创建表单+注释
        /// </summary>
        /// <param name="db">连接Db</param>
        /// <param name="tableModel">表</param>
        /// <param name="tableFieldList">字段</param>
        private async Task CreateTableMySql(SqlSugarClient db, DbTableModel tableModel,
            List<DbTableFieldModel> tableFieldList, string deleteTableSql = "")
        {
            var strSql = new StringBuilder();
            try
            {
                var DataTypeDic = Enum.GetValues(typeof(DataTypeEnum))
                    .Cast<DataTypeEnum>()
                    .ToDictionary(key => key.ToString(), value => value);
                if (!string.IsNullOrEmpty(deleteTableSql))
                {
                    await db.Ado.ExecuteCommandAsync(deleteTableSql);
                }
                strSql.Append("CREATE TABLE IF NOT EXISTS " + tableModel.Table + " (\r\n");

                foreach (var item in tableFieldList)
                {
                    DataTypeDic.TryGetValue(item.DataType, out var IsType);
                    strSql.Append(item.Field);
                    if (IsType == DataTypeEnum.Guid)
                    {
                        strSql.Append(" CHAR(36)");
                    }
                    if (IsType == DataTypeEnum.Nvarchar)
                    {
                        if (item.DataLength == "")
                        {
                            strSql.Append($" VARCHAR(200)");
                        }
                        else
                        {
                            strSql.Append($" VARCHAR({item.DataLength})");
                        }
                    }
                    if (IsType == DataTypeEnum.Maxnvarchar)
                    {
                        strSql.Append(" LONGTEXT");
                    }
                    if (IsType == DataTypeEnum.Blobnvarchar)
                    {
                        strSql.Append(" BLOB");
                    }
                    if (IsType == DataTypeEnum.Float)
                    {
                        strSql.Append(" FLOAT(2)");
                    }
                    if (IsType == DataTypeEnum.Decimal)
                    {
                        strSql.Append(" Decimal(18,4)");
                    }
                    if (IsType == DataTypeEnum.Int)
                    {
                        strSql.Append(" INT");
                    }
                    if (IsType == DataTypeEnum.Bigint)
                    {
                        strSql.Append(" BigInt");
                    }
                    if (IsType == DataTypeEnum.Bit)
                    {
                        strSql.Append(" BIT");
                    }
                    if (IsType == DataTypeEnum.Datetime || IsType == DataTypeEnum.Datetime2)
                    {
                        strSql.Append(" DATETIME(6)");
                    }
                    if (item.PrimaryKey == 1)
                    {
                        strSql.Append(" primary key");
                    }

                    if (item.AllowNull == 0)
                    {
                        strSql.Append(" NOT NULL");
                    }
                    else
                    {
                        strSql.Append(" NULL");
                    }

                    strSql.Append($" COMMENT '{item.FieldName}'");
                    strSql.Append(",");
                }
                strSql.Remove(strSql.Length - 1, 1);
                strSql.Append("\r\n");
                strSql.Append($")COMMENT='{tableModel.TableName}'\r\n\r\n");
                await db.Ado.ExecuteCommandAsync(strSql.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception($"StartSql-------{strSql}--------EndSql", ex);
            }
        }

        /// <summary>
        ///     Oracle创建表单+注释
        /// </summary>
        /// <param name="db">连接Db</param>
        /// <param name="tableModel">表</param>
        /// <param name="tableFieldList">字段</param>
        private async Task CreateTableOracle(SqlSugarClient db, DbTableModel tableModel,
            List<DbTableFieldModel> tableFieldList, string deleteTableSql = "")
        {
            try
            {
                var DataTypeDic = Enum.GetValues(typeof(DataTypeEnum))
                    .Cast<DataTypeEnum>()
                    .ToDictionary(key => key.ToString(), value => value);

                #region 创建表

                var strSql = new StringBuilder();
                if (!string.IsNullOrEmpty(deleteTableSql))
                {
                    await db.Ado.ExecuteCommandAsync(deleteTableSql);
                }
                strSql.Append("CREATE TABLE " + tableModel.Table + " (\r\n");
                foreach (var item in tableFieldList)
                {
                    DataTypeDic.TryGetValue(item.DataType, out var IsType);
                    strSql.Append(item.Field);
                    if (IsType == DataTypeEnum.Guid)
                    {
                        strSql.Append(" RAW(16)");
                    }
                    if (IsType == DataTypeEnum.Nvarchar)
                    {
                        if (item.DataLength == "")
                        {
                            strSql.Append($" NVARCHAR2(200)");
                        }
                        else
                        {
                            strSql.Append($" NVARCHAR2({item.DataLength})");
                        }
                    }
                    if (IsType == DataTypeEnum.Maxnvarchar)
                    {
                        strSql.Append(" CLOB");
                    }
                    if (IsType == DataTypeEnum.Blobnvarchar)
                    {
                        strSql.Append(" BLOB");
                    }
                    if (IsType == DataTypeEnum.Float)
                    {
                        strSql.Append(" FLOAT(2)");
                    }
                    if (IsType == DataTypeEnum.Int || IsType == DataTypeEnum.Bit || IsType == DataTypeEnum.Bigint)
                    {
                        strSql.Append(" NUMBER(10,0)");
                    }
                    if (IsType == DataTypeEnum.Decimal)
                    {
                        strSql.Append(" NUMBER(6,2)");
                    }
                    if (IsType == DataTypeEnum.Datetime || IsType == DataTypeEnum.Datetime2)
                    {
                        strSql.Append(" TIMESTAMP(7)");
                    }
                    if (item.PrimaryKey == 1)
                    {
                        strSql.Append(" primary key ");
                    }

                    if (item.AllowNull == 0)
                    {
                        strSql.Append(" NOT NULL ");
                    }
                    else
                    {
                        strSql.Append(" NULL ");
                    }

                    strSql.Append(",");
                }

                strSql.Remove(strSql.Length - 1, 1);
                strSql.Append("\r\n");
                strSql.Append(")\r\n\r\n");
                await db.Ado.ExecuteCommandAsync(strSql.ToString());

                #endregion 创建表

                #region 添加注释

                var strSql1 = new StringBuilder();
                strSql1.AppendFormat("comment on table {0} is '{1}'\r\n", tableModel.Table.ToUpper(),
                    tableModel.TableName);
                await db.Ado.ExecuteCommandAsync(strSql1.ToString());
                foreach (var item in tableFieldList)
                {
                    var strSql2 = new StringBuilder();
                    strSql2.AppendFormat("comment on column {0}.{1} is '{2}'\r\n", tableModel.Table.ToUpper(),
                        item.Field,
                        item.FieldName);
                    await db.Ado.ExecuteCommandAsync(strSql2.ToString());
                }

                #endregion 添加注释
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///     根据链接对象链接数据库
        /// </summary>
        /// <param name="linkId"></param>
        /// <returns></returns>
        public SqlSugarClient GetDatabase(Guid? linkId, string connectionStr = null)
        {
            //如果传了连接字符串 代表就是网关这边查询 直接返回
            if (connectionStr != null)
            {
                return new SqlSugarClient(new ConnectionConfig
                {
                    DbType = DatabaseType.SqlServer,
                    ConnectionString = connectionStr,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = false
                });
            }
            var dbType = DatabaseType.SqlServer;
            var connectionString = "";
            if (linkId == null)
            {
                var connectionType = _appConfiguration["ConnectionStrings:DatabaseType"];

                if (!string.IsNullOrEmpty(connectionType))
                {
                    dbType = connectionType.ToDatabaseType();
                }

                connectionString = _appConfiguration["ConnectionStrings:Default"];
            }
            else
            {
                var link = _dbLinkManager.QueryAsNoTracking
                    .FirstOrDefault(x => x.Id == linkId);

                if (link != null)
                {
                    dbType = link.DbType;

                    connectionString = DataHelper.ToConnectionString(dbType, link.Host, link.Port, link.UserName,
                        link.Password,
                        link.ServiceName);
                }
            }

            return new SqlSugarClient(new ConnectionConfig
            {
                DbType = dbType,
                ConnectionString = connectionString,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = false
            });
        }

        /// <summary>
        ///     数据库数据类型转换
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        private string DataTypeConversion(string dataType, DatabaseType databaseType)
        {
            if (databaseType.Equals(DatabaseType.Oracle))
            {
                switch (dataType)
                {
                    case "text":
                        return "LONG";

                    case "decimal":
                        return "NUMBER(6,2)";

                    case "datetime":
                        return "DATE";

                    case "bigint":
                        return "NUMBER";

                    default:
                        return dataType.ToUpper();
                }
            }

            if (databaseType.Equals(DatabaseType.MySql))
            {
                return dataType;
            }

            return dataType;
        }

        public async Task DeleteTableData(Guid? linkId, string tableName)
        {
            var db = GetDatabase(linkId);
            var dbSql = $"DELETE FROM {tableName}";

            using (db)
            {
                await db.Ado.ExecuteCommandAsync(dbSql);
            }
        }

        #endregion Method

        #region API网关

        /// <summary>
        ///  API网关连接数据库查询数据
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteDataTableAsync(string dbType, string connectionStr, string sql,
             SugarParameter[] param = null)
        {
            if (dbType.ToLower() == "sqlserver".ToLower())
            {
                using var db = new SqlSugarClient(new ConnectionConfig
                {
                    DbType = DatabaseType.SqlServer,
                    ConnectionString = connectionStr,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true
                });

                var dataTable = await db.Ado.GetDataTableAsync(sql, param);

                return dataTable;
            }
            else if (dbType.ToLower() == "mysql".ToLower())
            {
                using var db = new SqlSugarClient(new ConnectionConfig
                {
                    DbType = DatabaseType.MySql,
                    ConnectionString = connectionStr,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true
                });

                var dataTable = await db.Ado.GetDataTableAsync(sql, param);

                return dataTable;
            }
            else if (dbType.ToLower() == "oracle".ToLower())
            {
                using var db = new SqlSugarClient(new ConnectionConfig
                {
                    DbType = DatabaseType.Oracle,
                    ConnectionString = connectionStr,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true
                });

                var dataTable = await db.Ado.GetDataTableAsync(sql);

                return dataTable;
            }
            else
            {
                return null;
            }

            //using (db)
            //{
            //    //sql = " SELECT * FROM books WHERE 1=1 ";
            //    //获取表中数据总条数
            //    var dataTable = await db.Ado.GetDataTableAsync(sql, param);

            //    return dataTable;
            //}
        }

        #endregion API网关
    }
}
