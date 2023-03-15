using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Uow;
using Yoyo.Pro.Roles;
using Yoyo.Pro.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Yoyo.Pro.MultiTenancy;
using Abp.Authorization.Users;

namespace Yoyo.Pro.Identity
{
    public abstract class SignInManagerBase : AbpSignInManager<Tenant, Role, User>
    {
        public SignInManagerBase(
            AbpUserManager<Role, User> userManager,
            IHttpContextAccessor contextAccessor,
            AbpUserClaimsPrincipalFactory<User, Role> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<User>> logger,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<User> userConfirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, unitOfWorkManager, settingManager, schemes, userConfirmation)
        {
        }
    }
}
