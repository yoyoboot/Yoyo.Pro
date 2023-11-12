// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Timing;
using Abp.UI;
using Yoyo.LowCode.MultiTenancy;
using Yoyo.LowCode.UserManagement;
using Yoyo.Pro.MultiTenancy;
using Yoyo.Pro.MultiTenancy.Tenants;
using Yoyo.Pro.Users;
using Microsoft.AspNetCore.Identity;

namespace Yoyo.LowCode
{
    public abstract class LowCodeSharedAppServiceBase : ApplicationService
    {
        protected LowCodeSharedAppServiceBase()
        {
            LocalizationSourceName = LowCodeConsts.LocalizationSourceName;
        }

        protected User user
        {
            get => GetCurrentUser();
            set => user = value;
        }

        public LowCodeTenantManager TenantManager { get; set; }

        public LowCodeUserManager LowCodeUserManager { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        /// <summary>
        ///     返回当前用户信息
        /// </summary>
        /// <returns></returns>
        protected async Task<User> GetCurrentUserAsync()
        {
            var user = await LowCodeUserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("当前用户不存在!");
            }
            return user;
        }

        /// <summary>
        ///     返回当前用户信息--非异步
        /// </summary>
        /// <returns></returns>
        protected User GetCurrentUser()
        {
            return AsyncHelper.RunSync(GetCurrentUserAsync);
        }

        /// <summary>
        ///     返回当前租户信息
        /// </summary>
        /// <returns></returns>
        protected async Task<Tenant> GetCurrentTenantAsync()
        {
            return await TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        /// <summary>
        ///     名称重复错误 传入多语言
        /// </summary>
        protected void RepetError(string name, string str = "RepetError")
        {
            throw new UserFriendlyException(L("Error"),
                L(str, name, Clock.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        /// <summary>
        ///     数据为空  传入多语言
        /// </summary>
        protected void NullError(string str = "NullError")
        {
            throw new UserFriendlyException(L("Error"),
                L(str, Clock.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        protected void ThrowException(string msg, ErrorTypes errorType = ErrorTypes.Exception, string title = "Error")
        {
            switch (errorType)
            {
                case ErrorTypes.Exception:
                    throw new Exception(L(msg));
                case ErrorTypes.UserFriendlyException:
                    if (title == "Error")
                    {
                        throw new UserFriendlyException(L(msg));
                    }
                    else
                    {
                        throw new UserFriendlyException(L(title), L(msg));
                    }
                default:
                    throw new Exception(L("Error500Desc"));
            }
        }

        protected void ThrowDeleteDefaultVersion(string rdoModuleName)
        {
            throw new UserFriendlyException(L("Error"), L(rdoModuleName));
        }

        /// <summary>
        ///     抛出 ThrowUserFriendlyError 异常
        /// </summary>
        protected void ThrowUserFriendlyError(string reason)
        {
            throw new UserFriendlyException(L("Error"),
                L("UserFriendlyError", reason, Clock.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            );
        }

        /// <summary>
        ///     抛出 DeleteError 异常
        /// </summary>
        /// <param name="def">业务</param>
        /// <param name="defRef1">业务引用1</param>
        /// <param name="defRef2">业务引用2</param>
        protected void ThrowDeleteError(string def, string defRef1, string defRef2)
        {
            throw new UserFriendlyException(L("Error"),
                L("DeleteError", def, defRef1, defRef2, Clock.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            );
        }
    }

    public enum ErrorTypes
    {
        /// <summary>
        ///     普通异常
        /// </summary>
        Exception,

        /// <summary>
        ///     用户界面优化异常
        /// </summary>
        UserFriendlyException
    }
}
