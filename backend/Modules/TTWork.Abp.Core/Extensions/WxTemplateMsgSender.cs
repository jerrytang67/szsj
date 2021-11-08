using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Castle.Core.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TT.Extensions;
using TtWork.Lib;

namespace TTWork.Abp.Core.Extensions
{
    public interface IWxTemplateMsgSender
    {
        Task<(bool, string)> SendAsync(string openidList,
            string accessToken,
            string templateId,
            string first = "",
            string keyword1 = "",
            string keyword2 = "",
            string keyword3 = "",
            string keyword4 = "",
            string keyword5 = "",
            string keyword6 = "",
            string keyword7 = "",
            string remark = "",
            string url = "",
            string page = "",
            string form_id = "",
            string emphasis_keyword = "keyord1.DATA",
            string appid = "",
            string path = "pages/index/index");
    }

    public class WxTemplateMsgSender : IWxTemplateMsgSender, ITransientDependency
    {
        private readonly ILogger _logger;

        public WxTemplateMsgSender(
            ILogger logger
        )
        {
            _logger = logger;
        }

        ///  <summary>
        /// 发送微信模板消息
        ///  </summary>
        ///  <param name="openidList">多条请用英文标点,分隔</param>
        ///  <param name="accessToken"></param>
        ///  <param name="templateId"></param>
        ///  <param name="first"></param>
        ///  <param name="keyword1"></param>
        ///  <param name="keyword2"></param>
        ///  <param name="keyword3"></param>
        ///  <param name="keyword4"></param>
        ///  <param name="keyword5"></param>
        ///  <param name="keyword6"></param>
        ///  <param name="keyword7"></param>
        ///  <param name="remark"></param>
        ///  <param name="url"></param>
        ///  <param name="page"></param>
        ///  <param name="form_id"></param>
        ///  <param name="emphasis_keyword"></param>
        ///  <param name="appid"></param>
        ///  <param name="path"></param>
        ///  <returns></returns>
        public async Task<(bool, string)> SendAsync(string openidList,
            string accessToken,
            string templateId,
            string first = "",
            string keyword1 = "",
            string keyword2 = "",
            string keyword3 = "",
            string keyword4 = "",
            string keyword5 = "",
            string keyword6 = "",
            string keyword7 = "",
            string remark = "",
            string url = "",
            string page = "",
            string form_id = "",
            string emphasis_keyword = "keyword1.DATA",
            string appid = "",
            string path = "pages/index/index")
        {
            var _result = (true, "");
            foreach (var openid in openidList.Split(','))
            {
                var apiurl = $"https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={accessToken}";

                if (openid.IsNullOrEmptyOrWhiteSpace())
                    break;
                var jsonString = new StringBuilder();
                jsonString.Append($@"{{'touser':'{openid}','template_id':'{templateId.Trim()}',");
                if (!url.IsNullOrEmptyOrWhiteSpace())
                    jsonString.Append($@"'url':'{url}',");
                jsonString.Append($@"'data':{{");
                if (!first.IsNullOrEmptyOrWhiteSpace())
                    jsonString.Append($@"'first':{{'value':'{first}'}},");
                if (!keyword1.IsNullOrEmptyOrWhiteSpace())
                    jsonString.Append($@"'keyword1':{{'value':'{keyword1}'}},");
                if (!keyword2.IsNullOrEmptyOrWhiteSpace())
                    jsonString.Append($@"'keyword2':{{'value':'{keyword2}'}},");
                if (!keyword3.IsNullOrEmptyOrWhiteSpace())
                    jsonString.Append($@"'keyword3':{{'value':'{keyword3}'}},");
                if (!keyword4.IsNullOrEmptyOrWhiteSpace())
                    jsonString.Append($@"'keyword4':{{'value':'{keyword4}'}},");
                if (!keyword5.IsNullOrEmptyOrWhiteSpace())
                    jsonString.Append($@"'keyword5':{{'value':'{keyword5}'}},");
                if (!keyword6.IsNullOrEmptyOrWhiteSpace())
                    jsonString.Append($@"'keyword6':{{'value':'{keyword6}'}},");
                if (!keyword7.IsNullOrEmptyOrWhiteSpace())
                    jsonString.Append($@"'keyword7':{{'value':'{keyword7}'}},");
                if (!remark.IsNullOrEmptyOrWhiteSpace())
                    jsonString.Append($@"'remark':{{'value':'{remark}'}},");
                jsonString.Append($"}}");
                if (!page.IsNullOrEmptyOrWhiteSpace() && !form_id.IsNullOrEmptyOrWhiteSpace())
                {
                    //小程序
                    apiurl =
                        $"https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token={accessToken}";
                    jsonString.Append(
                        $@",'page':'{page}','form_id':'{form_id}','emphasis_keyword':'{emphasis_keyword}'");
                }
                else if (!appid.IsNullOrEmptyOrWhiteSpace())
                {
                    //公众号跳转小程序
                    jsonString.Append($@",'miniprogram':{{'appid':'{appid}','pagepath':'{path}'}}");
                }

                jsonString.Append($"}}");
                var jsondata = JObject.Parse(jsonString.ToString());

                var result = await HttpEx.PostAsync<dynamic>(apiurl, jsondata);

                if (result.errmsg != "ok")
                {
                    _logger.Warn(JsonConvert.SerializeObject(result));
                    _result.Item1 = false;
                    _result.Item2 += result.errmsg;
                }
            }

            return _result;
        }
    }
}