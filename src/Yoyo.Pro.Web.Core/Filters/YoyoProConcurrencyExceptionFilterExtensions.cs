// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Abp.AspNetCore.Mvc.ExceptionHandling;

namespace Microsoft.AspNetCore.Mvc
{
    public static class YoyoProConcurrencyExceptionFilterExtensions
    {
        /// <summary>
        /// 添加并发异常Filter
        /// </summary>
        /// <param name="mvcOptions"></param>
        /// <returns></returns>
        public static MvcOptions AddYoyoProConcurrencyExceptionFilter(this MvcOptions mvcOptions)
        {
            var abpExceptionFilterIndex = mvcOptions.Filters.IndexOf<AbpExceptionFilter>();

            mvcOptions.Filters.Insert<YoyoProConcurrencyExceptionFilter>(abpExceptionFilterIndex + 1);

            return mvcOptions;
        }
    }
}
