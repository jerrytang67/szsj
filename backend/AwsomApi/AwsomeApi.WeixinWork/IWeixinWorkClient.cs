using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AwsomeApi.WeixinWork.Message;
using AwsomeApi.WeixinWork.WeixinResult;

namespace AwsomeApi.WeixinWork
{
    public interface IWeixinWorkApi
    {
        public Task<TokenResult> GetToken(string corpid,
            string corpsecret);

        public Task<MessageResult> MessageSend(string accessToken, IMessage message);
    }

    public class WeixinWorkApi : IWeixinWorkApi
    {
        private readonly HttpClient _client;

        public WeixinWorkApi(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://qyapi.weixin.qq.com");
        }


        /// <summary>
        /// https://work.weixin.qq.com/api/doc/90000/90135/91039
        /// </summary>
        /// <param name="corpid">企业ID</param>
        /// <param name="corpsecret">应用的凭证密钥</param>
        /// <returns></returns>
        public async Task<TokenResult> GetToken(
            string corpid,
            string corpsecret)
        {
            var response = await _client.GetAsync($"cgi-bin/gettoken?corpid={corpid}&corpsecret={corpsecret}");
            var strJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TokenResult>(strJson);
        }


        /// <summary>
        /// https://work.weixin.qq.com/api/doc/90000/90135/90236
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<MessageResult> MessageSend(string accessToken, IMessage message)
        {
            var content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"cgi-bin/message/send?access_token={accessToken}", content);
            var strJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<MessageResult>(strJson);
        }
    }
}