using System.Net.Http;
using System.Threading.Tasks;

namespace TTWork.Abp.Oss.UpYun
{
    public interface IUpyunApi
    {
        Task<byte[]> GetBytesAsync(string imgUrl);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);
    }

    public class UpyunApi : IUpyunApi
    {
        private readonly HttpClient _client;

        public UpyunApi(HttpClient client)
        {
            _client = client;
        }

        public virtual async Task<byte[]> GetBytesAsync(string imgUrl)
        {
            return await _client.GetByteArrayAsync(imgUrl);
        }


        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
        {
            return await _client.SendAsync(message);
        }
    }
}