using Abp.Application.Services;
using TTWork.Abp.Core.Applications.Dtos;
using WechatUserinfoDto = TtWork.ProjectName.Apis.Wechat.Dtos.WechatUserinfoDto;

namespace TtWork.ProjectName.Apis.Wechat
{
    public interface IWechatUserinfoAppService : IAsyncCrudAppService<WechatUserinfoDto, string, AppResultRequestDto, WechatUserinfoDto, WechatUserinfoDto> 
    {
    }
}
