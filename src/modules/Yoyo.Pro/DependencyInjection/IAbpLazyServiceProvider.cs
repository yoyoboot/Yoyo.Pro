// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Yoyo.Pro.DependencyInjection
{
    public interface IAbpLazyServiceProvider
    {
        T LazyGetRequiredService<T>();

        object LazyGetRequiredService(Type serviceType);

        T LazyGetService<T>();

        object LazyGetService(Type serviceType);

        T LazyGetService<T>(T defaultValue);

        object LazyGetService(Type serviceType, object defaultValue);

        object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory);

        T LazyGetService<T>(Func<IServiceProvider, object> factory);
    }

}
