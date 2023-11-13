// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.LowCode.UserQueryModule.UserQueryRelationalDefinition.DomainService;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition.DomainService
{
    /// <inheritdoc/>
    public class UserQueryManager : BasicDomainService<BaseUserQuery, Guid>, IUserQueryManager
    {
        private readonly IUserQueryGroupEntityManager _userQueryGroupEntityManager;

        /// <inheritdoc/>
        public UserQueryManager(IServiceProvider serviceProvider,
            IUserQueryGroupEntityManager userQueryGroupEntityManager)
            : base(serviceProvider)
        {
            _userQueryGroupEntityManager = userQueryGroupEntityManager;
            LocalizationSourceName = LowCodeConsts.LocalizationSourceName;
        }

        public async Task<System.Data.DataTable> Execute(BaseUserQuery query, Dictionary<string, object> parms)
        {
            throw new NotImplementedException();
        }
    }
}
