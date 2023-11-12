// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.Databases.Dtos;
using Yoyo.LowCode.Models;

namespace Yoyo.LowCode.Databases
{
    public interface IDatabaseAppService : IApplicationService
    {
        /// <summary>
        /// 表列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<DatabaseTableListOutput>> GetTableList(GetDatabaseTableListInput input);

        /// <summary>
        /// 表字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<DbTableFieldModel>> GetTableFieldList(GetTableFieldListInput input);

        /// <summary>
        /// 表字段下拉列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<TableFieldsSelectorOutput>> GetTableFieldSelect(GetTableFieldListInput input);

        /// <summary>
        /// 获取表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<object>> GetTableData(GetTableDataInput input);

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteTable(DeleteTableInput input);

        /// <summary>
        /// 新增表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateTable(CreateOrUpdateTableInput input);

        /// <summary>
        /// 修改表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateTable(CreateOrUpdateTableInput input);
    }
}
