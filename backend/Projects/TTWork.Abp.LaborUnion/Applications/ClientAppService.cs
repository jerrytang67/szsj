using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Web.Models;
using AwsomeApi.WeixinWork;
using AwsomeApi.WeixinWork.Message;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TT.Extensions.Redis;
using TTWork.Abp.Core;
using TTWork.Abp.LaborUnion.Events;
using TTWork.Abp.LaborUnion.Events.Commands;
using TTWork.Abp.LaborUnion.WorkFlows;
using TTWork.Abp.WorkFlowCore.Models;
using WorkflowCore.Interface;

namespace TTWork.Abp.LaborUnion.Applications
{
    public class LaborUnionSettings
    {
        public const string Position = "LaborUnionSettings";
        public Dictionary<string, string> Data { get; set; } = new();
    }

    public class ClientAppService : AbpAppServiceBase
    {
        private readonly IWeixinWorkApi _weixinWorkApi;
        private readonly IRedisClient _redisClient;
        private readonly IMediator _mediator;
        private readonly LaborUnionSettings settings = new LaborUnionSettings();

        public ClientAppService(
            IConfiguration configuration,
            IWeixinWorkApi weixinWorkApi,
            IRedisClient redisClient,
            IMediator mediator)
        {
            _weixinWorkApi = weixinWorkApi;
            _redisClient = redisClient;
            _mediator = mediator;
            // settings = options.Value;
            configuration.GetSection(LaborUnionSettings.Position).Bind(settings);
        }

        public async Task<object> GetSettings()
        {
            return await Task.FromResult(settings);
        }
    }
}