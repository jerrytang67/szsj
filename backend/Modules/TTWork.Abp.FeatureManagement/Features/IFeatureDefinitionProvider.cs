﻿namespace TTWork.Abp.FeatureManagement.Features
{
    public interface IFeatureDefinitionProvider
    {
        void Define(IFeatureDefinitionContext context);
    }
}