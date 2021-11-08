using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using TTWork.Abp.FeatureManagement.Domain;
using TTWork.Abp.FeatureManagement.Features;

namespace TTWork.Abp.FeatureManagement.ValueProviders
{
    public class GlobalFeatureValueProvider : FeatureValueProvider
    {
        public const string ProviderName = "G";
        public override string Name => ProviderName;

        public GlobalFeatureValueProvider(
            IRepository<AbpFeature, Guid> featureRepository
        ) : base(featureRepository)
        {
        }

        [UnitOfWork]
        public override async Task<Guid?> GetOrNullAsync(FeatureDefinition audit)
        {
            var dbEntity = await FeatureRepository
                .FirstOrDefaultAsync(x =>
                    x.ProviderName == ProviderName
                    && x.Name == audit.Name
                    && x.Enable);

            return dbEntity?.Id;
        }
    }
}