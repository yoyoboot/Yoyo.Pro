// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.AspNetCore.Mvc.Controllers;

namespace Yoyo.LowCode.Controllers
{
    public abstract class LowCodeSharedControllerBase : AbpController
    {
        protected LowCodeSharedControllerBase()
        {
            LocalizationSourceName = LowCodeConfigs.Localization.SourceName;
        }
    }
}
