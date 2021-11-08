using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Mvc;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization.Users;
using TtWork.ProjectName.Apis.Authorization.Accounts.Dto;
using TtWork.ProjectName.Authorization.Accounts;
using TtWork.ProjectName.Authorization.Accounts.Dto;
using TtWork.ProjectName.Users.Dto;

namespace TtWork.ProjectName.Apis.Authorization.Accounts
{
    public class AccountAppService : AbpAppServiceBase, IAccountAppService
    {
        // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

        private readonly UserRegistrationManager _userRegistrationManager;
        public AccountAppService(UserRegistrationManager userRegistrationManager) { _userRegistrationManager = userRegistrationManager; }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                input.PhoneNumber,
                true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
            );

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput {CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)};
        }


        [HttpPost]
        public async Task<UserDto> UpdatePhone([FromBody] string phone)
        {
            var user = await GetCurrentUserAsync();
            CheckErrors(await UserManager.SetPhoneNumberAsync(user, phone));
            CheckErrors(await UserManager.SetUserNameAsync(user, phone));
            //TODO:暂时为密码自动修改为手机号码后6位
            CheckErrors(await UserManager.ChangePasswordAsync(user, Regex.Matches(phone, @"\d+(\d{6})\b")[0].Groups[1].Value));
            await CurrentUnitOfWork.SaveChangesAsync();
            var roles = await UserManager.GetRolesAsync(user);
            var userDto = ObjectMapper.Map<UserDto>(user);
            userDto.RoleNames = roles.ToArray();
            //var roles = await _roleManager.Roles.Where(r => roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.NormalizedName).ToListAsync();
            return userDto;
        }
    }
}