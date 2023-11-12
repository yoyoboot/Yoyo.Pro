// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.UserQueryModule.UserQueryRelationalDefinition.DomainService
{
    /// <inheritdoc/>
    public class UserQueryGroupEntityManager : BasicDomainService<BaseUserQueryGroupEntity, Guid>, IUserQueryGroupEntityManager
    {
        /// <inheritdoc/>
        public UserQueryGroupEntityManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <inheritdoc/>
        public async Task<bool> UserQueryHasBeenUsed(Guid userQueryId)
        {
            return await this.QueryAsNoTracking.Where(o => o.UserQueryId == userQueryId).AnyAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteGroupItemByGroupId(Guid inputId)
        {
            await this.Delete(itemEntity => itemEntity.UserQueryGroupId == inputId);
        }
    }
}
