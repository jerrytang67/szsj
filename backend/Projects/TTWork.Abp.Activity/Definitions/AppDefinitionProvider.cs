using Abp.Localization;
using TTWork.Abp.AppManagement.Apps;
using TTWork.Abp.LaborUnion;

namespace TTWork.Abp.Activity.Definitions
{
    public class ActivityAppDefinitionProvider : AppDefinitionProvider
    {
        public override void Define(IAppDefinitionContext context)
        {
            context.Add(new AppDefinition(ProjectApp.SZMLSJ_MINI,
                    ProjectApp.SZMLSJ_MINI,
                    "小程序",
                    null,
                    new LocalizableString(ProjectApp.SZMLSJ_MINI, ActivityConsts.LocalizationSourceName)
                )
            );
        }
    }

    public static class ProjectApp
    {
        public const string SZMLSJ_MINI = "SZMLSJ_MINI";
    }
}