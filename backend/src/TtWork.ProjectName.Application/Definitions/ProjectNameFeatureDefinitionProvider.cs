using Abp.Localization;
using TTWork.Abp.FeatureManagement.Features;

namespace TtWork.ProjectName.Definitions
{
    public class ProjectNameFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            context.Add(
                new FeatureDefinition(ProjectNameFeature.Activity.Fassion.Enable,
                    new LocalizableString(ProjectNameFeature.Activity.Fassion.Enable, ProjectNameConsts.LocalizationSourceName)
                    , "false"
                ).WithProviders("G", "T", "O"));

            context.Add(
                new FeatureDefinition(ProjectNameFeature.Organization.BgUrlEnable,
                    new LocalizableString(ProjectNameFeature.Organization.BgUrlEnable, ProjectNameConsts.LocalizationSourceName)
                    , "false"
                ).WithProviders("G", "T", "O"));
        }
    }

    public class ProjectNameFeature
    {
        public class Activity
        {
            public class Fassion
            {
                //裂变海报功能
                public const string Enable = "Feature_Activity_Fassion_Enable";
            }
        }

        public class Organization
        {
            //机构首页自定义背景
            public const string BgUrlEnable = "Feature_Organization_BgUrlEnable";
        }
    }
}