// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


namespace Microsoft.AspNetCore.Mvc
{
    using System;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Abp;

    /// <summary>
    /// MvcOptions 中的 Filters 扩展函数
    /// </summary>
    public static class MvcOptionsFilterCollectionExtensions
    {
        public static FilterCollection Insert<TFilter>(this FilterCollection filters, int index)
            where TFilter : IFilterMetadata
        {
            return filters.Insert(index, typeof(TFilter));
        }


        public static FilterCollection Insert(this FilterCollection filters, int index, Type filterType)
        {
            Check.NotNull(filterType, nameof(filterType));

            if (!typeof(IFilterMetadata).IsAssignableFrom(filterType))
            {
                throw new ArgumentException("请输入正确的 fitler 类型");
            }
            var filter = new ServiceFilterAttribute(filterType);
            filters.Insert(index, filter);

            return filters;
        }

        public static int IndexOf<TFilter>(this FilterCollection filters)
            where TFilter : IFilterMetadata
        {
            return filters.IndexOf(typeof(TFilter));
        }

        public static int IndexOf(this FilterCollection filters, Type filterType)
        {
            var index = 0;
            foreach (var item in filters)
            {
                if (item.GetType() == filterType)
                {
                    break;
                }
                else if (item is ServiceFilterAttribute serviceFilterAttribute)
                {
                    if (serviceFilterAttribute.ServiceType == filterType)
                    {
                        break;
                    }
                }
                index++;
            }

            return index;
        }
    }
}
