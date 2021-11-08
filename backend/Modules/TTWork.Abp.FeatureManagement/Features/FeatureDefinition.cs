using System;
using System.Collections.Generic;
using Abp.Collections.Extensions;
using Abp.Localization;

namespace TTWork.Abp.FeatureManagement.Features
{
    public class FeatureDefinition
    {
        public FeatureDefinition(string name, ILocalizableString displayName = null, string defaultValue = null)
        {
            Name = name;
            DisplayName = displayName;
            Providers = new List<string>();
            DefaultValue = defaultValue;
        }

        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public List<string> Providers { get; set; }

        public ILocalizableString DisplayName
        {
            get => _displayName;
            set => _displayName = value;
        }

        private ILocalizableString _displayName;

        /// <summary>
        /// Sets a property in the <see cref="Properties"/> dictionary.
        /// This is a shortcut for nested calls on this object.
        /// </summary>
        public virtual FeatureDefinition WithProviders(params string[] providers)
        {
            if (!providers.IsNullOrEmpty())
            {
                Providers.AddRange(providers);
            }

            return this;
        }
    }
}