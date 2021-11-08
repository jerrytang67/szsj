using Abp.Authorization;
using TTWork.Abp.Core.Authorization.Roles;
using TTWork.Abp.Core.Authorization.Users;

namespace TTWork.Abp.Core.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
