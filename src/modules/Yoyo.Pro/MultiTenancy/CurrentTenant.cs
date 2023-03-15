// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Abp.Dependency;
using Abp.Runtime.Session;

namespace Abp.MultiTenancy
{
    public class CurrentTenant : ICurrentTenant, ITransientDependency
    {
        public virtual bool IsAvailable => Id.HasValue();

        public virtual string Id => AbpSession?.TenantId;

        protected IAbpSession AbpSession { get; }


        public CurrentTenant(IAbpSession abpSession)
        {
            AbpSession = abpSession;
        }

        public IDisposable Change(string tenantId)
        {
            return AbpSession.Use(tenantId, AbpSession.UserId);
        }
    }
}
