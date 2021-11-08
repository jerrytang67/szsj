using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Authorization.Roles;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Editions;
using TTWork.Abp.Core.MultiTenancy;
using TtWork.ProjectName.Authorization.Roles;
using TtWork.ProjectName.Authorization.Users;
using TtWork.ProjectName.Identity;

namespace TTWork.Abp.Core.Identity
{
    public static class IdentityRegistrar
    {
        public static IdentityBuilder Register(IServiceCollection services)
        {
            services.AddLogging();

            return services.AddAbpIdentity<Tenant, User, Role>(option =>
                {
                    option.Password.RequireUppercase = false;
                    option.Password.RequiredLength = 6;
                    option.Password.RequireLowercase = false;
                    option.Password.RequireDigit = false;
                    option.Password.RequiredUniqueChars = 0;
                    option.Password.RequireNonAlphanumeric = false;
                })
                .AddAbpTenantManager<TenantManager>()
                .AddAbpUserManager<UserManager>()
                .AddAbpRoleManager<RoleManager>()
                .AddAbpEditionManager<EditionManager>()
                .AddAbpUserStore<UserStore>()
                .AddAbpRoleStore<RoleStore>()
                .AddAbpLogInManager<LogInManager>()
                .AddAbpSignInManager<SignInManager>()
                .AddAbpSecurityStampValidator<SecurityStampValidator>()
                .AddAbpUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddPermissionChecker<PermissionChecker>()
                .AddDefaultTokenProviders();
        }
    }
}
