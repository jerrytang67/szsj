using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using TtWork.ProjectName.Configuration;
using TtWork.ProjectName.Entities.Pay;
using MediatR;
using Newtonsoft.Json;
using TT.HttpClient.Weixin;
using TTWork.Abp.Core;

namespace TtWork.ProjectName.Managers
{
    public class PayManager : AppDomainServicebase
    {
        private readonly IRepository<PayOrder, long> _payOrderRepository;
        private readonly IPayApi _payApi;
        private readonly IMediator _mediator;

        public PayManager(
            IRepository<PayOrder, long> payOrderRepository,
            IPayApi payApi,
            IMediator mediator,
            IIocManager iocManager) : base(iocManager)
        {
            _payOrderRepository = payOrderRepository;
            _payApi = payApi;
            _mediator = mediator;
        }

        //todo:这里需要后台设置的KEY
        public async Task TenPayDetail(string BillNo, int fromClient)
        {
            var isMini = fromClient == 1;

            var appid = await SettingManager.GetSettingValueForTenantAsync(
                isMini ? AppSettings.WeixinManagement.MiniAppId : AppSettings.WeixinManagement.AppId,
                _abpSession.GetTenantId());
            var mchId = await SettingManager.GetSettingValueForTenantAsync(AppSettings.WeixinManagement.PayMchId,
                _abpSession.GetTenantId());
            var payKey = await SettingManager.GetSettingValueForTenantAsync(AppSettings.WeixinManagement.PayKey,
                _abpSession.GetTenantId());

            //var nonceStr = ""; // TenPayV3Util.GetNoncestr();


            //var data = ""; // new TenPayV3OrderQueryRequestData(appid, mchId, null, nonceStr, BillNo, "1245955802");
            var result = ""; // await TenPayV3.OrderQueryAsync(data);
            Logger.Warn(JsonConvert.SerializeObject(result));
        }


        public async Task<(bool, string)> TenPayRefund(PayOrder payorder, RefundLog refundLog)
        {
            var isMini = payorder.FromClient == 1;

            var appid = await SettingManager.GetSettingValueForTenantAsync(
                isMini ? AppSettings.WeixinManagement.MiniAppId : AppSettings.WeixinManagement.AppId,
                _abpSession.GetTenantId());
            var mchId = await SettingManager.GetSettingValueForTenantAsync(AppSettings.WeixinManagement.PayMchId,
                _abpSession.GetTenantId());
            var mchKey = await SettingManager.GetSettingValueForTenantAsync(AppSettings.WeixinManagement.PayKey,
                _abpSession.GetTenantId());

            var refundAccount =
                await SettingManager.GetSettingValueForTenantAsync(AppSettings.WeixinManagement.TenPay_RefundAccount,
                    _abpSession.GetTenantId());


            var result = await _payApi.RefundAsync(
                appid,
                mchId,
                mchKey: mchKey,
                "",
                "",
                transactionId: null,
                outTradeNo: payorder.BillNo,
                outRefundNo: refundLog.Id.ToString(),
                totalFee: payorder.TotalPrice,
                refundFee: refundLog.Price,
                refundDesc: "管理员通过退款",
                refundAccount: refundAccount
                // REFUND_SOURCE_UNSETTLED_FUNDS-- - 未结算资金退款（默认使用未结算资金退款）
                // REFUND_SOURCE_RECHARGE_FUNDS-- - 可用余额退款
            );

            if (result)
            {
                return (true, "退款成功");
            }

            return (false, "退款失败");
        }

        //todo:这里默认小程序
        // public async Task<object> CreateActiveTenPay(ActivityApply apply, bool isMini = true)
        // {
        //     var openid = _abpSession.Get_openid();
        //     if (openid.IsNullOrEmptyOrWhiteSpace())
        //     {
        //         throw new UserFriendlyException("微信信息获取失败,请重新打开本小程序登录后再试试");
        //     }
        //
        //     var appid = await SettingManager.GetSettingValueForTenantAsync(
        //         isMini ? AppSettings.WeixinManagement.MiniAppId : AppSettings.WeixinManagement.AppId,
        //         _abpSession.GetTenantId());
        //
        //     var mchId = await SettingManager.GetSettingValueForTenantAsync(AppSettings.WeixinManagement.PayMchId,
        //         _abpSession.GetTenantId());
        //
        //     var mchKey = await SettingManager.GetSettingValueForTenantAsync(AppSettings.WeixinManagement.PayKey,
        //         _abpSession.GetTenantId());
        //
        //     var notifyUrl = await SettingManager.GetSettingValueForTenantAsync(AppSettings.WeixinManagement.PayNotify,
        //         _abpSession.GetTenantId());
        //
        //     var insertOrder = new PayOrder();
        //
        //     insertOrder.CreatWxPayFromActivityApply(apply, mchId, openid, isMini ? 1 : 2);
        //
        //     insertOrder.FromUserId = await _mediator.Send(new UserSharedFromWhoQuery(_abpSession.UserId));
        //
        //     _payOrderRepository.Insert(insertOrder);
        //
        //     await CurrentUnitOfWork.SaveChangesAsync();
        //
        //     if (insertOrder.Id > 0)
        //     {
        //         var unifiedResult = await _payApi.UnifiedOrderAsync(
        //             appid,
        //             mchId,
        //             mchKey,
        //             body: apply.Activity.Title,
        //             outTradeNo: insertOrder.BillNo,
        //             totalFee: insertOrder.TotalPrice,
        //             notifyUrl: notifyUrl,
        //             tradeType: Consts.TradeType.JsApi,
        //             openId: insertOrder.OpenId,
        //             billCreateIp: _httpContext.HttpContext.Connection.RemoteIpAddress.ToString()
        //         );
        //
        //         insertOrder.SetData("prepay_id", unifiedResult.prepay_id);
        //         insertOrder.SetData("openid", openid);
        //
        //
        //         return unifiedResult;
        //     }
        //
        //     throw new UserFriendlyException("insert payorder to db failed!!");
        // }
    }
}