using System;
using System.Threading.Tasks;
using Abp.Dependency;
using MediatR;
using TTWork.Abp.AppManagement.Events;
using TTWork.Abp.Core.Events.Queries;
using TTWork.Abp.Core.Extensions;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace TTWork.Abp.Activity.WorkFlows
{
    public class WeixinTemplateMessageSendStep : StepBodyAsync, ITransientDependency
    {
        private readonly IWxSubscribeMessageSender _sender;
        private readonly IMediator _mediator;

        public string AppName { get; set; }

        public string[] openids { get; set; }
        public string template_id { get; set; }
        public object data { get; set; }
        public string page { get; set; }
        public string miniprogram_state { get; set; }
        public string lang { get; set; }

        public WeixinTemplateMessageSendStep(
            IWxSubscribeMessageSender sender,
            IMediator mediator
        )
        {
            _sender = sender;
            _mediator = mediator;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var app = await _mediator.Send(new QueryApp(AppName));
            var appid = app.GetValue("appid");
            var appSec = app.GetValue("appsec");

            var token = await _mediator.Send(new AccessTokenQuery(appid, appSec));

            var result = await _sender.SendAsync(openids, token, template_id, data, page, miniprogram_state, lang);

            if (result.Item1 != true)
                throw new Exception(result.Item2);

            return ExecutionResult.Next();
        }
    }
}