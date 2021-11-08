using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Uow;
using TTWork.Abp.Core.Authorization.Roles;
using TtWork.ProjectName.Authorization.Roles;
using TtWork.ProjectName.Authorization.Users;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.MultiTenancy;

namespace TtWork.ProjectName.Identity
{
    public class SignInManager : AbpSignInManager<Tenant, Role, User>
    {
        public SignInManager(
            UserManager userManager,
            IHttpContextAccessor contextAccessor,
            UserClaimsPrincipalFactory claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<User>> logger,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<User> userConfirmation)
            : base(
                userManager,
                contextAccessor,
                claimsFactory,
                optionsAccessor,
                logger,
                unitOfWorkManager,
                settingManager,
                schemes,
                userConfirmation)
        {
        }
    }
}