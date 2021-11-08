using Abp.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TTWork.Abp.Core.Authorization.Roles;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.MultiTenancy;
using TtWork.ProjectName.Identity;

namespace TTWork.Abp.Core.Identity
{
    public class SecurityStampValidator : AbpSecurityStampValidator<Tenant, Role, User>
    {
        public SecurityStampValidator(
            IOptions<SecurityStampValidatorOptions> options,
            SignInManager signInManager,
            ISystemClock systemClock,
            ILoggerFactory loggerFactory)
            : base(
                options,
                signInManager,
                systemClock,
                loggerFactory)
        {
        }
    }
}