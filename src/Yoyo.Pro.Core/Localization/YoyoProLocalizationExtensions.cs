// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;

namespace Yoyo.Pro.Localization
{
    public static class YoyoProLocalizationExtensions
    {
        public static void AddYoyoProLocalization(this ILocalizationConfiguration localization)
        {
            localization.Sources.Add(
                new DictionaryBasedLocalizationSource(YoyoProConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                      typeof(YoyoProLocalizationExtensions).Assembly,
                      "Yoyo.Pro.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
