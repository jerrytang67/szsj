using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using TtWork.ProjectName.Authentication.External;
using TtWork.ProjectName.Authentication.JwtBearer;
using TtWork.ProjectName.Common;
using TtWork.ProjectName.Configuration;
using TtWork.ProjectName.Debugging;
using TtWork.ProjectName.Models.TokenAuth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TT.Extensions;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Extensions.Weixin;
using TTWork.Abp.Core.MultiTenancy;
using TTWork.Abp.Core.Security;
using TtWork.ProjectName.Apis.Authorization;
using TtWork.ProjectName.Authorization;
using TtWork.ProjectName.Extensions.Weixin;
using TtWork.ProjectName.Identity;

namespace TtWork.ProjectName.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : AbpControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly ICacheManager _cacheManager;
        private readonly UserManager _userManager;
        private readonly IdentityOptions _identityOptions;
        private readonly ISettingManager _settingManager;
        private readonly IRepository<UserLogin, long> _userLoginRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly UserRegistrationManager _userRegistrationManager;

        public IRecaptchaValidator RecaptchaValidator { get; set; }

        public TokenAuthController(
            LogInManager logInManager,
            ITenantCache tenantCache,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager,
            ICacheManager cacheManager,
            UserManager userManager,
            UserRegistrationManager userRegistrationManager,
            ISettingManager settingManager,
            IOptions<IdentityOptions> identityOptions,
            IRepository<UserLogin, long> userLoginRepository, IRepository<User, long> userRepository)
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userManager = userManager;
            _userRegistrationManager = userRegistrationManager;
            _settingManager = settingManager;
            _userLoginRepository = userLoginRepository;
            _userRepository = userRepository;
            _cacheManager = cacheManager;
            _identityOptions = identityOptions.Value;

            RecaptchaValidator = NullRecaptchaValidator.Instance;
        }

        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            if (UseCaptchaOnLogin())
            {
                await ValidateReCaptcha(model.CaptchaResponse);
            }

            var loginResult = await GetLoginResultAsync(
                model.UserNameOrEmailAddress,
                model.Password,
                GetTenancyNameOrNull()
            );

            var returnUrl = model.ReturnUrl;

            if (model.SingleSignIn.HasValue && model.SingleSignIn.Value && loginResult.Result == AbpLoginResultType.Success)
            {
                loginResult.User.SetSignInToken();
                returnUrl = AddSingleSignInParametersToReturnUrl(model.ReturnUrl, loginResult.User.SignInToken, loginResult.User.Id, loginResult.User.TenantId);
            }

            await _userManager.InitializeOptionsAsync(loginResult.Tenant?.Id);

            string twoFactorRememberClientToken = null;

            var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
            var refreshToken = CreateRefreshToken(await CreateJwtClaims(loginResult.Identity, loginResult.User, tokenType: TokenType.RefreshToken));

            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                ExpireInSeconds = (int) _configuration.AccessTokenExpiration.TotalSeconds,
                RefreshToken = refreshToken,
                RefreshTokenExpireInSeconds = (int) _configuration.RefreshTokenExpiration.TotalSeconds,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                TwoFactorRememberClientToken = twoFactorRememberClientToken,
                UserId = loginResult.User.Id,
                ReturnUrl = returnUrl
            };
        }

        private bool UseCaptchaOnLogin()
        {
            if (DebugHelper.IsDebug)
            {
                return false;
            }

            try
            {
                return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnLogin);
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        {
            return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        }

        [HttpPost]
        public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
        {
            var externalUser = await GetExternalUserInfo(model);

            var loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                {
                    var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
                    var refreshToken = CreateRefreshToken(await CreateJwtClaims(loginResult.Identity, loginResult.User, tokenType: TokenType.RefreshToken));
                    var returnUrl = model.ReturnUrl;

                    return new ExternalAuthenticateResultModel
                    {
                        AccessToken = accessToken,
                        EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                        ExpireInSeconds = (int) _configuration.AccessTokenExpiration.TotalSeconds,
                        ReturnUrl = returnUrl,
                        RefreshToken = refreshToken,
                        RefreshTokenExpireInSeconds = (int) _configuration.RefreshTokenExpiration.TotalSeconds
                    };
                }
                case AbpLoginResultType.UnknownExternalLogin:
                {
                    var newUser = await RegisterExternalUserAsync(externalUser);
                    if (!newUser.IsActive)
                    {
                        return new ExternalAuthenticateResultModel {WaitingForActivation = true};
                    }

                    //Try to login again with newly registered user!
                    loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());
                    if (loginResult.Result != AbpLoginResultType.Success)
                    {
                        throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                            loginResult.Result,
                            model.ProviderKey,
                            GetTenancyNameOrNull()
                        );
                    }

                    var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
                    var refreshToken = CreateRefreshToken(await CreateJwtClaims(loginResult.Identity, loginResult.User, tokenType: TokenType.RefreshToken));

                    return new ExternalAuthenticateResultModel
                    {
                        AccessToken = accessToken,
                        EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                        ExpireInSeconds = (int) _configuration.AccessTokenExpiration.TotalSeconds,
                        RefreshToken = refreshToken,
                        RefreshTokenExpireInSeconds = (int) _configuration.RefreshTokenExpiration.TotalSeconds
                    };
                }
                default:
                {
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                        loginResult.Result,
                        model.ProviderKey,
                        GetTenancyNameOrNull()
                    );
                }
            }
        }


        [HttpPost]
        public async Task<object> GetWeixinMiniPhone([FromBody] WeChatMiniProgramAuthenticateModel model)
        {
            try
            {
                var jsonStr = Encryption.AES_decrypt(model.encryptedData, model.session_key, model.iv);
                //TODO:验证这里的JOSN是不是正确的号码格式
                return await Task.FromResult(JsonConvert.DeserializeObject(jsonStr));
            }
            catch (Exception)
            {
                throw new UserFriendlyException("长时间未登录,登录状态已过期,请重新登录");
                // throw new UserFriendlyException(e.Message);
            }
        }


        /// <summary>
        /// 公众号微信授权登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="???"></exception>
        [HttpGet]
        public async Task<ExternalAuthenticateResultModel> WeixinAuthenticate(string code)
        {
            var model = new ExternalAuthenticateModel {AuthProvider = TTWorkConsts.LoginProvider.WeChat, ProviderAccessCode = code};
            var (externalUser, loginResult) = await GetExternalAuthUserInfo(model);
            return await ExternalAuthenticateResultModel(loginResult, externalUser, model, (int) ClientTypeEnum.Weixin);
        }

        /// <summary>
        /// 小程序微信授权登录
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        /// <exception cref="???"></exception>
        [HttpPost]
        public async Task<ExternalAuthenticateResultModel> WeixinMiniAuthenticate([FromBody] WeChatMiniProgramAuthenticateModel loginModel)
        {
            var model = new ExternalAuthenticateModel {AuthProvider = TTWorkConsts.LoginProvider.WeChatMiniProgram, ProviderAccessCode = JsonConvert.SerializeObject(loginModel)};
            var (externalUser, loginResult) = await GetExternalAuthUserInfo(model);
            model.ProviderKey = externalUser.WeChatUserLogin.openid;
            return await ExternalAuthenticateResultModel(loginResult, externalUser, model, (int) ClientTypeEnum.WeixinMini);
        }

        [HttpPost]
        public async Task<ExternalAuthenticateResultModel> WeixinMiniPhoneAuthenticate([FromBody] WeChatMiniProgramAuthenticateModel loginModel)
        {
            try
            {
                var jsonStr = Encryption.AES_decrypt(loginModel.encryptedData, loginModel.session_key, loginModel.iv);

                var phoneResult = JsonConvert.DeserializeObject<GetPhoneInfo>(jsonStr);

                if (phoneResult == null || phoneResult.phoneNumber.IsNullOrEmptyOrWhiteSpace())
                    throw new UserFriendlyException("获取手机号失败,请重新打开小程序试试");

                var authUserInfo = new ExternalAuthUserInfo
                {
                    ProviderKey = phoneResult.phoneNumber,
                    UserName = phoneResult.phoneNumber,
                    Name = phoneResult.phoneNumber,
                    Surname = "",
                    EmailAddress = phoneResult.phoneNumber + "@wujiangapp.com",
                    WeChatUserLogin = new WeChatUserLoginModel
                    {
                        openid = loginModel.openid,
                        unionid = loginModel.unionid,
                        session_key = loginModel.session_key,
                        phoneNumber = phoneResult.phoneNumber
                    },
                    Provider = TTWorkConsts.LoginProvider.WeChatMiniPhone,
                    FromClient = (int) ClientTypeEnum.WeixinMini,
                    PhoneNumber = phoneResult.phoneNumber
                };

                var model = new ExternalAuthenticateModel {AuthProvider = TTWorkConsts.LoginProvider.WeChatMiniPhone, ProviderKey = phoneResult.phoneNumber};

                var (externalUser, loginResult) = await ExternalLogin(model, authUserInfo);

                return await ExternalAuthenticateResultModel(loginResult, externalUser, model, (int) ClientTypeEnum.WeixinMini);
            }
            catch (Exception e)
            {
                if (e is UserFriendlyException)
                    throw new UserFriendlyException(e.Message);
                throw new UserFriendlyException("登录状态已过期,请重新登录");
            }
        }

        private async Task<(ExternalAuthUserInfo, AbpLoginResult<Tenant, User>)> GetExternalAuthUserInfo(ExternalAuthenticateModel model)
        {
            var externalUser = await GetExternalUserInfo(model);
            return await ExternalLogin(model, externalUser);
        }

        private async Task<(ExternalAuthUserInfo, AbpLoginResult<Tenant, User>)> ExternalLogin(ExternalAuthenticateModel model, ExternalAuthUserInfo externalUser)
        {
            var loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());

            try
            {
                //如果openid登录不成功,查找有没有unionid
                if (externalUser.WeChatUserLogin?.unionid != null && loginResult.Result != AbpLoginResultType.Success)
                {
                    loginResult = await _logInManager.LoginAsync(
                        new UserLoginInfo(TTWorkConsts.LoginProvider.WeChatUnionId, externalUser.WeChatUserLogin.unionid, TTWorkConsts.LoginProvider.WeChatUnionId),
                        GetTenancyNameOrNull());

                    if (loginResult.Result == AbpLoginResultType.Success)
                    {
                        //插入openid登录信息
                        if (!externalUser.WeChatUserLogin.openid.IsNullOrEmptyOrWhiteSpace() && !await _userLoginRepository.GetAll().AsNoTracking().AnyAsync(x =>
                            x.LoginProvider == TTWorkConsts.LoginProvider.WeChatMiniProgram &&
                            x.UserId == loginResult.User.Id &&
                            x.ProviderKey == externalUser.WeChatUserLogin.openid)
                        )
                        {
                            await _userLoginRepository.InsertAsync(new UserLogin
                            {
                                LoginProvider = TTWorkConsts.LoginProvider.WeChatMiniProgram,
                                UserId = loginResult.User.Id,
                                ProviderKey = externalUser.WeChatUserLogin.openid
                            });
                        }

                        //插入手机登录信息
                        if (!externalUser.WeChatUserLogin.phoneNumber.IsNullOrEmptyOrWhiteSpace() && !await _userLoginRepository.GetAll().AsNoTracking().AnyAsync(x =>
                            x.LoginProvider == TTWorkConsts.LoginProvider.WeChatMiniPhone &&
                            x.UserId == loginResult.User.Id &&
                            x.ProviderKey == externalUser.WeChatUserLogin.phoneNumber)
                        )
                        {
                            await _userLoginRepository.InsertAsync(new UserLogin
                            {
                                LoginProvider = TTWorkConsts.LoginProvider.WeChatMiniPhone,
                                UserId = loginResult.User.Id,
                                ProviderKey = externalUser.WeChatUserLogin.phoneNumber
                            });
                        }

                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, e);
            }

            //如果登录成功后,要更新的User表数据
            if (loginResult.Result == AbpLoginResultType.Success && externalUser.FromClient == (int) ClientTypeEnum.Weixin)
            {
            }

            return (externalUser, loginResult);
        }


        private async Task<ExternalAuthenticateResultModel> WeixinExtAuthResult(AbpLoginResult<Tenant, User> loginResult, ExternalAuthUserInfo externalUser)
        {
            var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
            var refreshToken = CreateRefreshToken(await CreateJwtClaims(loginResult.Identity, loginResult.User, tokenType: TokenType.RefreshToken));

            var roles = await _userManager.GetRolesAsync(loginResult.User);

            externalUser.PhoneNumber = loginResult.User.PhoneNumber;
            externalUser.Id = loginResult.User.Id;

            return new ExternalAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int) _configuration.Expiration.TotalSeconds,
                RefreshToken = refreshToken,

                User = externalUser,
                RoleNames = roles.ToArray()
            };
        }

        private async Task<ExternalAuthenticateResultModel> ExternalAuthenticateResultModel(AbpLoginResult<Tenant, User> loginResult, ExternalAuthUserInfo externalUser,
            ExternalAuthenticateModel model, int fromclient)
        {
            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                {
                    loginResult.AddWeixinClaim(externalUser, fromclient);
                    return await WeixinExtAuthResult(loginResult, externalUser);
                }
                case AbpLoginResultType.UnknownExternalLogin:
                {
                    var newUser = await RegisterExternalUserAsync(externalUser);
                    if (!newUser.IsActive)
                    {
                        return new ExternalAuthenticateResultModel {WaitingForActivation = true};
                    }

                    // Try to login again with newly registered user!
                    loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());
                    if (loginResult.Result != AbpLoginResultType.Success)
                    {
                        throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                            loginResult.Result,
                            model.ProviderKey,
                            GetTenancyNameOrNull()
                        );
                    }

                    loginResult.AddWeixinClaim(externalUser, fromclient);
                    return await WeixinExtAuthResult(loginResult, externalUser);
                }
                default:
                {
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                        loginResult.Result,
                        model.ProviderKey,
                        GetTenancyNameOrNull()
                    );
                }
            }
        }


        private async Task<User> RegisterExternalUserAsync(ExternalAuthUserInfo externalUser)
        {
            var dbUser = await _userRepository.GetAll().Include(x => x.Logins).FirstOrDefaultAsync(x => x.Name == externalUser.Name || x.EmailAddress == externalUser.EmailAddress);


            //如果数据库中不存在相同名称的用户,自动重新注册
            if (dbUser == null)
            {
                var user = await _userRegistrationManager.RegisterAsync(
                    externalUser.Name,
                    externalUser.Surname,
                    externalUser.EmailAddress,
                    externalUser.EmailAddress,
                    TTWork.Abp.Core.Authorization.Users.User.CreateRandomPassword(),
                    externalUser.PhoneNumber,
                    false,
                    externalUser.HeadImgUrl,
                    externalUser.FromClient
                );

                user.Logins = new List<UserLogin>
                {
                    new() {LoginProvider = externalUser.Provider, ProviderKey = externalUser.ProviderKey, TenantId = user.TenantId},
                };
                if (externalUser.WeChatUserLogin?.unionid != null)
                {
                    user.Logins.Add(
                        new UserLogin {LoginProvider = TTWorkConsts.LoginProvider.WeChatUnionId, ProviderKey = externalUser.WeChatUserLogin.unionid, TenantId = user.TenantId}
                    );
                }

                if (externalUser.WeChatUserLogin?.openid != null)
                {
                    user.Logins.Add(
                        new UserLogin {LoginProvider = TTWorkConsts.LoginProvider.WeChatMiniProgram, ProviderKey = externalUser.WeChatUserLogin.openid, TenantId = user.TenantId}
                    );
                }

                await CurrentUnitOfWork.SaveChangesAsync();
                return user;
            }

            if (externalUser.Provider is TTWorkConsts.LoginProvider.WeChatMiniPhone or TTWorkConsts.LoginProvider.WeChatMiniProgram or TTWorkConsts.LoginProvider.WeChatUnionId)
            {
                if (externalUser.WeChatUserLogin?.phoneNumber != null && externalUser.Provider == TTWorkConsts.LoginProvider.WeChatMiniPhone)
                {
                    dbUser.Logins.Add(
                        new UserLogin {LoginProvider = TTWorkConsts.LoginProvider.WeChatMiniPhone, ProviderKey = externalUser.WeChatUserLogin.phoneNumber, TenantId = dbUser.TenantId}
                    );
                }

                if (externalUser.WeChatUserLogin?.unionid != null && externalUser.Provider == TTWorkConsts.LoginProvider.WeChatMiniPhone)
                {
                    dbUser.Logins.Add(
                        new UserLogin {LoginProvider = TTWorkConsts.LoginProvider.WeChatUnionId, ProviderKey = externalUser.WeChatUserLogin.unionid, TenantId = dbUser.TenantId}
                    );
                }

                await CurrentUnitOfWork.SaveChangesAsync();

                return dbUser;
            }

            throw new UserFriendlyException("相同用户名已存在");
        }

        private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
            //if (userInfo.ProviderKey != model.ProviderKey)
            //{
            //    throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
            //}
            return userInfo;
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);
            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }


        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            return CreateToken(claims, expiration ?? _configuration.AccessTokenExpiration);
        }

        private string CreateRefreshToken(IEnumerable<Claim> claims)
        {
            return CreateToken(claims, AppConsts.RefreshTokenExpiration);
        }

        private string CreateToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                signingCredentials: _configuration.SigningCredentials,
                expires: now.Add(expiration ?? _configuration.Expiration)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private async Task<IEnumerable<Claim>> CreateJwtClaims(ClaimsIdentity identity, User user, TimeSpan? expiration = null, TokenType tokenType = TokenType.AccessToken)
        {
            var tokenValidityKey = Guid.NewGuid().ToString();
            var claims = identity.Claims.ToList();

            var nameIdClaim = claims.First(c => c.Type == _identityOptions.ClaimsIdentity.UserIdClaimType);
            if (_identityOptions.ClaimsIdentity.UserIdClaimType != JwtRegisteredClaimNames.Sub)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value));
            }

            var userIdentifier = new UserIdentifier(AbpSession.TenantId, Convert.ToInt64(nameIdClaim.Value));
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(AppConsts.TokenValidityKey, tokenValidityKey),
                new Claim(AppConsts.UserIdentifier, userIdentifier.ToUserIdentifierString()),
                new Claim(AppConsts.TokenType, tokenType.To<int>().ToString())
            });
            if (!expiration.HasValue)
            {
                expiration = tokenType == TokenType.AccessToken
                    ? _configuration.AccessTokenExpiration
                    : _configuration.RefreshTokenExpiration;
            }

            _cacheManager
                .GetCache(AppConsts.TokenValidityKey)
                .Set(tokenValidityKey, "", expiration);
            // .Set(tokenValidityKey, "", absoluteExpireTime: expiration);

            await _userManager.AddTokenValidityKeyAsync(
                user,
                tokenValidityKey,
                DateTime.UtcNow.Add(expiration.Value)
            );
            return claims;
        }

        private string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }

        private string GetEncrpyedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }

        private bool AllowOneConcurrentLoginPerUser()
        {
            return _settingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowOneConcurrentLoginPerUser);
        }

        private async Task ValidateReCaptcha(string captchaResponse)
        {
            var requestUserAgent = Request.Headers["User-Agent"].ToString();
            if (!requestUserAgent.IsNullOrEmptyOrWhiteSpace() && WebConsts.ReCaptchaIgnoreWhiteList.Contains(requestUserAgent.Trim()))
            {
                return;
            }

            await RecaptchaValidator.ValidateAsync(captchaResponse);
        }

        private string AddSingleSignInParametersToReturnUrl(string returnUrl, string signInToken, long userId, int? tenantId)
        {
            returnUrl += (returnUrl.Contains("?") ? "&" : "?") +
                         "accessToken=" + signInToken +
                         "&userId=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(userId.ToString()));
            if (tenantId.HasValue)
            {
                returnUrl += "&tenantId=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(tenantId.Value.ToString()));
            }

            return returnUrl;
        }
    }
}