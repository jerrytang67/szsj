using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using TTWork.Abp.Core.Authorization.Roles;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.MultiTenancy;
using TtWork.ProjectName.Authorization.Roles;
using TtWork.ProjectName.Authorization.Users;

namespace TTWork.Abp.Core.Authorization
{
    public class LogInManager : AbpLogInManager<Tenant, Role, User>
    {
        public LogInManager(
            UserManager userManager, 
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager, 
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository, 
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            IPasswordHasher<User> passwordHasher, 
            RoleManager roleManager,
            UserClaimsPrincipalFactory claimsPrincipalFactory) 
            : base(
                  userManager, 
                  multiTenancyConfig,
                  tenantRepository, 
                  unitOfWorkManager, 
                  settingManager, 
                  userLoginAttemptRepository, 
                  userManagementConfig, 
                  iocResolver, 
                  passwordHasher, 
                  roleManager, 
                  claimsPrincipalFactory)
        {
        }
    }
}
