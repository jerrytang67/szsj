using System.Collections.Generic;

namespace TTWork.Abp.FeatureManagement.Features
{
    public interface IFeatureValueProviderManager
    {
        List<IFeatureValueProvider> Providers { get; }
    }
}