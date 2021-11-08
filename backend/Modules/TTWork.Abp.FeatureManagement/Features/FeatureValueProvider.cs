using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using TTWork.Abp.FeatureManagement.Domain;

namespace TTWork.Abp.FeatureManagement.Features
{
    public abstract class FeatureValueProvider : IFeatureValueProvider, ITransientDependency
    {
        protected IRepository<AbpFeature, Guid> FeatureRepository { get; }
        public abstract string Name { get; }

        protected FeatureValueProvider(IRepository<AbpFeature, Guid> featureRepository)
        {
            FeatureRepository = featureRepository;
        }

        public abstract Task<Guid?> GetOrNullAsync(FeatureDefinition audit);
    }
}