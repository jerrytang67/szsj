using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using TT.Extensions;
using TT.HttpClient.Weixin;
using TTWork.Abp.AppManagement.Apps;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Applications.Wechat;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.Core.Domains.Weixin;
using WechatUserinfoDto = TtWork.ProjectName.Apis.Wechat.Dtos.WechatUserinfoDto;

namespace TtWork.ProjectName.Apis.Wechat
{
    [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
    public class WechatUserinfoAppService : AsyncCrudAppService<WechatUserinfo, WechatUserinfoDto, string, AppResultRequestDto, WechatUserinfoDto, WechatUserinfoDto>, IWechatUserinfoAppService
    {
        private readonly IRepository<UserLogin, long> _userLoginRepo;
        private readonly IWeixinApi _weixinApi;
        private readonly IAppProvider _appProvider;
        private readonly WeixinManger _weixinManger;
        private readonly ILogger _logger;

        public WechatUserinfoAppService(
            IRepository<WechatUserinfo, string> repository,
            IRepository<UserLogin, long> userLoginRepo,
            IWeixinApi weixinApi,
            IAppProvider appProvider,
            WeixinManger weixinManger,
            ILogger logger
        ) : base(repository)
        {
            _userLoginRepo = userLoginRepo;
            _weixinApi = weixinApi;
            _appProvider = appProvider;
            _weixinManger = weixinManger;
            _logger = logger;
        }


        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        public async Task<object> GetInfoByOpenid(string openid)
        {
            openid = openid.Trim();
            var user = await Repository.FirstOrDefaultAsync(x => x.openid == openid);
            if (user == null)
                throw new UserFriendlyException(L("notfind"));

            var app = await _appProvider.GetOrNullAsync(user.appName);
            var appid = app["appid"] ?? throw new UserFriendlyException($"App:{user.appName} appid未设置");
            var appSec = app["appsec"] ?? throw new UserFriendlyException($"App:{user.appName} appsec未设置");

            var token = await _weixinManger.GetAccessTokenAsync(appid, appSec);


            var result = await _weixinApi.GetUserInfo(token, openid);


            if (!result.unionid.IsNullOrEmptyOrWhiteSpace())
            {
                var a = await _userLoginRepo.FirstOrDefaultAsync(x => x.LoginProvider == "WeChat" && x.ProviderKey == openid);

                if (a != null)
                {
                    var b = await _userLoginRepo.FirstOrDefaultAsync(x => x.LoginProvider == "WeChatUnionId" && x.ProviderKey == result.unionid);
                    if (b == null)
                    {
                        b = new UserLogin
                        {
                            TenantId = AbpSession.TenantId,
                            LoginProvider = "WeChatUnionId",
                            ProviderKey = result.unionid,
                            UserId = a.UserId
                        };
                        await _userLoginRepo.InsertAsync(b);
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }
            }

            return result;
        }


        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        public async Task<object> SendMini(string openid)
        {
            // var app = await _appProvider.GetOrNullAsync("MallApp_PW");
            // var appid = app["appid"] ?? throw new UserFriendlyException($"App:MallApp_PW appid未设置");
            // var appSec = app["appsec"] ?? throw new UserFriendlyException($"App:MallApp_PW appsec未设置");
            var token = await _weixinManger.GetAccessTokenAsync("wxce64b119daa2eabe", "dc828c3fb06bbbb3869c1235bc3f8f15");

            return await _weixinApi.CustomSend(token, openid, "miniprogrampage", new
            {
                title = "测试商户",
                appid = "wxcaa8c0e5a3de5000",
                pagepath = "/pages/coupon/useCoupon?id=1",
                thumb_media_id = "ttAGJ-T1fmwr_Zo4fqW98i2FXKQ5aK5z-1iYJ9bOcJI"
            });
        }


        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        [UnitOfWork(isTransactional: false, timeout: 7200000)]
        public async Task RunAll(int start = 0)
        {
            var app = await _appProvider.GetOrNullAsync("MallApp_PW");
            var appid = app["appid"] ?? throw new UserFriendlyException($"App:MallApp_PW appid未设置");
            var appSec = app["appsec"] ?? throw new UserFriendlyException($"App:MallApp_PW appsec未设置");

            var token = await _weixinManger.GetAccessTokenAsync(appid, appSec);


            int count = start;

            var list = await Repository.GetAll().Skip(start).Select(x => x.openid).ToListAsync();

            foreach (var openid in list)
            {
                var result = await _weixinApi.GetUserInfo(token, openid);

                if (!result.unionid.IsNullOrEmptyOrWhiteSpace())
                {
                    var a = await _userLoginRepo.FirstOrDefaultAsync(x => x.LoginProvider == "WeChat" && x.ProviderKey == openid);

                    if (a != null)
                    {
                        var b = await _userLoginRepo.FirstOrDefaultAsync(x => x.LoginProvider == "WeChatUnionId" && x.ProviderKey == result.unionid);
                        if (b == null)
                        {
                            b = new UserLogin
                            {
                                TenantId = AbpSession.TenantId,
                                LoginProvider = "WeChatUnionId",
                                ProviderKey = result.unionid,
                                UserId = a.UserId
                            };
                            await _userLoginRepo.InsertAsync(b);
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                    }
                }

                count++;
                _logger.InfoFormat($"已处理{count}条记录.{{@WxResult}}", result);
            }
        }

        protected override IQueryable<WechatUserinfo> ApplySorting(IQueryable<WechatUserinfo> query, AppResultRequestDto input)
        {
            return query.OrderBy(input.Sorting);
        }

        protected override IQueryable<WechatUserinfo> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input).WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), z => z.nickname.Contains(input.Keyword));
        }
    }
}