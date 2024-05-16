// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Yoyo.LowCode.LowCodeViewModels;
using Yoyo.LowCode.Renders.Dtos;

namespace Yoyo.LowCode.Renders
{
    public interface IRenderManager : ITransientDependency
    {
        /// <summary>
        /// 执行新增或修改
        /// </summary>
        /// <param name="tableRelationList">表关联关系</param>
        /// <param name="data">数据</param>
        /// <param name="afterExecuteSql">之后要执行的SQL语句</param>
        /// <param name="childIsDelete">手动控制子列表是否对比删除</param>
        /// <returns></returns>
        Task<object> CreateOrUpdate(List<LowCodeModelRelation> tableRelationList, CreateOrUpDate.CreateOrUpdateRenderDto data, string afterExecuteSql = "", bool childIsDelete = false);

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<object>> GetPaged(List<LowCodeModelRelation> dbTableRelations, string mainTableName, int maxResultCount, int skipCount,
            bool isPaged = true);

        /// <summary>
        /// 获取页面数据
        /// </summary>
        /// <param name="dbTableRelations"></param>
        /// <param name="mainTableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetById(List<LowCodeModelRelation> dbTableRelations, string mainTableName, string id,
            string mainTableSelectSql = "");

        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <param name="dbTableRelations"></param>
        /// <param name="mainTableName"></param>
        /// <param name="id"></param>
        /// <param name="afterExecuteSql"></param>
        /// <returns></returns>
        Task Delete(List<LowCodeModelRelation> dbTableRelations, string mainTableName, string id, string afterExecuteSql = "");
    }
}
