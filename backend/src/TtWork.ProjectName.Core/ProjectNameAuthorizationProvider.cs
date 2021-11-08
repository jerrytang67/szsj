using Abp.Authorization;
using Abp.Localization;
using TTWork.Abp.Core.Authorization;

namespace TtWork.ProjectName
{
    public class ProjectNameAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // var pages = context.GetPermissionOrNull(AppPermissions.Pages.Default) ?? context.CreatePermission(AppPermissions.Pages.Default, L("Pages"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ProjectNameConsts.LocalizationSourceName);
        }
    }
}