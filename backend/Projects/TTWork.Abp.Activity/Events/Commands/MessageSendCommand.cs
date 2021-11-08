using System;
using System.Threading;
using System.Threading.Tasks;
using AwsomeApi.WeixinWork.Message;
using MediatR;
using TTWork.Abp.Activity.WorkFlows;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.WorkFlowCore.Models;
using WorkflowCore.Interface;

namespace TTWork.Abp.Activity.Events.Commands
{
    public class MessageSendCommand : INotification
    {
        public MessageType MessageType { get; }
        public IMessageDetail Detail { get; }

        public MessageSendCommand(MessageType messageType, IMessageDetail detail)
        {
            MessageType = messageType;
            Detail = detail;
        }
    }

    public class MessageSendQueryHandle : INotificationHandler<MessageSendCommand>
    {
        private readonly IWxSubscribeMessageSender _sender;
        private readonly IWorkflowHost _workflowHost;

        public MessageSendQueryHandle(
            IWxSubscribeMessageSender sender,
            IWorkflowHost workflowHost
        )
        {
            _sender = sender;
            _workflowHost = workflowHost;
        }

        public async Task Handle(MessageSendCommand request, CancellationToken cancellationToken)
        {
            if (request.MessageType == MessageType.WechatTemplate)
            {
                var detail = request.Detail as SendWechatTemplateDetail;

                var data = new WorkflowParamDictionary
                {
                    Data =
                    {
                        [nameof(WeixinTemplateMessageSendStep.AppName)] = detail!.AppName,
                        [nameof(WeixinTemplateMessageSendStep.openids)] = detail!.openids,
                        [nameof(WeixinTemplateMessageSendStep.template_id)] = detail!.template_id,
                        [nameof(WeixinTemplateMessageSendStep.data)] = detail!.data,
                        [nameof(WeixinTemplateMessageSendStep.page)] = detail!.page,
                        [nameof(WeixinTemplateMessageSendStep.miniprogram_state)] = detail!.miniprogram_state,
                        [nameof(WeixinTemplateMessageSendStep.lang)] = detail!.lang,
                    }
                };
                await _workflowHost.StartWorkflow("WeixinTemplateMessageSendWorkFlow", data);
            }


            if (request.MessageType == MessageType.WechatWorkApp)
            {
                var detail = request.Detail as SendWechatWorkAppDetail;
                var data = new WorkflowParamDictionary
                {
                    Data =
                    {
                        [nameof(WeixinWorkMessageSendStep.CorpId)] = "wwa26abd927b7b4c87",
                        [nameof(WeixinWorkMessageSendStep.CorpSecret)] = "T0f1eR_U8_4O3uM0ZSLvqwnOQm1FRQ1sIDtFwgu6kNc",
                        [nameof(WeixinWorkMessageSendStep.Message)] = detail!.Message
                    }
                };
                await _workflowHost.StartWorkflow(nameof(WeixinWorkMessageSendWorkFlow), data);
            }
        }
    }


    public class SendWechatWorkAppDetail : IMessageDetail
    {
        public IMessage Message { get; }

        public SendWechatWorkAppDetail(IMessage message)
        {
            Message = message;
        }
    }


    public class SendWechatTemplateDetail : IMessageDetail
    {
        public SendWechatTemplateDetail(string appName, string[] openids, string templateId, object data, string page = "", string miniprogramState = "formal", string lang = "zh_CN")
        {
            this.openids = openids;
            this.AppName = appName;
            this.template_id = templateId;
            this.data = data;
            this.page = page;
            this.miniprogram_state = miniprogramState;
            this.lang = lang;
        }

        public string AppName { get; set; }

        public string[] openids { get; set; }
        public string template_id { get; set; }
        public object data { get; set; }
        public string page { get; set; }
        public string miniprogram_state { get; set; }
        public string lang { get; set; }
    }


    public interface IMessageDetail
    {
    }

    [Flags]
    public enum MessageType
    {
        Email = 1,
        Sms = 2,
        WechatTemplate = 4,
        WechatWorkWebHook = 8,
        WechatWorkApp = 16,
        DTalkWebHook = 128
    }
}