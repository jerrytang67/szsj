using Abp.Authorization;
using Abp.Localization;

namespace TTWork.Abp.Core.Definitions
{
    public class AbpAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            
            var pages = context.GetPermissionOrNull(AppPermissions.Pages.Default) ?? context.CreatePermission(AppPermissions.Pages.Default, L("Permission:Pages"));
            
            var dashboard = pages.CreateChildPermission(AppPermissions.Pages.Dashboard.Default, L("Permission:Dashboard"));
            
            var administration = pages.CreateChildPermission(AppPermissions.Pages.Administration.Default, L("Permission:Administration"));
            
            var organization = pages.CreateChildPermission(AppPermissions.Pages.Organization.Default, L("Permission:Organization"));
            organization.CreateChildPermission(AppPermissions.Pages.Organization.Administration, L("Permission:Organization:Administration"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TTWorkConsts.LocalizationSourceName);
        }
    }

    public static class AppPermissions
    {
        public class Pages
        {
            public const string Default = "Pages";

            public class Dashboard
            {
                public const string Default = "Pages.Dashboard";
            }

            public class Administration
            {
                public const string Default = "Pages.Administration";
            }

            public class Organization
            {
                public const string Default = "Pages.Organization";
                public const string Administration = "Pages.Organization.Administration";
            }
        }
    }
}