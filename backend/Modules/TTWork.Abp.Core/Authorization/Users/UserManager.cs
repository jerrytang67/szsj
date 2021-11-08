using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Threading;
using Abp.UI;
using Abp.Zero.Configuration;
using TtWork.ProjectName.Authorization.Roles;
using TtWork.ProjectName.Authorization.Users;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TT.Extensions;
using TTWork.Abp.Core.Authorization.Roles;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.Core.Domains.Weixin;
using TTWork.Abp.Core.Security;

namespace TTWork.Abp.Core.Authorization.Users
{
    public class UserManager : AbpUserManager<Role, User>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<UserLogin, long> _userLoginRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationRepository;
        private readonly IRepository<UserRole, long> _userRoleRepostiory;
        private readonly IRepository<Role, int> _roleRepository;

        public UserManager(UserStore userStore,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager> logger,
            RoleManager roleManager,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ISettingManager settingManager,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<User, long> userRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationRepository,
            IRepository<UserRole, long> userRoleRepostiory,
            IRepository<Role, int> roleRepository)
            : base(
                roleManager,
                userStore,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger,
                permissionManager,
                unitOfWorkManager,
                cacheManager,
                organizationUnitRepository,
                userOrganizationUnitRepository,
                organizationUnitSettings,
                settingManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _userLoginRepository = userLoginRepository;
            _userRepository = userRepository;
            _userOrganizationRepository = userOrganizationRepository;
            _userRoleRepostiory = userRoleRepostiory;
            _roleRepository = roleRepository;
        }

        public IQueryable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        [ItemCanBeNull]
        public virtual async Task<string> GetUserLoginKey(long userId, ClientTypeEnum clienType)
        {
            string provider = "";
            switch (clienType)
            {
                case ClientTypeEnum.Weixin:
                    provider = TTWorkConsts.LoginProvider.WeChat;
                    break;
                case ClientTypeEnum.WeixinMini:
                    provider = TTWorkConsts.LoginProvider.WeChatMiniProgram;
                    break;
                case ClientTypeEnum.System:
                    provider = TTWorkConsts.LoginProvider.WeChatUnionId;
                    break;
            }

            if (!provider.IsNullOrEmptyOrWhiteSpace())
            {
                var result = await _userLoginRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.LoginProvider == provider);
                if (result != null)
                {
                    return result.ProviderKey;
                }
            }

