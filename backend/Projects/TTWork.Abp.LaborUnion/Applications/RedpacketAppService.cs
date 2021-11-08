using TT.HttpClient.Weixin;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Authorization.Users;

namespace TTWork.Abp.LaborUnion.Applications
{
    public class RedpacketAppService : AbpAppServiceBase
    {
        private readonly IPayApi _payApi;
        private readonly UserManager _userManager;

        public RedpacketAppService(
            IPayApi payApi,
            UserManager userManager)
        {
            _payApi = payApi;
            _userManager = userManager;
        }
    }
}