using System;
using JetBrains.Annotations;

namespace TTWork.Abp.Core.Organizations
{
    public interface ICurrentOrganization
    {
        bool IsAvailable { get; }

        [CanBeNull] long? Id { get; }

        [CanBeNull] string DisplayName { get; }

        IDisposable Change(long? id, string displayName = null);
    }
}