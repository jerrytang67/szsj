using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TTWork.Abp.Core;

namespace TTWork.Abp.Activity.Applications
{
    public class LinkunstClient
    {
        public HttpClient _client { get; private set; }

        public LinkunstClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("http://lkapi3.linkunst.com");
            // httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            // httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            _client = httpClient;
        }

        public async Task<string> TradeSummaryListAsync()
        {
            return await _client.GetStringAsync("/hadoop/Hadoop_api/tradeSummaryList?number=4");
        }

        public async Task<string> DetectionListAsync()
        {
            return await _client.GetStringAsync("/hadoop/Hadoop_Api/detectionList?number=4");
        }
    }


    public class ClientAppService : AbpAppServiceBase
    {
        private readonly LinkunstClient _linkunstClient;

        public ClientAppService(LinkunstClient linkunstClient)
        {
            _linkunstClient = linkunstClient;
        }

        public async Task<object> GetTradeSummaryList()
        {
            return await _linkunstClient.TradeSummaryListAsync();
            return JsonSerializer.Deserialize<JObject>(await _linkunstClient.TradeSummaryListAsync());
        }


        public async Task<object> GetDetectionList()
        {

            var str = await _linkunstClient.DetectionListAsync();
            return str;
            return JsonSerializer.Deserialize<JObject>(str);
        }
    }
}