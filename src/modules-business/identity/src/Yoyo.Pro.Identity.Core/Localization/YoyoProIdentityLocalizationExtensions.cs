using System.IO;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;

namespace Yoyo.Pro.Localization
{
    public static class YoyoProIdentityLocalizationExtensions
    {
        public static void AddYoyoProIdentityLocalization(this ILocalizationConfiguration localization)
        {
            localization.Sources.Add(
                new DictionaryBasedLocalizationSource(YoyoProIdentityConfigs.Localization.SourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                      typeof(YoyoProIdentityLocalizationExtensions).Assembly,
                      "Yoyo.Pro.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
