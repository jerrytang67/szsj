using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Abp;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TTWork.Abp.AuditManagement.Audits
{
    public class AuditDefinitionManager : IAuditDefinitionManager, ISingletonDependency
    {
        protected Lazy<IDictionary<string, AuditDefinition>> AuditDefinitions { get; }

        protected AuditOptions Options { get; }

        protected IServiceProvider ServiceProvider { get; }


        public AuditDefinitionManager(IServiceProvider serviceProvider)
        {
            // Options = options.Value;
            ServiceProvider = serviceProvider;

            Options = serviceProvider.GetService<AuditOptions>();

            AuditDefinitions = new Lazy<IDictionary<string, AuditDefinition>>(CreateAuditDefinitions, true);
        }

        public AuditDefinition Get(string name)
        {
            Check.NotNull(name, nameof(name));

            var audit = GetOrNull(name);

            if (audit == null)
            {
                throw new AbpException("Undefined setting: " + name);
            }

            return audit;
        }

        public IReadOnlyList<AuditDefinition> GetAll()
        {
            return AuditDefinitions.Value.Values.ToImmutableList();
        }

        public AuditDefinition GetOrNull(string name)
        {
            return AuditDefinitions.Value.GetOrDefault(name);
        }

        protected virtual IDictionary<string, AuditDefinition> CreateAuditDefinitions()
        {
            var audits = new Dictionary<string, AuditDefinition>();

            using var scope = ServiceProvider.CreateScope();
            var providers = Options
                .DefinitionProviders
                .Select(p => scope.ServiceProvider.GetRequiredService(p) as IAuditDefinitionProvider)
                .ToList();

            foreach (var provider in providers)
            {
                provider.Define(new AuditDefinitionContext(audits));
            }

            return audits;
        }
    }
}