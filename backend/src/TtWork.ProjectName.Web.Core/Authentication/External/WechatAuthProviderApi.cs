using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.AppManagement.Events;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Wechat;
using TTWork.Abp.Core.Domains.Weixin;
using TtWork.ProjectName.Events.Queries;

namespace TtWork.ProjectName.Authentication.External
{
    public class WechatAuthProviderApi : ExternalAuthProviderApiBase
    {
        /// <summary>
        /// 微信公众号
        /// </summary>
        public const string ProviderName = TTWorkConsts.LoginProvider.WeChat;

        //private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IRepository<WechatUserinfo, string> _wxuseRepository;
        private readonly WeixinManger _weixinManger;
        private readonly IMediator _mediator;

        public WechatAuthProviderApi(
            IRepository<WechatUserinfo, string> wxuseRepository,
            WeixinManger weixinManger,
            IMediator mediator
        )
        {
            _wxuseRepository = wxuseRepository;
            _weixinManger = weixinManger;
            _mediator = mediator;
        }

        [UnitOfWork]
        public override async Task<ExternalAuthUserInfo> GetUserInfo(string code) //因为需要获取微信放进User.Name 
        {
            var app = await _mediator.Send(new QueryApp());
            var appid = app.GetValue("appid");
            var appSec = app.GetValue("appsec");

            var weixinUserInfoResult = await _weixinManger.Code2Session(code, appid, appSec);
            if (weixinUserInfoResult == null)
                throw new UserFriendlyException("wechat code to auth fail");

            var wxUser = await _wxuseRepository.GetAll().FirstOrDefaultAsync(z => z.openid == weixinUserInfoResult.openid);
            if (wxUser == null)
            {
                wxUser = new WechatUserinfo(weixinUserInfoResult.openid, weixinUserInfoResult.unionid,
                    weixinUserInfoResult.nickname, weixinUserInfoResult.headimgurl, weixinUserInfoResult.city, weixinUserInfoResult.province,
                    weixinUserInfoResult.country, weixinUserInfoResult.sex, (int) ClientTypeEnum.Weixin,
                    app.Name, appid);
                await _wxuseRepository.InsertAsync(wxUser);
            }
            else
            {
                wxUser.Update(weixinUserInfoResult.unionid,
                    weixinUserInfoResult.nickname, weixinUserInfoResult.headimgurl, weixinUserInfoResult.city, weixinUserInfoResult.province,
                    weixinUserInfoResult.country, weixinUserInfoResult.sex, (int) ClientTypeEnum.Weixin,
                    app.Name, appid);
                await _wxuseRepository.UpdateAsync(wxUser);
            }

            //Log.Information(JsonConvert.SerializeObject(miniUserInfo));
            var authUserInfo = new ExternalAuthUserInfo
            {
                ProviderKey = weixinUserInfoResult.openid,
                UserName = weixinUserInfoResult.openid,
                Name = weixinUserInfoResult.nickname,
                Surname = "",
                EmailAddress = weixinUserInfoResult.openid + "@wujiangapp.com",
                WeChatUserLogin = new WeChatUserLoginModel
                {
                    headimgurl = weixinUserInfoResult.headimgurl,
                    nickname = weixinUserInfoResult.nickname,
                    openid = weixinUserInfoResult.openid,
                    unionid = weixinUserInfoResult.unionid,
                    session_key = code
                },
                Provider = ProviderName,
                HeadImgUrl = weixinUserInfoResult.headimgurl,
                FromClient = (int) ClientTypeEnum.Weixin,
                City = weixinUserInfoResult.city
            };
            return authUserInfo;
        }
    }
}