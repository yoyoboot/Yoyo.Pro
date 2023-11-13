// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using SqlSugar;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.LowCode.Models;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.Databases.DomainService
{
    public interface IDatabaseManager : IBasicDomainService<BaseDbLink, Guid>
    {
        /// <summary>
        ///     表列表
        /// </summary>
        /// <param name="linkId">数据连接Id</param>
        /// <returns></returns>
        Task<List<DbTableModel>> GetTableList(Guid? linkId);

        /// <summary>
        ///     表字段
        /// </summary>
        /// <param name="linkId">数据连接</param>
        /// <param name="table">表名</param>
        /// <returns></returns>
        Task<List<DbTableFieldModel>> GetFieldList(Guid? linkId, string table);

        /// <summary>
        ///     表字段
        /// </summary>
        /// <param name="linkId">数据连接</param>
        /// <param name="table">表名</param>
        /// <returns></returns>
        List<DbTableFieldModel> GetFieldListByNoAsync(Guid? linkId, string table);

        /// <summary>
        ///     表数据
        /// </summary>
        /// <param name="linkId">数据连接</param>
        /// <param name="table">表名</param>
        /// <param name="requestParam">请求参数</param>
        /// <returns></returns>
        Task<PagedResultDto<object>> GetData(Guid? linkId, string table, int maxResultCount, int skipCount);

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="linkId">数据连接</param>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        Task<int> ExecuteSql(Guid? linkId, string strSql);

        /// <summary>
        ///     删除表
        /// </summary>
        /// <param name="linkId">数据连接</param>
        /// <param name="table">表名</param>
        Task Delete(Guid? linkId, string table);

        /// <summary>
        ///     创建表
        /// </summary>
        /// <param name="linkId">数据连接</param>
        /// <param name="tableModel">表对象</param>
        /// <param name="tableFieldList">字段对象</param>
        Task Create(Guid? linkId, DbTableModel tableModel, List<DbTableFieldModel> tableFieldList, string deleteTableSql = "");

        /// <summary>
        ///     修改表
        /// </summary>
        /// <param name="linkId">数据连接</param>
        /// <param name="oldTable">旧表</param>
        /// <param name="tableModel">表对象</param>
        /// <param name="tableFieldList">字段对象</param>
        Task Update(Guid? linkId, string oldTable, DbTableModel tableModel, List<DbTableFieldModel> tableFieldList);

        /// <summary>
        /// 根据链接对象链接数据库
        /// </summary>
        /// <param name="linkId"></param>
        /// <returns></returns>
        public SqlSugarClient GetDatabase(Guid? linkId, string connectionStr = null);

        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <param name="linkId"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task DeleteTableData(Guid? linkId, string tableName);

        /// <summary>
        /// API网关连接数据库查询数据
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<DataTable> ExecuteDataTableAsync(string dbType, string connectionStr, string sql, SugarParameter[] param = null);
    }
}
