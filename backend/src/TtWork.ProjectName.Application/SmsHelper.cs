using System;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;

namespace TtWork.ProjectName
{
    public static class SmsHelper
    {
        /// <summary>
        /// 发送阿里云短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="templateCode"></param>
        /// <param name="json"></param>
        public static void SendAcsCode(string mobile, string code, int ourid = 0)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                throw new Exception("mobile不能为空");
            }
            var product = "Dysmsapi"; //短信API产品名称（短信产品名固定，无需修改）
            var domain = "dysmsapi.aliyuncs.com"; //短信API产品域名（接口地址固定，无需修改）
            var accessKeyId = "LTAI4FygBKtCPFDAc36AMXt6"; //你的accessKeyId，参考本文档步骤2
            var accessKeySecret = "foLD0VQjld8ZFBIcw3Yao2we7x46PD"; //你的accessKeySecret，参考本文档步骤2

            var profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            profile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();

            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式，发送国际/港澳台消息时，接收号码格式为00+国际区号+号码，如“0085200000000”
                request.PhoneNumbers = mobile;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = "吴江优选";
                //必填:短信模板-可在短信控制台中找到，发送国际/港澳台消息时，请使用国际/港澳台短信模版
                request.TemplateCode = "SMS_199200362";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = "{\"code\":\"" + code + "\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = ourid.ToString();
                ; // "yourOutId";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                if (sendSmsResponse.BizId == null)
                    throw new ApplicationException(sendSmsResponse.Message);
                Console.WriteLine(sendSmsResponse.Message);
            }
            catch (ServerException ex)
            {
                throw new ApplicationException(ex.Message);
                //System.Console.WriteLine("Hello World!");
            }
            catch (ClientException ex)
            {
                throw new ApplicationException(ex.Message);
                //System.Console.WriteLine("Hello World!");
            }
        }
    }
}