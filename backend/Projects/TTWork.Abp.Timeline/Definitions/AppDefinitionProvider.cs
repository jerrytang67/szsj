using Abp.Localization;
using TTWork.Abp.AppManagement.Apps;

namespace TTWork.Abp.Timeline.Definitions
{
    public class TimelineAppDefinitionProvider : AppDefinitionProvider
    {
        public override void Define(IAppDefinitionContext context)
        {
            // context.Add(new AppDefinition(ProjectApp.ZGH_MINI,
            //         ProjectApp.ZGH_MINI,
            //         "小程序",
            //         null,
            //         new LocalizableString(ProjectApp.ZGH_MINI, TimelineConsts.LocalizationSourceName)
            //     )
            // );
        }
    }

    // public static class ProjectApp
    // {
    //     public const string ZGH_MINI = "ZGH_MINI";
    // }
}