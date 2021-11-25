using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using TTWork.Abp.Core.Authorization.Roles;
using TtWork.ProjectName.Authorization.Roles;
using TTWork.Abp.Core.Authorization.Users;
using Abp.Domain.Uow;

namespace TtWork.ProjectName.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor, unitOfWorkManager)
        {
        }
    }
}
