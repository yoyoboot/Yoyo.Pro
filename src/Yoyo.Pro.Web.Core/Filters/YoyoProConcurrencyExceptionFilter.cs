// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Abp.Dependency;
using Abp.Domain.Uow;


namespace Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class YoyoProConcurrencyExceptionFilter : IExceptionFilter, ITransientDependency
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is AbpDbConcurrencyException abpDbConcurrencyException)
            {
                context.Exception = new UserFriendlyException(
                    "并发错误!",
                    abpDbConcurrencyException
                    );
            }
            else if (context.Exception is DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                context.Exception = new UserFriendlyException(
                    "并发错误!",
                    dbUpdateConcurrencyException
                    );
            }
        }
    }

}
