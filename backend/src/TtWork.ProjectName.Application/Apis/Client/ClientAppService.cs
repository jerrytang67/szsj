using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TtWork.ProjectName.Apis.MultiTenancy.Dto;
using TtWork.ProjectName.Dto;
using TtWork.ProjectName.Entities;
using TtWork.ProjectName.Entities.Place;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using TT.Extensions;
using TT.Extensions.Redis;
using TTWork.Abp.AppManagement.Applications.TT.Abp.AppManagement.Application;
using TTWork.Abp.AppManagement.Apps;
using TTWork.Abp.AppManagement.Events;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Wechat;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Definitions;
using TtWork.Lib;
using TtWork.ProjectName.Apis.Client.Dtos;
using TtWork.ProjectName.Events.Queries;

namespace TtWork.ProjectName.Apis.Client
{
    public class ClientAppService : AbpAppServiceBase
    {
        private string ak = "f8vW5GLQR7CaKA52XsxGXpR0";

        private readonly IRepository<Swiper> _swipeRepository;
        private readonly WeixinManger _weixinManger;
        private readonly IUserNotificationManager _userNotificationManager;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IAppProvider _appProvider;
        private readonly IRedisClient _redisClient;
        private readonly IMediator _mediator;

        public ClientAppService(IRepository<Swiper> swipeRepository,
            WeixinManger weixinManger,
            IUserNotificationManager userNotificationManager,
            IGuidGenerator guidGenerator,
            IAppProvider appProvider,
            IRedisClient redisClient,
            IMediator mediator
        )
        {
            _swipeRepository = swipeRepository;
            _weixinManger = weixinManger;
            _userNotificationManager = userNotificationManager;
            _guidGenerator = guidGenerator;
            _appProvider = appProvider;
            _redisClient = redisClient;
            _mediator = mediator;
        }

        [AbpAllowAnonymous]
        [HttpGet]
        public async Task<object> Init()
        {
            var tenant = await GetCurrentTenantAsync();
            var swiper = await _swipeRepository.GetAll().ToListAsync();
            var version = "1.0.0";
            return new
            {
                tenant = ObjectMapper.Map<TenantDto>(tenant),
                swiper = ObjectMapper.Map<List<SwiperDto>>(swiper),
                version
            };
        }

        [HttpGet]
        public async Task<JssdkResultDto> GetJssdk(string url, string appName)
        {
            var app = await GetAppNameAsync();
            var appid = app.GetValue("appid");
            var appSec = app.GetValue("appsec");

            var ticket = await _weixinManger.GetJsSdkAsync(appid, appSec);

            var nonceStr = _guidGenerator.Create().ToShortString();
            var timestamp = StringExt.GetTimestamp();

            var signature = $"jsapi_ticket={ticket}&noncestr={nonceStr}&timestamp={timestamp}&url={url}".GetSha1();

            return new JssdkResultDto(appid, timestamp, nonceStr, signature);
        }

        [HttpGet]
        public async Task<object> OAuth2(string code)
        {
            var app = await GetAppNameAsync();
            var appid = app.GetValue("appid");
            var appSec = app.GetValue("appsec");

            var result = await _weixinManger.Code2Session(code, appid, appSec);
            return result;
        }

        [HttpGet]
        public async Task<dynamic> MiniCode2Session(string code)
        {
            var app = await _mediator.Send(new QueryApp());
            var appid = app.GetValue("appid");
            var appSec = app.GetValue("appsec");
            return await _weixinManger.Mini_Code2Session(code, appid, appSec);
        }


        private async Task<AppDto> GetAppNameAsync()
        {
            return await _mediator.Send(new QueryApp());
        }

        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<object> GetMyInfo()
        {
            //await _appNotifier.WelcomeToTheApplicationAsync(user: await GetCurrentUserAsync());
            var unReadCount = await _userNotificationManager.GetUserNotificationCountAsync(
                AbpSession.ToUserIdentifier(), UserNotificationState.Unread
            );
            return new
            {
                UnReadCount = unReadCount
            };
        }

        [AbpAllowAnonymous]
        public async Task<string> GetQr(string code, string appName, string page = "pages/index/index", int width = 430, bool useCache = true)
        {
            Logger.Warn($"生成二维码{appName},{page},{code}");

            var app = await _appProvider.GetOrNullAsync(appName);
            var appid = app["appid"] ?? throw new UserFriendlyException($"App:{appName} appid未设置");
            var appSec = app["appsec"] ?? throw new UserFriendlyException($"App:{appName} appsec未设置");

            return await _weixinManger.getwxacodeunlimit(appid, appSec, code, page, width, useCache);
        }


        static string GetMd5(string str)
        {
            //创建MD5哈稀算法的默认实现的实例
            MD5 md5 = MD5.Create();
            //将指定字符串的所有字符编码为一个字节序列
            byte[] buffer = Encoding.Default.GetBytes(str);
            //计算指定字节数组的哈稀值
            byte[] bufferMd5 = md5.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bufferMd5.Length; i++)
            {
                //x:表示将十进制转换成十六进制
                sb.Append(bufferMd5[i].ToString("x2"));
            }

            return sb.ToString();
        }

        [DontWrapResult]
        public async Task<object> GetSignature(string data)
        {
            var password = GetMd5("ugFYuYpFlzIYI3ovAPA5OGKRon5SZ5jh");

            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(password));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));

            return await Task.FromResult(new {signature = Convert.ToBase64String(hashBytes)});
        }


        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<List<Place>> GetPlaceSuggestion(string query,
            string region = "全国",
            string type = "gcj02ll")
        {
            try
            {
                var limit = region != "全国";
                var url = $@"https://api.map.baidu.com/place/v2/suggestion?query={query}&region={region}&city_limit={limit}&output=json&ak={ak}&ret_coordtype=gcj02ll";

                var result = await HttpEx.GetJsonAsync<PlaceSuggestionDto>(url);
                if (result.message != "ok") return new List<Place>();
                foreach (var poi in result.result)
                {
                    await _redisClient.Database.HashSetAsync("TtWork:Poi", poi.Uid,
                        JsonConvert.SerializeObject(poi), When.Always);
                }

                return result.result;
            }
            catch (Exception)
            {
                return new List<Place>();
            }
        }
    }
}