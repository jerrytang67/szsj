using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using TTWork.Abp.Core.Organizations;
using TTWork.Abp.FeatureManagement.Domain;
using TTWork.Abp.FeatureManagement.Features;

namespace TTWork.Abp.FeatureManagement.ValueProviders
{
    public class OrganizationFeatureValueProvider : FeatureValueProvider
    {
        public ICurrentOrganization CurrentOrganization { get; }

        public const string ProviderName = "O";

        public override string Name => ProviderName;

        public OrganizationFeatureValueProvider(
            IRepository<AbpFeature, Guid> featureRepository,
            ICurrentOrganization currentOrganization
        ) : base(featureRepository)
        {
            CurrentOrganization = currentOrganization;
        }

        [UnitOfWork]
        public override async Task<Guid?> GetOrNullAsync(FeatureDefinition audit)
        {
            if (!CurrentOrganization.Id.HasValue)
                return null;

            var dbEntity = await FeatureRepository
                .FirstOrDefaultAsync(
                    x => x.ProviderName == ProviderName
                         && x.Name == audit.Name
                         && x.Enable
                         && x.ProviderKey == CurrentOrganization.Id.ToString()
                         && (x.DateTimeExpired >= DateTime.Now || !x.DateTimeExpired.HasValue)
                );

            return dbEntity?.Id;
        }
    }
}