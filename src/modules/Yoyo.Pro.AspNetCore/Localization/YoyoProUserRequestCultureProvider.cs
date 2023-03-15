using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro.Localization
{
    public class YoyoProUserRequestCultureProvider : RequestCultureProvider
    {
        readonly RequestLocalizationOptions _requestLocalizationOptions;

        public YoyoProUserRequestCultureProvider(RequestLocalizationOptions requestLocalizationOptions)
        {
            _requestLocalizationOptions = requestLocalizationOptions;
        }

        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var cultureAccessor = httpContext.RequestServices.GetService<IRequestCultureAccessor>();

            cultureAccessor.Options = this._requestLocalizationOptions;

            return await cultureAccessor.GetUserRequestCulture(httpContext);
        }
    }
}

