// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Uow;
using Yoyo.Pro.Users;
using Yoyo.Pro.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Yoyo.Pro.Users
{
    /// <summary>
    /// 给用户凭据自定义添加字段
    /// </summary>
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        protected readonly IUserClaimsPrincipalProcessor<User> _claimsPrincipalProcessor;

        public UserClaimsPrincipalFactory(
            AbpUserManager<Role, User> userManager,
            AbpRoleManager<Role, User> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            IUnitOfWorkManager unitOfWorkManager,
            IUserClaimsPrincipalProcessor<User> claimsPrincipalProcessor = null)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor,
                  unitOfWorkManager
                  )
        {
            _claimsPrincipalProcessor = claimsPrincipalProcessor;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);

            if (_claimsPrincipalProcessor != null)
            {
                await _claimsPrincipalProcessor.Run(principal, user);
            }

            return principal;
        }
    }
}
