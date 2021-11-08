using Abp.Authorization;
using Abp.Localization;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.LaborUnion;

namespace TTWork.Abp.Activity.Definitions
{
    public class ActivityAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull(AppPermissions.Pages.Default);
            if (pages == null) return;

            var riddle = pages.CreateChildPermission(ActivityPermissions.Default, L($"Permission:{ActivityPermissions.Default}"));
            riddle.CreateChildPermission(ActivityPermissions.Admin, L($"Permission:{ActivityPermissions.Admin}"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ActivityConsts.LocalizationSourceName);
        }
    }

    public static class ActivityPermissions
    {
        public const string Default = "Activity.Default";
        public const string Admin = "Activity.Admin";
    }
}