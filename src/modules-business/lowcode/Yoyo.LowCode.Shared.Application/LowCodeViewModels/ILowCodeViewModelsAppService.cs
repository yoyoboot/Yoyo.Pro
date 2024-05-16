// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.Databases.Dtos;
using Yoyo.LowCode.LowCodeViewModels.Dtos;

namespace Yoyo.LowCode.LowCodeViewModels
{
    public interface ILowCodeViewModelsAppService : IApplicationService
    {
        /// <summary>
        /// 为表获取数据库所有表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<TableListOutput>> GetTableAsTable(GetDatabaseTableListInput input);

        /// <summary>
        /// 为下拉框获取数据库所有表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<TableListSelectOutput>> GetTableAsSelect(GetDatabaseTableListInput input);

        /// <summary>
        /// 通过主表名获取所有子表名称
        /// </summary>
        /// <param name="mainTableName"></param>
        /// <returns></returns>
        Task<List<TableListSelectOutput>> GetTableByAssociation(string mainTableName);

        /// <summary>
        /// 为表获取数据表字段信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<LowCodeFieldListDto>> GetTableFieldAsTable(GetTableFieldListInput input);

        /// <summary>
        /// 获取表的主键字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> GetTablePrimaryKey(GetTableFieldListInput input);

        /// <summary>
        /// 为下拉框获取数据表字段信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<LowCodeTableFieldsSelectorOutput>> GetTableFieldAsSelect(GetTableFieldListInput input);

        /// <summary>
        /// 获取表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<object>> GetTableData(GetTableDataInput input);

        /// <summary>
        /// 清空表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteTableData(DeleteTableDataInput input);

        /// <summary>
        /// 添加表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateTable(LowCodeCreateOrUpdateTableInput input);

        /// <summary>
        /// 修改表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateTable(LowCodeCreateOrUpdateTableInput input);

        /// <summary>
        /// 判断表在数据库是否存在
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> TableIsExists(List<TableInfoList> list);
    }
}
