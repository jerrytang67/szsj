using Abp.Collections;

namespace TTWork.Abp.FeatureManagement.Features
{
    public class FeatureOptions
    {
        public ITypeList<IFeatureDefinitionProvider> DefinitionProviders { get; }

        public ITypeList<IFeatureValueProvider> ValueProviders { get; }

        public FeatureOptions()
        {
            DefinitionProviders = new TypeList<IFeatureDefinitionProvider>();
            ValueProviders = new TypeList<IFeatureValueProvider>();
        }
    }
}