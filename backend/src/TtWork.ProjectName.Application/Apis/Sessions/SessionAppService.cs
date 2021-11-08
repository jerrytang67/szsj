using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.UI;
using TtWork.ProjectName.Apis.Sessions.Dto;
using TtWork.ProjectName.Authorization.Roles;
using TtWork.ProjectName.Extensions;
using TtWork.ProjectName.Sessions;
using TtWork.ProjectName.Sessions.Dto;
using TT.Extensions;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Extensions;

namespace TtWork.ProjectName.Apis.Sessions
{
    public class SessionAppService : AbpAppServiceBase, ISessionAppService
    {
        private readonly RoleManager _roleManage;

        public SessionAppService(RoleManager roleManage)
        {
            _roleManage = roleManage;
        }

        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Name = AppVersionHelper.AppName, Version = AppVersionHelper.Version, ReleaseDate = AppVersionHelper.ReleaseDate, Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                var userDto = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
                if (await IsAdmin())
                    userDto.IsSubscribe = true;
                else
                    userDto.IsSubscribe = !(await UserManager.GetUserLoginKey(userDto.Id, ClientTypeEnum.Weixin)).IsNullOrEmptyOrWhiteSpace();
                output.User = userDto;
            }

            if (AbpSession.UserId.HasValue)
            {
                var roles = await UserManager.GetRolesAsync(await GetCurrentUserAsync());
                foreach (var role in roles)
                {
                    output.Roles.Add(role);
                    var grantedPermissions = (await _roleManage.GetGrantedPermissionsAsync(role)).ToArray();
                    foreach (var p in grantedPermissions)
                    {
                        if (output.Permissions.All(z => z != p.Name))
                        {
                            output.Permissions.Add(p.Name);
                        }
                    }
                }
            }

            if (AbpSession.UserId.HasValue)
            {
                var ouId = AbpSession.Get_OrganizationUnitId();
                if (ouId.HasValue && ouId > 0 && !await IsAdmin())
                {
                    //判断是不是用户的机构
                    var ouList = await UserManager.GetOrganizationUnitsAsync(await GetCurrentUserAsync());
                    var userOuIds = ouList.Select(x => x.Id).ToList();
                    if (!userOuIds.Contains(ouId.Value))
                        throw new UserFriendlyException("你没有管理这个机构的权限");
                }
            }

            return output;
        }
    }
}