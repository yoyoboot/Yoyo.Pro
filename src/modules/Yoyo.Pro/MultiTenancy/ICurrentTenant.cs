// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using JetBrains.Annotations;

namespace Abp.MultiTenancy
{
    public interface ICurrentTenant
    {
        bool IsAvailable { get; }

        [CanBeNull]
        string Id { get; }


        IDisposable Change(string tenantId);
    }
}
