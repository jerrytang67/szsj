using System;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Json;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TT.Extensions;
using TT.Extensions.Redis;
using TT.HttpClient.Weixin;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Domains.Weixin;
using TTWork.Abp.Core.Extensions.Weixin;
using TTWork.Abp.Core.Oss;
using TtWork.Lib;

namespace TTWork.Abp.Core.Applications.Wechat
{
    public class WeixinManger : AppDomainServicebase
    {
        private readonly IRepository<WechatUserinfo, string> _wxuseRepository;
        private readonly IRepository<UserLogin, long> _userLoginRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IWeixinApi _weixinApi;
        private readonly IOssClient _ossClient;
        private readonly IRedisClient _redisClient;

        public WeixinManger(
            IRepository<WechatUserinfo, string> wxuseRepository,
            IIocManager iocManager,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<User, long> userRepository,
            IWeixinApi weixinApi,
            IOssClient ossClient,
            IRedisClient redisClient
        )
            : base(iocManager)
        {
            _wxuseRepository = wxuseRepository;
            _userLoginRepository = userLoginRepository;
            _userRepository = userRepository;
            _weixinApi = weixinApi;
            _ossClient = ossClient;
            _redisClient = redisClient;
        }

        public virtual async Task<WeixinUserInfoResult> Code2Session(string code, string appid, string appsecret)
        {
            var key = $"code:{code}";
            var cache = await _redisClient.Database.StringGetAsync(key);
            if (cache.HasValue)
            {
                return JsonConvert.DeserializeObject<WeixinUserInfoResult>(cache.ToString());
            }

            var oAuth2Result = await _weixinApi.GetToken(appid, appsecret, code);
            if (!oAuth2Result.access_token.IsNullOrEmptyOrWhiteSpace())
            {
                var result = await _weixinApi.SnsUserInfo(oAuth2Result.access_token, oAuth2Result.openid);
                await _redisClient.Database.StringSetAsync(key, result.ToJsonString(), TimeSpan.FromDays(1));
                return result;
            }

            return null;
        }

        public virtual async Task<MiniSessionResult> Mini_Code2Session(string code, string appid, string appsecret)
        {
            var session = await _weixinApi.Mini_Code2Session(code, appid, appsecret);

            if (session == null)
            {
                throw new UserFriendlyException("解密失败");
            }

            return session;
        }


        public virtual async Task<IMiniUserInfoResultDto> Mini_GetUserInfo(string encryptedDataStr,
            string session_key,
            string iv)
        {
            var json = Encryption.AES_decrypt(encryptedDataStr, session_key,
                iv);
            var userInfo = JsonConvert.DeserializeObject<IMiniUserInfoResultDto>(json);
            return await Task.FromResult(userInfo);
        }


        /// <summary>
        /// 公众号获取用户信息
        /// <see cref="https://developers.weixin.qq.com/doc/offiaccount/User_Management/Get_users_basic_information_UnionID.html#UinonId"/>
        /// </summary>
        /// <param name="token"></param>
        /// <param name="openid"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<WechatUserinfo> GetUserInfo(string token,
            string openid,
            int tenantId, string appName, string appid)
        {
            var url = $"https://api.weixin.qq.com/cgi-bin/user/info?access_token={token}&openid={openid}&lang=zh_CN";
            var result = await HttpEx.GetJsonAsync<WeixinUserInfoResult>(url);

            if (result.subscribe == 1)
            {
                UnitOfWorkManager.Current.SetTenantId(tenantId);
                var wxUser = await _wxuseRepository.GetAll()
                    .FirstOrDefaultAsync(z => z.openid == result.openid);

                if (wxUser == null)
                {
                    wxUser = new WechatUserinfo(result.openid, result.unionid,
                        result.nickname, result.headimgurl,
                        result.city, result.province,
                        result.country, result.sex,
                        (int) ClientTypeEnum.Weixin, appName, appid);
                    wxUser.TenantId = tenantId;

                    await _wxuseRepository.InsertAsync(wxUser);
                }
                else
                {
                    wxUser.Update(result.unionid,
                        result.nickname, result.headimgurl,
                        result.city, result.province,
                        result.country, result.sex,
                        (int) ClientTypeEnum.Weixin);
                    await _wxuseRepository.UpdateAsync(wxUser);
                }

                await _unitOfWork.SaveChangesAsync();
                return wxUser;
            }

            return null;
        }

