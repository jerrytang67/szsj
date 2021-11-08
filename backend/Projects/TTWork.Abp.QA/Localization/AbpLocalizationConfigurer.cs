using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;
using TTWork.Abp.LaborUnion;

namespace TTWork.Abp.QA.Localization
{
    // ReSharper disable once InconsistentNaming
    public static class QALocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    QAConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        $"TTWork.Abp.{QAConsts.LocalizationSourceName}.Localization.JsonSources"
                    )
                )
            );
        }
    }
}