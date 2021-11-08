using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.UI;
using TtWork.ProjectName.Apis.Dtos;
using TtWork.ProjectName.Apis.Organizations.Dtos;
using TtWork.ProjectName.Authorization;
using TtWork.ProjectName.Authorization.Roles;
using TtWork.ProjectName.Dto;
using TtWork.ProjectName.Entities.Organizations;
using TtWork.ProjectName.Roles.Dto;
using TtWork.ProjectName.Users;
using TtWork.ProjectName.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TT.Extensions;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Authorization.Roles;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Definitions;
using TtWork.ProjectName.Apis.Users.Dto;

namespace TtWork.ProjectName.Apis.Users
{
    [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
    public class UserAppService : AbpAsyncCrudAppService<User, UserDto, long, AppResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<ProjectNameOrganizationUnit, long> _organizationUnitRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;

        private readonly ITenantCache _tenantCache;

        public UserAppService(
            IRepository<User, long> repository,
            IocManager iocManager,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IRepository<ProjectNameOrganizationUnit, long> organizationUnitRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            ITenantCache tenantCache)
            : base(repository, iocManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _tenantCache = tenantCache;
        }

        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        public async Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input)
        {
            //Getting all available roles
            var userRoleDtos = await _roleManager.Roles
                .OrderBy(r => r.DisplayName)
                .Select(r => new UserRoleDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    RoleDisplayName = r.DisplayName
                })
                .ToArrayAsync();

            var allOrganizationUnits = await _organizationUnitRepository.GetAllListAsync();

            var output = new GetUserForEditOutput
            {
                Roles = userRoleDtos,
                AllOrganizationUnits = ObjectMapper.Map<List<OrganizationUnitDto>>(allOrganizationUnits),
                MemberedOrganizationUnits = new List<string>()
            };

            if (!input.Id.HasValue)
            {
                //Creating a new user
                output.User = new UserEditDto
                {
                    IsActive = true,
                    //ShouldChangePasswordOnNextLogin = true,
                    //IsTwoFactorEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled),
                    //IsLockoutEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled)
                };

                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    var defaultUserRole = userRoleDtos.FirstOrDefault(ur => ur.RoleName == defaultRole.Name);
                    if (defaultUserRole != null)
                    {
                        defaultUserRole.IsAssigned = true;
                    }
                }
            }
            else
            {
                //Editing an existing user
                var user = await _userManager.GetUserByIdAsync(input.Id.Value);

                output.User = ObjectMapper.Map<UserEditDto>(user);
                output.HeadImgUrl = user.HeadImgUrl;

                foreach (var userRoleDto in userRoleDtos)
                {
                    userRoleDto.IsAssigned = await _userManager.IsInRoleAsync(user, userRoleDto.RoleName);
                }

                var organizationUnits = await _userManager.GetOrganizationUnitsAsync(user);
                output.MemberedOrganizationUnits = organizationUnits.Select(ou => ou.Code).ToList();
            }

            return output;
        }


        public async Task CreateOrUpdateUser(CreateOrUpdateUserInput input)
        {
            if (input.User.Id.HasValue)
            {
                await UpdateUserAsync(input);
            }
            else
            {
                //await CreateUserAsync(input);
            }
        }

        protected virtual async Task UpdateUserAsync(CreateOrUpdateUserInput input)
        {
            Debug.Assert(input.User.Id != null, "input.User.Id should be set.");

            var user = await _userManager.FindByIdAsync(input.User.Id.Value.ToString());

            //Update user properties
            ObjectMapper.Map(input.User, user); //Passwords is not mapped (see mapping configuration)

            if (!input.User.Password.IsNullOrEmpty())
            {
                await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.User.Password));
            }

            CheckErrors(await _userManager.UpdateAsync(user));

            //Update roles
            CheckErrors(await _userManager.SetRolesAsync(user, input.AssignedRoleNames));

            //update organization units
            //await _userManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

            //if (input.SendActivationEmail)
            //{
            //    user.SetNewEmailConfirmationCode();
            //    await _userEmailer.SendEmailActivationLinkAsync(
            //        user,
            //        AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
            //        input.User.Password
            //    );
            //}
        }


        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(AppResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(),
                    x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, AppResultRequestDto input)
        {
            return query.OrderBy(input.Sorting);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("请先登录");
            }

            long userId = _abpSession.UserId.Value;

            var user = await _userManager.GetUserByIdAsync(userId);

            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, GetTenancyNameOrNull(), shouldLockout: false);

            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("原密码不正确");
            }

            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);

            CurrentUnitOfWork.SaveChanges();
            return true;
        }


        public async Task<string> GetUserLoginKey(long id, ClientTypeEnum clientType = ClientTypeEnum.Weixin)
        {
            var result = await _userManager.GetUserLoginKey(id, clientType);
            return result;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to reset password.");
            }

            long currentUserId = _abpSession.UserId.Value;
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }

            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }


        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }
    }
}