using System;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.MultiTenancy;

namespace TTWork.Abp.Core
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class AbpAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }
        public UserManager UserManager { get; set; }

        protected AbpAppServiceBase()
        {
            LocalizationSourceName = AbpConsts.LocalizationSourceName;
            PermissionChecker = NullPermissionChecker.Instance;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected async Task<bool> IsInRole(long userId, string roleName)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var roles = await UserManager.GetRolesAsync(user);
            return roles.Any(x => String.Equals(x, roleName,
                StringComparison.CurrentCultureIgnoreCase));
        }

        protected async Task<bool> IsAdmin()
        {
            if (AbpSession.UserId.HasValue)
                return await IsInRole(AbpSession.UserId.Value, "admin");
            return false;
        }
    }
}