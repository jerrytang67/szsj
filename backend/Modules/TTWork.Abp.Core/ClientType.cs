using System.ComponentModel.DataAnnotations;

namespace TTWork.Abp.Core
{
    public enum ClientTypeEnum
    {
        [Display(Name = "系统")] System = 0,
        [Display(Name = "微信小程序")] WeixinMini = 1,
        [Display(Name = "微信公众号")] Weixin = 2
    }
}