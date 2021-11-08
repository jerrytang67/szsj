using Abp.Dependency;
using TTWork.Abp.WorkFlowCore.Interfaces;
using TTWork.Abp.WorkFlowCore.Models;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace TTWork.Abp.LaborUnion.WorkFlows
{
    public class WeixinTemplateMessageSendWorkFlow : IAbpWorkflow, ITransientDependency
    {
        public string Id => "WeixinTemplateMessageSendWorkFlow";

        public int Version => 1;

        public void Build(IWorkflowBuilder<WorkflowParamDictionary> builder)
        {
            builder.StartWith<WeixinTemplateMessageSendStep>()
                .Input(step => step.AppName, data => data.Data[nameof(WeixinTemplateMessageSendStep.AppName)] as string)
                .Input(step => step.openids, data => data.Data[nameof(WeixinTemplateMessageSendStep.openids)] as string[])
                .Input(step => step.template_id, data => data.Data[nameof(WeixinTemplateMessageSendStep.template_id)] as string)
                .Input(step => step.data, data => data.Data[nameof(WeixinTemplateMessageSendStep.data)])
                .Input(step => step.page, data => data.Data[nameof(WeixinTemplateMessageSendStep.page)] as string)
                .Input(step => step.miniprogram_state, data => data.Data[nameof(WeixinTemplateMessageSendStep.miniprogram_state)] as string)
                .Input(step => step.lang, data => data.Data[nameof(WeixinTemplateMessageSendStep.lang)] as string)
                .OnError(WorkflowErrorHandling.Terminate)
                ;
        }
    }
}