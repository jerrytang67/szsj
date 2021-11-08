using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using MediatR;
using Microsoft.AspNetCore.Http;
using TtWork.ProjectName.Models.TokenAuth;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TT.Extensions;
using TTWork.Abp.AppManagement.Events;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Wechat;
using TTWork.Abp.Core.Domains.Weixin;
using TTWork.Abp.Core.Extensions;

namespace TtWork.ProjectName.Authentication.External
{
    public class WechatMiniProgramAuthProviderApi : ExternalAuthProviderApiBase
    {
        /// <summary>
        /// 微信小程序
        /// </summary>
        public const string ProviderName = TTWorkConsts.LoginProvider.WeChatMiniProgram;

        //private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IRepository<WechatUserinfo, string> _wxuseRepository;
        private readonly IMediator _mediator;
        private readonly WeixinManger _weixinManger;

        public WechatMiniProgramAuthProviderApi(IExternalAuthConfiguration externalAuthConfiguration,
            IRepository<WechatUserinfo, string> wxuseRepository,
            IMediator mediator,
            WeixinManger weixinManger)
        {
            _wxuseRepository = wxuseRepository;
            _mediator = mediator;
            _weixinManger = weixinManger;
        }

        [UnitOfWork]
        public async override Task<ExternalAuthUserInfo> GetUserInfo(string weChatAuthModel) //因为需要获取微信放进User.Name 
        {
            var app = await _mediator.Send(new QueryApp());
            var appid = app.GetValue("appid");
            var appSec = app.GetValue("appsec");

            var authModel = JsonConvert.DeserializeObject<WeChatMiniProgramAuthenticateModel>(weChatAuthModel);

            if (authModel!.openid.IsNullOrEmptyOrWhiteSpace())
                throw new UserFriendlyException("需要小程序端传递openid");

            // 解密用户信息
            var miniUserInfo =
                await _weixinManger.Mini_GetUserInfo(authModel.encryptedData, authModel.session_key, authModel.iv);

            if (miniUserInfo != null)
            {
                miniUserInfo.openid = authModel.openid;
                miniUserInfo.unionid = authModel.unionid;
                
                var wxUser = await _wxuseRepository.GetAll().FirstOrDefaultAsync(z => z.openid == miniUserInfo.openid);
                if (wxUser == null)
                {
                    wxUser = new WechatUserinfo(miniUserInfo.openid, miniUserInfo.unionid,
                        miniUserInfo.nickName, miniUserInfo.avatarUrl, miniUserInfo.city, miniUserInfo.province,
                        miniUserInfo.country, miniUserInfo.gender, 1, app.Name, appid);
                    await _wxuseRepository.InsertAsync(wxUser);
                }
                else
                {
                    wxUser.Update(miniUserInfo.unionid,
                        miniUserInfo.nickName, miniUserInfo.avatarUrl, miniUserInfo.city, miniUserInfo.province,
                        miniUserInfo.country, miniUserInfo.gender, (int) ClientTypeEnum.WeixinMini, app.Name, appid);
                    await _wxuseRepository.UpdateAsync(wxUser);
                }
            }

            //Log.Information(JsonConvert.SerializeObject(miniUserInfo));
            var authUserInfo = new ExternalAuthUserInfo
            {
                ProviderKey = miniUserInfo.openid,
                UserName = miniUserInfo.openid,
                Name = miniUserInfo.nickName,
                Surname = "",
                EmailAddress = miniUserInfo.openid + "@wujiangapp.com",
                WeChatUserLogin = new WeChatUserLoginModel
                {
                    headimgurl = miniUserInfo.avatarUrl,
                    nickname = miniUserInfo.nickName,
                    openid = miniUserInfo.openid,
                    unionid = miniUserInfo.unionid,
                    session_key = authModel.session_key
                },
                Provider = ProviderName,
                HeadImgUrl = miniUserInfo.avatarUrl,
                FromClient = (int) ClientTypeEnum.WeixinMini,
                City = miniUserInfo.city
            };
            return authUserInfo;
        }
    }
}