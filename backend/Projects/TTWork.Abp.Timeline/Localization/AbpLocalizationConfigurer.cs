using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;

namespace TTWork.Abp.Timeline.Localization
{
    public static class TimelineLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    TimelineConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        $"TTWork.Abp.{TimelineConsts.LocalizationSourceName}.Localization.JsonSources"
                    )
                )
            );
        }
    }
}