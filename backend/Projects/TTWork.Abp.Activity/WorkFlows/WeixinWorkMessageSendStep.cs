using System;
using System.Threading.Tasks;
using Abp.Dependency;
using AwsomeApi.WeixinWork;
using AwsomeApi.WeixinWork.Message;
using TT.Extensions.Redis;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace TTWork.Abp.Activity.WorkFlows
{
    public class WeixinWorkMessageSendStep : StepBodyAsync, ITransientDependency
    {
        public string CorpId { get; set; }
        public string CorpSecret { get; set; }
        public IMessage Message { get; set; }

        private readonly IRedisClient _redisClient;
        private readonly IWeixinWorkApi _weixinWorkApi;

        public WeixinWorkMessageSendStep(
            IRedisClient redisClient,
            IWeixinWorkApi weixinWorkApi
        )
        {
            _redisClient = redisClient;
            _weixinWorkApi = weixinWorkApi;
        }


        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var corpid = CorpId; // "wwa26abd927b7b4c87";
            var corpsecret = CorpSecret; // "T0f1eR_U8_4O3uM0ZSLvqwnOQm1FRQ1sIDtFwgu6kNc";

            var key = $"{corpid}:{corpsecret}";
            var cache = await _redisClient.Database.StringGetAsync(key);
            var token = "";
            if (cache.HasValue)
            {
                token = cache.ToString();
            }
            else
            {
                var tokenResult = await _weixinWorkApi.GetToken(corpid, corpsecret);
                token = tokenResult.access_token;
                await _redisClient.Database.StringSetAsync(key, tokenResult.access_token, TimeSpan.FromSeconds(tokenResult.expires_in));
            }

            var message = await _weixinWorkApi.MessageSend(token, Message);

            if (message.errcode == 0)
                return ExecutionResult.Next();

            throw new Exception(message.errmsg);
        }
    }
}