// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;


namespace Yoyo.Pro
{
    public abstract class YoyoProDbContext<TTenant, TRole, TUser, TSelf>
        : YoyoProDbContextBase<TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
        where TSelf : YoyoProDbContext<TTenant, TRole, TUser, TSelf>
    {

        protected YoyoProDbContext(DbContextOptions<TSelf> options)
            : base(options)
        {
        }
    }
}
