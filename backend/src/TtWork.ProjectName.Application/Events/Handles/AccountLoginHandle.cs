using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using TT.Extensions;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Wechat;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Extensions;
using TtWork.Lib;


namespace TtWork.ProjectName.Events.Handles
{
    public class AccountLoginHandle : IAsyncEventHandler<AccountLoginEvent>,
        ITransientDependency
    {
        private readonly WeixinManger _weixinManger;
        private readonly IAbpSession _abpSession;
        private readonly ILogger _logger;
        private readonly IWxTemplateMsgSender _sender;
        private readonly UserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountLoginHandle(IWxTemplateMsgSender sender,
            UserManager userManager,
            WeixinManger weixinManger,
            IAbpSession abpSession,
            IHttpContextAccessor httpContextAccessor,
            ILogger logger)
        {
            _sender = sender;
            _userManager = userManager;
            _weixinManger = weixinManger;
            _abpSession = abpSession;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task HandleEventAsync(AccountLoginEvent eventData)
        {
            if (eventData.LoginResult.Result == AbpLoginResultType.Success)
            {
                var openid = await _userManager.GetUserLoginKey(eventData.LoginResult.User.Id, ClientTypeEnum.Weixin);
                if (openid != null)
                {
                }
            }
        }

        // ReSharper disable once InconsistentNaming
        private string GetSingleIP(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return ip;
            var commaIndex = ip.LastIndexOf(",", StringComparison.Ordinal);
            if (commaIndex >= 0)
            {
                ip = ip.Substring(commaIndex + 1);
            }

            return ip;
        }
    }
}