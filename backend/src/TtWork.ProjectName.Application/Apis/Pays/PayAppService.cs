using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TTWork.Abp.Core;
using TtWork.ProjectName.Entities.Pay;

namespace TtWork.ProjectName.Apis.Pays
{
    public class PayAppService : AbpAppServiceBase
    {
        private readonly IRepository<TenPayNotify> _tenPayNotifyRepository;
        private readonly IRepository<PayOrder, long> _payorderRepository;

        public PayAppService(
            IRepository<TenPayNotify> tenPayNotifyRepository,
            IRepository<PayOrder, long> payorderRepository
        )
        {
            _tenPayNotifyRepository = tenPayNotifyRepository;
            _payorderRepository = payorderRepository;
        }

        /// <summary>
        /// JS-SDK支付回调地址（在统一下单接口中设置notify_url）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [DontWrapResult(WrapOnSuccess = false)]
        public async Task<object> PayNotifyUrl([FromBody] TenPayNotifyXml input)
        {
            Logger.Warn(JsonConvert.SerializeObject(input));
            var return_msg = input.return_msg;
            var return_code = input.return_code;
            //string res = null;

            var tenPayNotify = new TenPayNotify
            {
                appid = input.appid,
                bank_type = input.bank_type,
                cash_fee = input.cash_fee,
                fee_type = input.fee_type,
                is_subscribe = input.is_subscribe,
                mch_id = input.mch_id,
                nonce_str = input.nonce_str,
                openid = input.openid,
                out_trade_no = input.out_trade_no,
                result_code = input.result_code,
                return_code = input.return_code,

                sign = input.sign,
                time_end = input.time_end,
                total_fee = input.total_fee,
                trade_type = input.trade_type,
                transaction_id = input.transaction_id
            };

            var notify = await _tenPayNotifyRepository.GetAll()
                .FirstOrDefaultAsync(z => z.out_trade_no == tenPayNotify.out_trade_no);

            if (notify == null)
            {
                notify = tenPayNotify;
                _tenPayNotifyRepository.Insert(notify);
            }

            var order = await _payorderRepository.FirstOrDefaultAsync(z => z.BillNo == tenPayNotify.out_trade_no);
            if (order != null)
            {
                if (order.TotalPrice.ToString() == notify.total_fee)
                {
                    order.SuccessPay(notify.Id);
                }
                else
                    Logger.Error($"Tenpay Result Fee not equals !!pay is {notify.fee_type} , db is {order.TotalPrice} , BillNo is {order.BillNo}");
            }
            else
            {
                Logger.Error($"cant't find BillNo {notify.out_trade_no}");
            }
            
            var xml = $@"<xml>
<return_code><![CDATA[{return_code}]]></return_code>
<return_msg><![CDATA[{return_msg}]]></return_msg>
</xml>";
            return xml;
        }
    }

    [XmlRoot("xml", Namespace = "")]
    public class TenPayResultXML
    {
        public XmlNode return_code { get; set; }
        public XmlNode return_msg { get; set; }
    }

    public class TenPayResult
    {
        public string return_code { get; set; }
        public string return_msg { get; set; }
    }

    [XmlRoot("xml")]
    public class TenPayNotifyXml : TenPayResult
    {
        //just return_code success
        public string appid { get; set; }
        public string mch_id { get; set; }
        public string device_info { get; set; }
        public string nonce_str { get; set; }
        public string sign { get; set; }
        public string result_code { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; } //JSAPI、NATIVE、APP

        /// <summary>
        /// 付款银行 ,银行类型，采用字符串类型的银行标识，银行类型见银行列表 
        /// </summary>
        public string bank_type { get; set; }

        /// <summary>
        /// is_subscribe 用户是否关注公众账号，Y-关注，N-未关注
        /// </summary>
        public string is_subscribe { get; set; }

        public string openid { get; set; }

        public string total_fee { get; set; }
        public int settlement_total_fee { get; set; }
        public string fee_type { get; set; }
        public string cash_fee { get; set; }
        
        public string cash_fee_type { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户订单号	
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 支付完成时间	
        /// </summary>
        public string time_end { get; set; }
    }
}