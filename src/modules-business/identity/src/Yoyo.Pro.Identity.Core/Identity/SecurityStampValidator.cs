using Abp.Authorization;
using Abp.Domain.Uow;
using Yoyo.Pro.MultiTenancy;
using Yoyo.Pro.MultiTenancy.Tenants;
using Yoyo.Pro.Roles;
using Yoyo.Pro.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Yoyo.Pro.Identity
{
    public abstract class SecurityStampValidatorBase : AbpSecurityStampValidator<Tenant, Role, User>
    {
        public SecurityStampValidatorBase(
            IOptions<SecurityStampValidatorOptions> options,
            AbpSignInManager<Tenant, Role, User> signInManager,
            ILoggerFactory loggerFactory, IUnitOfWorkManager unitOfWorkManager)
            : base(options, signInManager, loggerFactory, unitOfWorkManager)
        {

        }
    }
}
