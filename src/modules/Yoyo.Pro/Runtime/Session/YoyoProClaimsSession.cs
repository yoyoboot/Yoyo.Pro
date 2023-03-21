// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;

namespace Yoyo.Pro.Runtime.Session
{
    public class YoyoProClaimsSession : ClaimsAbpSession, IHasUserName, IHasClaimsPrincipal, ITransientDependency
    {
        public virtual string UserName => this.GetClaimValue(ClaimTypes.Name);

        public virtual ClaimsPrincipal ClaimsPrincipal => this.PrincipalAccessor.Principal;

        public YoyoProClaimsSession(
            IPrincipalAccessor principalAccessor,
            IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider)
            : base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        {
        }

        public virtual string GetClaimValue(string claimType)
        {
            var claimsPrincipal = this.ClaimsPrincipal;
            var claim = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == claimType);
            return string.IsNullOrEmpty(claim?.Value) ? null : claim.Value;
        }
    }
}

