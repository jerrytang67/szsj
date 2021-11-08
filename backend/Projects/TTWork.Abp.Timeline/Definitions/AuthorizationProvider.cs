using Abp.Authorization;
using Abp.Localization;
using TTWork.Abp.Core.Definitions;

namespace TTWork.Abp.Timeline.Definitions
{
    public class TimelineAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull(AppPermissions.Pages.Default);
            if (pages == null) return;

            var riddle = pages.CreateChildPermission(TimelinePermissions.Default, L($"Permission:{TimelinePermissions.Default}"));
            riddle.CreateChildPermission(TimelinePermissions.Admin, L($"Permission:{TimelinePermissions.Admin}"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TimelineConsts.LocalizationSourceName);
        }
    }

    public static class TimelinePermissions
    {
        public const string Default = "Timeline.Default";
        public const string Admin = "Timeline.Admin";
    }
}