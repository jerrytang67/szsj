using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Abp.WebApi.Controllers;
using Microsoft.AspNetCore.Identity;

namespace TtWork.ProjectName.Controllers
{
    public abstract class AbpControllerBase: AbpController
    {
        protected AbpControllerBase()
        {
            LocalizationSourceName = ProjectNameConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
