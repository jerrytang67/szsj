using Abp.Authorization;
using Abp.Localization;
using TTWork.Abp.Core.Definitions;

namespace TTWork.Abp.LaborUnion.Definitions
{
    public class LaborUnionAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull(AppPermissions.Pages.Default);
            if (pages == null) return;

            var riddle = pages.CreateChildPermission(LaborUnionPermissions.Default, L($"Permission:{LaborUnionPermissions.Default}"));
            riddle.CreateChildPermission(LaborUnionPermissions.Admin, L($"Permission:{LaborUnionPermissions.Admin}"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LaborUnionConsts.LocalizationSourceName);
        }
    }

    public static class LaborUnionPermissions
    {
        public const string Default = "LaborUnion.Default";
        public const string Admin = "LaborUnion.Admin";
    }
}