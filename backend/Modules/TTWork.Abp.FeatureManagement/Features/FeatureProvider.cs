using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;

namespace TTWork.Abp.FeatureManagement.Features
{
    public class FeatureProvider : IFeatureProvider, ITransientDependency
    {
        protected IFeatureDefinitionManager DefinitionManager { get; }
        protected IFeatureValueProviderManager ValueProviderManager { get; }

        public FeatureProvider(
            IFeatureDefinitionManager definitionManager,
            IFeatureValueProviderManager valueProviderManager
        )
        {
            DefinitionManager = definitionManager;
            ValueProviderManager = valueProviderManager;
        }


        public virtual async Task<Guid?> GetOrNullAsync(string name)
        {
            var audit = DefinitionManager.Get(name);

            var providers = Enumerable
                .Reverse(ValueProviderManager.Providers);

            if (audit.Providers.Any())
            {
                providers = providers.Where(p => audit.Providers.Contains(p.Name));
            }

            var value = await GetOrNullValueFromProvidersAsync(providers, audit);
            return value;
        }

        protected virtual async Task<Guid?> GetOrNullValueFromProvidersAsync(
            IEnumerable<IFeatureValueProvider> providers,
            FeatureDefinition audit)
        {
            foreach (var provider in providers)
            {
                var value = await provider.GetOrNullAsync(audit);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }
    }
}