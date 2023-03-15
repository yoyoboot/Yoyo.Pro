// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro.DependencyInjection
{
    public class AbpLazyServiceProvider : IAbpLazyServiceProvider, ITransientDependency
    {
        protected IDictionary<Type, object> CachedServices { get; set; }

        protected IServiceProvider ServiceProvider { get; set; }

        public AbpLazyServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            CachedServices = new Dictionary<Type, object>();
        }

        public virtual T LazyGetRequiredService<T>()
        {
            return (T)LazyGetRequiredService(typeof(T));
        }

        public virtual object LazyGetRequiredService(Type serviceType)
        {
            return CachedServices.GetOrAdd(serviceType, () => ServiceProvider.GetRequiredService(serviceType));
        }

        public virtual T LazyGetService<T>()
        {
            return (T)LazyGetService(typeof(T));
        }

        public virtual object LazyGetService(Type serviceType)
        {
            return CachedServices.GetOrAdd(serviceType, () => ServiceProvider.GetService(serviceType));
        }

        public virtual T LazyGetService<T>(T defaultValue)
        {
            return (T)LazyGetService(typeof(T), defaultValue);
        }

        public virtual object LazyGetService(Type serviceType, object defaultValue)
        {
            return LazyGetService(serviceType) ?? defaultValue;
        }

        public virtual T LazyGetService<T>(Func<IServiceProvider, object> factory)
        {
            return (T)LazyGetService(typeof(T), factory);
        }

        public virtual object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory)
        {
            return CachedServices.GetOrAdd(serviceType, () => factory(ServiceProvider));
        }
    }

}
