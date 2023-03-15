// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Dependency;

using System.Runtime.InteropServices;

namespace System.Runtime
{
    public interface IOSPlatformProvider
    {
        OSPlatform GetCurrentOSPlatform();
    }

    public class OSPlatformProvider : IOSPlatformProvider, ITransientDependency
    {
        public virtual OSPlatform GetCurrentOSPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX; //MAC
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows;
            }

            return OSPlatform.Linux;
        }
    }
}
