using Abp.Authorization;
using Abp.Localization;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.LaborUnion;

namespace TTWork.Abp.QA.Definitions
{
    // ReSharper disable once InconsistentNaming
    public class QAAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull(AppPermissions.Pages.Default);
            if (pages == null) return;

            var riddle = pages.CreateChildPermission(QAPermissions.Default, L($"Permission:{QAPermissions.Default}"));
            riddle.CreateChildPermission(QAPermissions.Admin, L($"Permission:{QAPermissions.Admin}"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, QAConsts.LocalizationSourceName);
        }
    }

    // ReSharper disable once InconsistentNaming
    public static class QAPermissions
    {
        public const string Default = "QA.Default";
        public const string Admin = "QA.Admin";
    }
}