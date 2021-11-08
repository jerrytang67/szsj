using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

namespace TTWork.Abp.FeatureManagement.Domain
{
    public class AbpFeature : FullAuditedAggregateRoot<Guid>
    {
        protected AbpFeature(DateTime? dateTimeExpired)
        {
            DateTimeExpired = dateTimeExpired;
        }

        public AbpFeature(
            [NotNull] string featureName,
            bool enable,
            [NotNull] string providerName,
            [CanBeNull] string providerKey,
            [CanBeNull] string value = null,
            DateTime? dateTimeExpired = null)
        {
            Name = featureName;
            Enable = enable;
            ProviderName = providerName;
            ProviderKey = providerKey;
            Value = value;
            DateTimeExpired = dateTimeExpired;
        }

        [NotNull] [StringLength(64)] public virtual string Name { get; protected set; }

        //This field "G","T","O","A"
        [NotNull] [StringLength(1)] public virtual string ProviderName { get; protected set; }
        [CanBeNull] [StringLength(64)] public virtual string ProviderKey { get; protected set; }

        public virtual bool Enable { get; protected set; }

        public virtual DateTime? DateTimeExpired { get; protected set; }

        public virtual string Value { get; protected set; }
    }
}