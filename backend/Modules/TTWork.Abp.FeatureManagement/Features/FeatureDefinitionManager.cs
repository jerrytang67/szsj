using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Abp;
using Abp.Collections.Extensions;
using Abp.Dependency;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace TTWork.Abp.FeatureManagement.Features
{
    public interface IFeatureDefinitionManager
    {
        FeatureDefinition Get([NotNull] string name);

        IReadOnlyList<FeatureDefinition> GetAll();

        FeatureDefinition GetOrNull(string name);
    }


    public class FeatureDefinitionManager : IFeatureDefinitionManager, ISingletonDependency
    {
        protected Lazy<IDictionary<string, FeatureDefinition>> Definitions { get; }

        protected FeatureOptions Options { get; }

        protected IServiceProvider ServiceProvider { get; }


        public FeatureDefinitionManager(IServiceProvider serviceProvider)
        {
            // Options = options.Value;
            ServiceProvider = serviceProvider;

            Options = serviceProvider.GetService<FeatureOptions>();

            Definitions = new Lazy<IDictionary<string, FeatureDefinition>>(CreateAuditDefinitions, true);
        }

        public FeatureDefinition Get(string name)
        {
            Check.NotNull(name, nameof(name));

            var audit = GetOrNull(name);

            if (audit == null)
            {
                throw new AbpException("Undefined setting: " + name);
            }

            return audit;
        }

        public IReadOnlyList<FeatureDefinition> GetAll()
        {
            return Definitions.Value.Values.ToImmutableList();
        }

        public FeatureDefinition GetOrNull(string name)
        {
            return Definitions.Value.GetOrDefault(name);
        }

        protected virtual IDictionary<string, FeatureDefinition> CreateAuditDefinitions()
        {
            var features = new Dictionary<string, FeatureDefinition>();

            using var scope = ServiceProvider.CreateScope();
            var providers = Options
                .DefinitionProviders
                .Select(p => scope.ServiceProvider.GetRequiredService(p) as IFeatureDefinitionProvider)
                .ToList();

            foreach (var provider in providers)
            {
                provider.Define(new FeatureDefinitionContext(features));
            }

            return features;
        }
    }
}