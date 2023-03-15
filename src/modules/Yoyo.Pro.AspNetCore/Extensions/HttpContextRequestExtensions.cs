// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Yoyo.Pro.Extensions
{
    public static class HttpContextRequestExtensions
    {
        public const string RequestedWithHeader = "X-Requested-With";
        public const string RequestedWithHeaderLower = "x-requested-with";
        public const string XmlHttpRequest = "XMLHttpRequest";
        public const string RefererHeader = "Referer";
        public const string RefererFromSwagger = "swagger";

        public static bool IsAjax([NotNull] this HttpContext httpContext)
        {
            Check.NotNull(httpContext, nameof(httpContext));
            Check.NotNull(httpContext.Request, nameof(httpContext.Request));

            if (httpContext.Request.Headers == null)
            {
                return false;
            }

            if (httpContext.Request.Headers.TryGetValue(RefererHeader, out StringValues refererValues)
                && refererValues.ToString().Contains(RefererFromSwagger))
            {
                return true;
            }

            var ajaxHeader = httpContext.Request.Headers
                .FirstOrDefault(o => o.Key == RequestedWithHeaderLower || o.Key == RequestedWithHeader);

            return ajaxHeader.Value == XmlHttpRequest;
        }

    }
}
