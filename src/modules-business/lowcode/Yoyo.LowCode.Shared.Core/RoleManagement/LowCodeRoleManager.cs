using System.Collections.Generic;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Yoyo.Pro.Roles;
using Yoyo.Pro.Users;

namespace Yoyo.LowCode.RoleManagement
{
    public class LowCodeRoleManager : RoleManagerBase
    {
        public LowCodeRoleManager(AbpRoleStore<Role, User> store, IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<AbpRoleManager<Role, User>> logger,
            IPermissionManager permissionManager, ICacheManager cacheManager, IUnitOfWorkManager unitOfWorkManager,
            IRoleManagementConfig roleManagementConfig, IRepository<OrganizationUnit> organizationUnitRepository,
            IRepository<OrganizationUnitRole> organizationUnitRoleRepository) : base(store, roleValidators,
            keyNormalizer, errors, logger, permissionManager, cacheManager, unitOfWorkManager, roleManagementConfig,
            organizationUnitRepository, organizationUnitRoleRepository)
        {
        }
    }
}
