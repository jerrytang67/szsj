using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace TTWork.Abp.FeatureManagement.Features
{
    public interface IFeatureValueProvider
    {
        string Name { get; }

        Task<Guid?> GetOrNullAsync([NotNull] FeatureDefinition audit);
    }
}