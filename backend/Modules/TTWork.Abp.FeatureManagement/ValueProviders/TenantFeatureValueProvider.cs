using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using TTWork.Abp.FeatureManagement.Domain;
using TTWork.Abp.FeatureManagement.Features;

namespace TTWork.Abp.FeatureManagement.ValueProviders
{
    public class TenantFeatureValueProvider : FeatureValueProvider
    {
        private readonly IAbpSession _abpSession;
        public const string ProviderName = "T";

        public override string Name => ProviderName;

        public TenantFeatureValueProvider(
            IRepository<AbpFeature, Guid> featureRepository,
            IAbpSession abpSession
        ) : base(featureRepository)
        {
            _abpSession = abpSession;
        }

        [UnitOfWork]
        public override async Task<Guid?> GetOrNullAsync(FeatureDefinition audit)
        {
            if (!_abpSession.TenantId.HasValue)
                return null;

            var tenantId = _abpSession.TenantId.ToString();

            var dbEntity = await FeatureRepository
                .FirstOrDefaultAsync(
                    x => x.ProviderName == ProviderName
                         && x.Name == audit.Name
                         && x.Enable
                         && x.ProviderKey == tenantId
                         && (x.DateTimeExpired >= DateTime.Now || !x.DateTimeExpired.HasValue)
                );

            return dbEntity?.Id;
        }
    }
}