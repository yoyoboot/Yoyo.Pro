// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Uow;
using Yoyo.Pro.Database;

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition.DomainService
{
    public class UserQueryExecuter : IUserQueryExecuter, ITransientDependency
    {
        #region Private Fields

        private readonly ISqlExecutor _sqlExecutor;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        #endregion Private Fields

        #region Public Constructors

        public UserQueryExecuter(ISqlExecutor sqlExecutor,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _sqlExecutor = sqlExecutor;
            _unitOfWorkManager = unitOfWorkManager;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <inheritdoc/>
        public async Task<DataTable> Execute(BaseUserQuery query)
        {
            return await this.Execute(query, query.GetQueryParamters());
        }

        /// <inheritdoc/>
        public async Task<DataTable> Execute(BaseUserQuery query, Dictionary<string, object> parms)
        {
            return await this.Execute(query.QueryTemplate, parms);
        }

        /// <inheritdoc/>
        [UnitOfWork]
        public async Task<DataTable> Execute(string queryTemplate, Dictionary<string, object> parms)
        {
            var sql = this.GetQueryString(queryTemplate);

            try
            {
                using (var reader = await this._sqlExecutor.ExecuteReaderAsync(sql, parms))
                {
                    var dataTable = new DataTable();

                    dataTable.Load(reader);

                    return dataTable;
                }
            }
            catch (System.Exception ex)
            {
                throw new Abp.UI.UserFriendlyException("执行出错！请确认查询模板和输入参数是否正确！", ex.ToString());
            }
        }

        public string GetQueryString(string queryTemplate)
        {
            return queryTemplate.Replace("?", "@");
        }

        #endregion Public Methods
    }
}
