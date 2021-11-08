using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;

namespace TTWork.Abp.LaborUnion.Localization
{
    public static class LaborUnionLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    LaborUnionConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        $"TTWork.Abp.{LaborUnionConsts.LocalizationSourceName}.Localization.JsonSources"
                    )
                )
            );
        }
    }
}