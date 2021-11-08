using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using TTWork.Abp.FeatureManagement.Domain;

namespace TTWork.Abp.FeatureManagement.Features
{
    public interface IAbpFeatureChecker
    {
        Task<bool> IsEnabledAsync(string featureName);
        Task<string> GetValueAsync(string featureName);
        Task<T> GetValueAsync<T>(string featureName);
    }

    public class AbpFeatureChecker : IAbpFeatureChecker, ISingletonDependency
    {
        private readonly IFeatureProvider _provider;
        private readonly IFeatureDefinitionManager _definitionManager;
        private readonly IRepository<AbpFeature, Guid> _repository;

        public AbpFeatureChecker(
            IFeatureProvider provider,
            IFeatureDefinitionManager definitionManager,
            IRepository<AbpFeature, Guid> repository
        )
        {
            _provider = provider;
            _definitionManager = definitionManager;
            _repository = repository;
        }

        public async Task<bool> IsEnabledAsync(string featureName)
        {
            var definition = _definitionManager.Get(featureName);
            if (definition == null)
                throw new NotImplementedException();

            var featureId = await _provider.GetOrNullAsync(featureName);
            if (featureId == null)
                return Convert.ToBoolean(definition.DefaultValue);

            var dbEntity = await _repository.FirstOrDefaultAsync(x => x.Id == featureId);
            if (dbEntity == null)
                return Convert.ToBoolean(definition.DefaultValue);

            return Convert.ToBoolean(dbEntity.Value ?? definition.DefaultValue);
        }

        public async Task<string> GetValueAsync(string featureName)
        {
            var definition = _definitionManager.Get(featureName);
            if (definition == null)
                throw new NotImplementedException();

            var featureId = await _provider.GetOrNullAsync(featureName);
            if (featureId == null)
                return definition.DefaultValue;

            var dbEntity = await _repository.FirstOrDefaultAsync(x => x.Id == featureId);
            if (dbEntity == null)
                return definition.DefaultValue;

            return dbEntity.Value ?? definition.DefaultValue;
        }

        public async Task<T> GetValueAsync<T>(string featureName)
        {
            var result = await GetValueAsync(featureName);
            return (T) Convert.ChangeType(result, typeof(T));
        }
    }
}