            return null;
        }

        [UnitOfWork]
        public virtual async Task<User> GetUserOrNullAsync(UserIdentifier userIdentifier)
        {
            using (_unitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                return await FindByIdAsync(userIdentifier.UserId.ToString());
            }
        }

        public User GetUserOrNull(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserOrNullAsync(userIdentifier));
        }

        public async Task<User> GetUserAsync(UserIdentifier userIdentifier)
        {
            var user = await GetUserOrNullAsync(userIdentifier);
            if (user == null)
            {
                throw new Exception("There is no user: " + userIdentifier);
            }

            return user;
        }

        public User GetUser(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserAsync(userIdentifier));
        }

        [UnitOfWork]
        public override async Task<IdentityResult> SetRolesAsync(User user, string[] roleNames)
        {
            if (user.Name == "admin" && !roleNames.Contains(StaticRoleNames.Host.Admin))
            {
                throw new UserFriendlyException(L("AdminRoleCannotRemoveFromAdminUser"));
            }

            return await base.SetRolesAsync(user, roleNames);
        }

        [UnitOfWork]
        public virtual async Task<User> RegisterUserFromWechat(WechatUserinfo externalUser, int? tenantId, long[] ous = null)
        {
            var loginKey = await _userLoginRepository.FirstOrDefaultAsync(x => x.LoginProvider == TTWorkConsts.LoginProvider.WeChat && x.ProviderKey == externalUser.openid);

            User user = null;

            if (loginKey != null)
                user = await _userRepository.FirstOrDefaultAsync(x => x.Id == loginKey.UserId && x.TenantId == tenantId);

            if (user == null)
            {
                user = new User()
                {
                    UserName = externalUser.openid,
                    Name = externalUser.nickname,
                    Surname = "",
                    TenantId = tenantId,
                    IsEmailConfirmed = false,
                    EmailAddress = externalUser.openid + "@wujiangapp.com",
                    HeadImgUrl = externalUser.headimgurl,
                };
                user.TenantId = AbpSession.TenantId;
                user.IsEmailConfirmed = true;

                await InitializeOptionsAsync(tenantId);

                await CreateAsync(user, User.CreateRandomPassword());
                user.Logins = new List<UserLogin> {new UserLogin {LoginProvider = TTWorkConsts.LoginProvider.WeChat, ProviderKey = externalUser.openid, TenantId = user.TenantId}};
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }

            if (ous != null)
            {
                foreach (var ou in ous)
                {
                    if (!await _userOrganizationRepository.GetAll().AnyAsync(x => x.UserId == user.Id && x.OrganizationUnitId == ou && x.TenantId == tenantId))
                    {
                        await _userOrganizationRepository.InsertAsync(new UserOrganizationUnit(tenantId, user.Id, ou));
                    }

                    var ouRole = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Name == StaticRoleNames.Tenants.Organize);
                    var userRole = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Name == StaticRoleNames.Tenants.User);

                    if (!await _userRoleRepostiory.GetAll().AnyAsync(x => x.UserId == user.Id && x.TenantId == tenantId && x.RoleId == ouRole.Id))
                    {
                        await _userRoleRepostiory.InsertAsync(new UserRole(tenantId, user.Id, ouRole.Id));
                    }

                    if (!await _userRoleRepostiory.GetAll().AnyAsync(x => x.UserId == user.Id && x.TenantId == tenantId && x.RoleId == userRole.Id))
                    {
                        await _userRoleRepostiory.InsertAsync(new UserRole(tenantId, user.Id, userRole.Id));
                    }

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }
            }

            return user;
        }


        public override async Task SetGrantedPermissionsAsync(User user, IEnumerable<Permission> permissions)
        {
            CheckPermissionsToUpdate(user, permissions);
            await base.SetGrantedPermissionsAsync(user, permissions);
        }


        public async Task<string> CreateRandomPassword()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                // RequireDigit = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit),
                // RequireLowercase = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase),
                // RequireNonAlphanumeric = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric),
                // RequireUppercase = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase),
                // RequiredLength = await _settingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength)

                RequireDigit = false,
                RequireLowercase = false,
                RequireNonAlphanumeric = false,
                RequireUppercase = false,
                RequiredLength = 6
            };

            var upperCaseLetters = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            var lowerCaseLetters = "abcdefghijkmnopqrstuvwxyz";
            var digits = "0123456789";
            var nonAlphanumerics = "!@$?_-";

            string[] randomChars =
            {
                upperCaseLetters,
                lowerCaseLetters,
                digits,
                nonAlphanumerics
            };

            var rand = new Random(Environment.TickCount);
            var chars = new List<char>();

            if (passwordComplexitySetting.RequireUppercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    upperCaseLetters[rand.Next(0, upperCaseLetters.Length)]);
            }

            if (passwordComplexitySetting.RequireLowercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    lowerCaseLetters[rand.Next(0, lowerCaseLetters.Length)]);
            }

            if (passwordComplexitySetting.RequireDigit)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    digits[rand.Next(0, digits.Length)]);
            }

            if (passwordComplexitySetting.RequireNonAlphanumeric)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    nonAlphanumerics[rand.Next(0, nonAlphanumerics.Length)]);
            }

            for (var i = chars.Count; i < passwordComplexitySetting.RequiredLength; i++)
            {
                var rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return await Task.FromResult(new string(chars.ToArray()));
        }


        private void CheckPermissionsToUpdate(User user, IEnumerable<Permission> permissions)
        {
            if (user.Name == AbpUserBase.AdminUserName &&
                (!permissions.Any(p => p.Name == AppPermissions.Pages.Administration.Default) ||
                 !permissions.Any(p => p.Name == AppPermissions.Pages.Administration.Default)))
            {
                throw new UserFriendlyException(L("YouCannotRemoveUserRolePermissionsFromAdminUser"));
            }
        }
    }
}