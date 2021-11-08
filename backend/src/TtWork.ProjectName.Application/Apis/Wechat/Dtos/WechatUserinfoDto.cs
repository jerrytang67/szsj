using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Domains.Weixin;

namespace TtWork.ProjectName.Apis.Wechat.Dtos
{
    [AutoMapFrom(typeof(WechatUserinfo))]
    public class WechatUserinfoDto : EntityDto<string>
    {
        public string openid { get; set; }
        
        public string unionid { get; set; }

        public string nickname { get; set; }

        public string headimgurl { get; set; }

        public string city { get; set; }

        public string province { get; set; }

        public string country { get; set; }

        public int sex { get; set; }

        public ClientTypeEnum FromClient { get; set; }
        
        public string appName { get; protected set; }
    }
}