        [UnitOfWork]
        public virtual async Task<WechatUserinfo> GetUserInfoByUnionId(string unionId,
            int resultClientType,
            int tenantId)
        {
            return await _wxuseRepository.FirstOrDefaultAsync(x =>
                x.unionid == unionId && x.FromClient == resultClientType && x.TenantId == tenantId);
        }

        [UnitOfWork]
        public virtual async Task<(bool, string, User)> GeneralWechatLogin(WechatUserinfo wxuser, int tenantId)
        {
            if (wxuser == null)
            {
                return (false, "获取微信用户信息失败 code:0x01", null);
            }

            if (wxuser.unionid.IsNullOrEmptyOrWhiteSpace())
            {
                return (false, "微信用户没有绑定开放平台,无unionid code:0x04", null);
            }

            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var miniWxUser = await GetUserInfoByUnionId(wxuser.unionid, 1,
                    tenantId);
                if (miniWxUser == null)
                {
                    return (false, "未找到相关小程序用户 code:0x02", null);
                }

                var miniLogin = await _userLoginRepository.GetAll().FirstOrDefaultAsync(x =>
                    x.LoginProvider == TTWorkConsts.LoginProvider.WeChatMiniProgram
                    &&
                    x.ProviderKey == miniWxUser.openid);

                if (miniLogin == null)
                {
                    return (false, "未找到相关小程序登陆记录 code:0x03", null);
                }

                // WeChatProgram Provider 微信公众号openid登录增加记录
                if (!await _userLoginRepository.GetAll().AnyAsync(x => x.LoginProvider == TTWorkConsts.LoginProvider.WeChat &&
                                                                       x.ProviderKey == wxuser.openid &&
                                                                       x.UserId == miniLogin.UserId))
                    await _userLoginRepository.InsertAsync(new UserLogin()
                    {
                        TenantId = tenantId,
                        LoginProvider = TTWorkConsts.LoginProvider.WeChat,
                        ProviderKey = wxuser.openid,
                        UserId = miniLogin.UserId
                    });

                // WeChatUnionIdProgram Provider unionId登录增加记录
                if (!await _userLoginRepository.GetAll().AnyAsync(x => x.LoginProvider == TTWorkConsts.LoginProvider.WeChatUnionId &&
                                                                       x.ProviderKey == wxuser.unionid &&
                                                                       x.UserId == miniLogin.UserId))
                    await _userLoginRepository.InsertAsync(new UserLogin()
                    {
                        TenantId = tenantId,
                        LoginProvider = TTWorkConsts.LoginProvider.WeChatUnionId,
                        ProviderKey = wxuser.unionid,
                        UserId = miniLogin.UserId
                    });

                var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == miniLogin.UserId);

