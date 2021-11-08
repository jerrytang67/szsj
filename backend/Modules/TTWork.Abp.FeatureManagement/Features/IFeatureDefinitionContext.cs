namespace TTWork.Abp.FeatureManagement.Features
{
    public interface IFeatureDefinitionContext
    {
        FeatureDefinition GetOrNull(string name);

        void Add(params FeatureDefinition[] definitions);
    }
}