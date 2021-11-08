using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Editions;

namespace TTWork.Abp.Core.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager,
                featureValueStore)
        {
        }
    }
}
