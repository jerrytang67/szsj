using System;
using System.Threading.Tasks;

namespace TTWork.Abp.FeatureManagement.Features
{
    public interface IFeatureProvider
    {
        Task<Guid?> GetOrNullAsync(string name);
    }
}