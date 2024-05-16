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
    public class UserQueryGroupSubManager : BasicDomainService<BaseQueryGroupSub, Guid>, IUserQueryGroupSubManager
    {
        /// <inheritdoc/>
        public UserQueryGroupSubManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <inheritdoc/>
        public async Task<bool> UserQueryGroupHasBeenUsed(Guid userQueryGroupId)
        {
            return await this.QueryAsNoTracking.Where(o => o.SubUserQueryGroupId == userQueryGroupId).AnyAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteSubGroupByGroupId(Guid inputId)
        {
            await Delete(group => group.UserQueryGroupId == inputId);
        }
    }
}
