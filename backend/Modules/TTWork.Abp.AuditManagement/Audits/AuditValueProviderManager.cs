using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace TTWork.Abp.AuditManagement.Audits
{
    public class AuditValueProviderManager : IAuditValueProviderManager, ISingletonDependency
    {
        public List<IAuditValueProvider> Providers => _lazyProviders.Value;
        protected AuditOptions Options { get; }

        private readonly Lazy<List<IAuditValueProvider>> _lazyProviders;

        public AuditValueProviderManager(
            IServiceProvider serviceProvider)
        {
            Options = serviceProvider.GetService<AuditOptions>();


            _lazyProviders = new Lazy<List<IAuditValueProvider>>(
                () => Options
                    .ValueProviders
                    .Select(type => serviceProvider.GetRequiredService(type) as IAuditValueProvider)
                    .ToList(),
                true
            );
        }
    }
}