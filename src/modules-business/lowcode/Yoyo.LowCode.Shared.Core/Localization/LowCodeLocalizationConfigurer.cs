// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;

namespace Yoyo.LowCode.Localization
{
    public static class LowCodeLocalizationConfigurer
    {
        public static void AddLowCodeLocalization(this ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                 new DictionaryBasedLocalizationSource(
                     LowCodeConfigs.Localization.SourceName,
                     new JsonEmbeddedFileLocalizationDictionaryProvider(
                         typeof(LowCodeLocalizationConfigurer).Assembly,
                         "Yoyo.LowCode.Localization.SourceFiles"
                     )
                 )
             );
        }
    }
}