                return (true, $"成功绑定到帐号 {user.UserName}", user);
            }
        }

        public virtual async Task<string> GetAccessTokenAsync(string appid, string appSeret)
        {
            var key = $"accesstoken:{appid}";
            var cache = await _redisClient.Database.StringGetAsync(key);
            if (cache.HasValue)
            {
                return cache.ToString();
            }

            var token = await _weixinApi.GetToken(appid, appSeret);
            Logger.InfoFormat("请求AccessToken {@AccessTokenResult}", JsonConvert.SerializeObject(token));
            if (token == null || token.errcode != 0) throw new UserFriendlyException("AccessToken获取失败");
            await _redisClient.Database.StringSetAsync(key, token.access_token, TimeSpan.FromSeconds(7200));
            return token.access_token;
        }


        private int _tryTimes = 0;

        [UnitOfWork]
        public virtual async Task<string> GetJsSdkAsync(string appid, string appSeret)
        {
            var key = $"jssdk:{appid}";

            var cache = await _redisClient.Database.StringGetAsync(key);
            if (cache.HasValue)
            {
                try
                {
                    return JsonConvert.DeserializeObject<JsSdkCacheItem>(cache.ToString()).Ticket;
                }
                catch (Exception)
                {
                    if (++_tryTimes >= 5) return null;
                    Logger.Warn($"GetJsSdkAsync JSON 反序列化失败,删除KEY后重试.第{_tryTimes}");
                    await _redisClient.Database.KeyDeleteAsync(key);
                    return await GetJsSdkAsync(appid, appSeret);
                }
            }

            JsSdkCacheItem jssdk = null;
            var token = await GetAccessTokenAsync(appid, appSeret);

            var ticket = await _weixinApi.GetTicket(token);
            if (ticket.errcode == 0)
            {
                Logger.Info(JsonConvert.SerializeObject(token));
                jssdk = new JsSdkCacheItem
                {
                    Appid = appid,
                    AppSecret = appSeret,
                    Ticket = ticket.ticket,
                    TimeCreated = DateTimeOffset.Now,
                    ExpiresIn = ticket.expires_in,
                    TimeExpired = DateTimeOffset.Now.AddSeconds(ticket.expires_in),
                };
            }

            else
                throw new UserFriendlyException($"ticket 获取失败: {ticket.errmsg}");

            var jsonStr = JsonConvert.SerializeObject(jssdk);
#if DEBUG
            Logger.Warn(jsonStr);
#endif
            await _redisClient.Database.StringSetAsync(key, jsonStr, TimeSpan.FromSeconds(jssdk.ExpiresIn));
            return jssdk.Ticket;
        }


        private async Task<JsSdkCacheItem> GetJsSdk(string appid, string appSeret)
        {
            var token = await GetAccessTokenAsync(appid, appSeret);

            var ticket = await _weixinApi.GetTicket(token);
            if (ticket.errcode == 0)
            {
                Logger.Info(JsonConvert.SerializeObject(token));
                return new JsSdkCacheItem
                {
                    Appid = appid,
                    AppSecret = appSeret,
                    Ticket = ticket.ticket,
                    TimeCreated = DateTimeOffset.Now,
                    ExpiresIn = ticket.expires_in,
                    TimeExpired = DateTimeOffset.Now.AddSeconds(ticket.expires_in),
                };
            }

            Logger.Error($"ticket 获取失败: {ticket.errmsg}");
            return null;
        }

        /// <summary>
        /// 生成小程序码
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appSec"></param>
        /// <param name="scene"></param>
        /// <param name="page">"pages/index/index" by default</param>
        /// <param name="width">二维码的宽度，单位 px，最小 280px，最大 1280px</param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public virtual async Task<string> getwxacodeunlimit(string appid, string appSec, string scene = "", string page = "pages/index/index", int width = 430, bool useCache = true)
        {
            var key = $"{appid}:QR:{page}";

            if (useCache)
            {
                var cache = await _redisClient.Database.HashGetAsync(key, scene ?? "");
                if (cache.HasValue)
                {
                    return cache.ToString();
                }
            }

            var token = await GetAccessTokenAsync(appid, appSec);
            var url = $"https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token={token}";
            var img = HttpEx.PostGotImageByte(url, new {scene, page, width});
            var ts = DateTime.Now.ToFileTimeUtc();

            var result = await _ossClient.writeFile($"/{_ossClient.UserName}/qr/{scene}_{ts}.jpg", img, true);
            if (result)
            {
                var path = $"{_ossClient.Domain}/{_ossClient.UserName}/qr/{scene}_{ts}.jpg";
                if (useCache)
                    await _redisClient.Database.HashSetAsync(key, scene, path);

                return path;
            }

            throw new UserFriendlyException("生成小程序二维码失败");
        }


        public virtual async Task<string> GetQrLimitCodeUrl(string appid, string appSec, string str)
        {
            var key = "ProjectName:QrLimit";

            var cache = await _redisClient.Database.HashGetAsync(key, str);
            if (cache.HasValue)
            {
                return cache.ToString();
            }

            var accessToken = await GetAccessTokenAsync(appid, appSec);

            var url =
                $"https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={accessToken}";
            try
            {
                var postdata = new
                {
                    action_name = "QR_LIMIT_STR_SCENE",
                    action_info = new
                    {
                        scene = new
                        {
                            scene_str = str
                        }
                    }
                };
                var res = HttpEx.PostHtml(url, JsonConvert.SerializeObject(postdata));
                Logger.Warn(res);
                var data = JsonConvert.DeserializeObject<WechatQrTicketResponse>(res);
                var result = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + data.ticket;
                await _redisClient.Database.HashSetAsync(key, str, result);
                return "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + data.ticket;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return null;
            }
        }


        [UnitOfWork]
        public virtual async Task RemoveLoginKey(string wechatprogram, string contextFromUserName, int tenantId)
        {
            Logger.Warn($"{contextFromUserName} 取消关注");
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var find = await _userLoginRepository
                    .FirstOrDefaultAsync(x => x.TenantId == tenantId
                                              && x.ProviderKey == contextFromUserName
                                              && x.LoginProvider == wechatprogram);
                if (find != null)
                    await _userLoginRepository.DeleteAsync(find);
            }
        }
    }

    public class WechatQrTicketResponse
    {
        public string ticket { get; set; }
        public int expire_seconds { get; set; }
        public string url { get; set; }
    }
}