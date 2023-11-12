// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Domain.Services;
using Abp.Timing;
using Abp.UI;

namespace Yoyo.LowCode
{
    public interface ILowCodeDomainService : IDomainService
    {
    }

    public abstract class LowCodeDomainService : DomainService, ILowCodeDomainService
    {
        protected LowCodeDomainService()
        {
            LocalizationSourceName = LowCodeConfigs.Localization.SourceName;
        }

        /// <summary>
        /// 抛出 ThrowUserFriendlyError 异常
        /// </summary>
        protected virtual void ThrowUserFriendlyError(string reason)
        {
            throw new UserFriendlyException(L("Error"),
                L("UserFriendlyError", reason, Clock.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            );
        }
    }
}
