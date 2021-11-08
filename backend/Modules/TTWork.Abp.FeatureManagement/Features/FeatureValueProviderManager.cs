using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace TTWork.Abp.FeatureManagement.Features
{
    public class FeatureValueProviderManager : IFeatureValueProviderManager, ISingletonDependency
    {
        public List<IFeatureValueProvider> Providers => _lazyProviders.Value;
        protected FeatureOptions Options { get; }

        private readonly Lazy<List<IFeatureValueProvider>> _lazyProviders;

        public FeatureValueProviderManager(
            IServiceProvider serviceProvider)
        {
            Options = serviceProvider.GetService<FeatureOptions>();

            _lazyProviders = new Lazy<List<IFeatureValueProvider>>(
                () => Options
                    .ValueProviders
                    .Select(type => serviceProvider.GetRequiredService(type) as IFeatureValueProvider)
                    .ToList(),
                true
            );
        }
    }
}