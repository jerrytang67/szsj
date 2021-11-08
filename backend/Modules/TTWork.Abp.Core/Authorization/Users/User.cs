using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.Timing;

namespace TTWork.Abp.Core.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";
        [StringLength(256)] public virtual string HeadImgUrl { get; set; }

        [StringLength(32)] public virtual string Town { get; set; }
        public virtual int Jf { get; set; } = 0;
        public virtual int FromClient { get; set; } = 0; // 0:normal   1:mini  2:wechat

        public virtual List<UserOrganizationUnit> OrganizationUnits { get; set; }

        public DateTime? SignInTokenExpireTimeUtc { get; set; }

        public string SignInToken { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }


        public override void SetNewPasswordResetCode()
        {
            /* This reset code is intentionally kept short.
             * It should be short and easy to enter in a mobile application, where user can not click a link.
             */
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(10).ToUpperInvariant();
        }

        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
        }

        public void SetSignInToken()
        {
            SignInToken = Guid.NewGuid().ToString();
            SignInTokenExpireTimeUtc = Clock.Now.AddMinutes(1).ToUniversalTime();
        }
    }
}