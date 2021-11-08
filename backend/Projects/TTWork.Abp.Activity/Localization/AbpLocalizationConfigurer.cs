using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;
using TTWork.Abp.LaborUnion;

namespace TTWork.Abp.Activity.Localization
{
    public static class ActivityLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    ActivityConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        $"TTWork.Abp.{ActivityConsts.LocalizationSourceName}.Localization.JsonSources"
                    )
                )
            );
        }
    }
}