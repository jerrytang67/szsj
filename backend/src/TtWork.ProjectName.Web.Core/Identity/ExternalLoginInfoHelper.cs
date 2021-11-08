using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Abp.Authorization.Users;
using Abp.Extensions;
using TT.Extensions;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.MultiTenancy;
using TtWork.ProjectName.Authentication.External;

namespace TtWork.ProjectName.Identity
{
    public static class ExternalLoginInfoHelper
    {
        public static void AddWeixinClaim(this AbpLoginResult<Tenant, User> loginResult, ExternalAuthUserInfo externalUser, int fromClient)
        {
            loginResult.User.FromClient = fromClient;

            #region 把微信的openid和unionid加入到jwt token

            if (!externalUser.WeChatUserLogin!.openid.IsNullOrEmptyOrWhiteSpace())
                loginResult.Identity.AddClaim(new Claim("openid", externalUser.WeChatUserLogin!.openid));

            if (!externalUser.WeChatUserLogin!.nickname.IsNullOrEmptyOrWhiteSpace())
            {
                loginResult.User.Name = externalUser.WeChatUserLogin?.nickname;
                loginResult.Identity.AddClaim(new Claim("nickname", externalUser.WeChatUserLogin!.nickname));
            }

            if (!externalUser.WeChatUserLogin!.headimgurl.IsNullOrEmptyOrWhiteSpace())
            {
                loginResult.User.HeadImgUrl = externalUser.WeChatUserLogin?.headimgurl;
                loginResult.Identity.AddClaim(new Claim("headimgurl", externalUser.WeChatUserLogin!.headimgurl));
            }

            if (externalUser.WeChatUserLogin?.unionid != null)
                loginResult.Identity.AddClaim(new Claim("unionid", externalUser.WeChatUserLogin!.unionid));

            if (externalUser.WeChatUserLogin?.phoneNumber != null)
            {
                if (loginResult.User.PhoneNumber.IsNullOrEmptyOrWhiteSpace())
                    loginResult.User.PhoneNumber = externalUser.WeChatUserLogin?.phoneNumber;
                loginResult.Identity.AddClaim(new Claim("phoneNumber", externalUser.WeChatUserLogin!.phoneNumber));
            }

            #endregion
        }

        public static (string name, string surname) GetNameAndSurnameFromClaims(List<Claim> claims)
        {
            string name = null;
            string surname = null;

            var givennameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            if (givennameClaim != null && !givennameClaim.Value.IsNullOrEmpty())
            {
                name = givennameClaim.Value;
            }

            var surnameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
            if (surnameClaim != null && !surnameClaim.Value.IsNullOrEmpty())
            {
                surname = surnameClaim.Value;
            }

            if (name == null || surname == null)
            {
                var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (nameClaim != null)
                {
                    var nameSurName = nameClaim.Value;
                    if (!nameSurName.IsNullOrEmpty())
                    {
                        var lastSpaceIndex = nameSurName.LastIndexOf(' ');
                        if (lastSpaceIndex < 1 || lastSpaceIndex > (nameSurName.Length - 2))
                        {
                            name = surname = nameSurName;
                        }
                        else
                        {
                            name = nameSurName.Substring(0, lastSpaceIndex);
                            surname = nameSurName.Substring(lastSpaceIndex);
                        }
                    }
                }
            }

            return (name, surname);
        }
    }
}