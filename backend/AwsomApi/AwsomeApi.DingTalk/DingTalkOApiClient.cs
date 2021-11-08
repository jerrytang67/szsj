using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace AwsomeApi.DingTalk
{
    public class DingTalkOApiClient : IDingTalkClient
    {
        private HttpClient _httpClient;

        public DingTalkOApiClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://oapi.dingtalk.com");
            _httpClient = httpClient;
        }

        public async Task<object> Robot_Send(string access_token, string secret, object data)
        {
            var url = signUrl($"https://oapi.dingtalk.com/robot/send?access_token={access_token}", secret);

            var response = Response();

            return JsonSerializer.Deserialize<object>(await response.Result.Content.ReadAsStringAsync());

            Task<HttpResponseMessage> Response()
            {
                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync(url, content);
                return response;
            }
        }

        /// <summary>
        /// <see cref="https://ding-doc.dingtalk.com/doc#/serverapi2/qf2nxq"/>
        /// 方式二，加签 第一步，把timestamp+"\n"+密钥当做签名字符串，使用HmacSHA256算法计算签名，然后进行Base64 encode，最后再把签名参数再进行urlEncode，得到最终的签名（需要使用UTF-8字符集）。
        /// </summary>
        /// <param name="url"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        private static string signUrl(string url, string secret)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var stringToSign = $"{timestamp}\n{secret}";
            var sign = WebUtility.UrlEncode(Convert.ToBase64String(HmacSHA256(stringToSign, secret)));
            url = $"{url}&timestamp={timestamp}&sign={sign}";
            return url;
        }

        private static byte[] HmacSHA256(string message, string secret)
        {
            using var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            return hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(message));
        }
    }
}