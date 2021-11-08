﻿using Abp.Dependency;

namespace TTWork.Abp.FeatureManagement.Features
{
    public abstract class FeatureDefinitionProvider : IFeatureDefinitionProvider, ITransientDependency
    {
        public abstract void Define(IFeatureDefinitionContext context);
    }
}