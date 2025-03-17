using System.Collections.Generic;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using Yoyo.Pro.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Yoyo.Pro.Roles
{
    /// <summary>
    /// Role Manager Base
    /// </summary>
    public abstract class RoleManagerBase : AbpRoleManager<Role, User>
    {
        public RoleManagerBase(
            AbpRoleStore<Role, User> store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<AbpRoleManager<Role, User>> logger,
            IPermissionManager permissionManager,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            IRoleManagementConfig roleManagementConfig, IRepository<OrganizationUnit> organizationUnitRepository,
            IRepository<OrganizationUnitRole> organizationUnitRoleRepository)
            : base(
                store,
                roleValidators,
                keyNormalizer,
                errors, logger,
                permissionManager,
                cacheManager,
                unitOfWorkManager,
                roleManagementConfig, organizationUnitRepository, organizationUnitRoleRepository)
        {
            LocalizationSourceName = YoyoProIdentityConfigs.Localization.SourceName;
        }
    }
}
