using System;
using Abp;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;

namespace Yoyo.Pro.Runtime.Session
{
    public class TestAbpSession : IAbpSession, ISingletonDependency
    {
        private readonly IMultiTenancyConfig _multiTenancy;
        private readonly IAmbientScopeProvider<SessionOverride> _sessionOverrideScopeProvider;
        private readonly ITenantResolver _tenantResolver;
        private string _tenantId;
        private string _userId;

        public TestAbpSession(
            IMultiTenancyConfig multiTenancy,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider,
            ITenantResolver tenantResolver)
        {
            _multiTenancy = multiTenancy;
            _sessionOverrideScopeProvider = sessionOverrideScopeProvider;
            _tenantResolver = tenantResolver;
        }

        public virtual string UserId
        {
            get
            {
                if (_sessionOverrideScopeProvider.GetValue(AbpSessionBase.SessionOverrideContextKey) != null)
                {
                    return _sessionOverrideScopeProvider.GetValue(AbpSessionBase.SessionOverrideContextKey).UserId;
                }

                return _userId;
            }
            set => _userId = value;
        }

        public virtual string TenantId
        {
            get
            {
                if (!_multiTenancy.IsEnabled)
                {
                    return "1";
                }

                if (_sessionOverrideScopeProvider.GetValue(AbpSessionBase.SessionOverrideContextKey) != null)
                {
                    return _sessionOverrideScopeProvider.GetValue(AbpSessionBase.SessionOverrideContextKey).TenantId;
                }

                var resolvedValue = _tenantResolver.ResolveTenantId();
                if (resolvedValue != null)
                {
                    return resolvedValue;
                }

                return _tenantId;
            }
            set
            {
                if (!_multiTenancy.IsEnabled && value != "1" && value != null)
                {
                    throw new AbpException(
                        "Can not set TenantId since multi-tenancy is not enabled. Use IMultiTenancyConfig.IsEnabled to enable it.");
                }

                _tenantId = value;
            }
        }

        public virtual MultiTenancySides MultiTenancySide => GetCurrentMultiTenancySide();

        public virtual string ImpersonatorUserId { get; set; }

        public virtual string ImpersonatorTenantId { get; set; }

        public virtual IDisposable Use(string tenantId, string userId)
        {
            return _sessionOverrideScopeProvider.BeginScope(AbpSessionBase.SessionOverrideContextKey,
                new SessionOverride(tenantId, userId));
        }

        protected virtual MultiTenancySides GetCurrentMultiTenancySide()
        {
            return _multiTenancy.IsEnabled && !TenantId.HasValue()
                ? MultiTenancySides.Host
                : MultiTenancySides.Tenant;
        }
    }
}
