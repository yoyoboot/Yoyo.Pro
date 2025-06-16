// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro.DependencyInjection
{
    public interface ILazyLoader<T>
        where T : class
    {
        T Value { get; }

        Lazy<T> Lazy { get; }
    }

    public class LazyLoader<T> : ILazyLoader<T>
        where T : class
    {
        public T Value => _lazyInstace.Value;

        public Lazy<T> Lazy => _lazyInstace;

        readonly Lazy<T> _lazyInstace;

        public LazyLoader(IServiceProvider serviceProvider)
        {
            _lazyInstace = new Lazy<T>(() =>
            {
                return serviceProvider.GetRequiredService<T>();
            });
        }
    }
}
