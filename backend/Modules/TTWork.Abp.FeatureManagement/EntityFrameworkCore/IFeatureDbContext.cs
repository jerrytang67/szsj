using Microsoft.EntityFrameworkCore;
using TTWork.Abp.FeatureManagement.Domain;

namespace TTWork.Abp.FeatureManagement.EntityFrameworkCore
{
    public interface IFeatureDbContext
    {
        public DbSet<AbpFeature> AbpFeatures { get; set; }
    }
}