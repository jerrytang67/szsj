using Abp.Dependency;
using AwsomeApi.WeixinWork.Message;
using TTWork.Abp.WorkFlowCore.Interfaces;
using TTWork.Abp.WorkFlowCore.Models;
using WorkflowCore.Interface;

namespace TTWork.Abp.LaborUnion.WorkFlows
{
    public class WeixinWorkMessageSendWorkFlow : IAbpWorkflow, ITransientDependency
    {
        public string Id => "WeixinWorkMessageSendWorkFlow";

        public int Version => 1;

        public void Build(IWorkflowBuilder<WorkflowParamDictionary> builder)
        {
            builder.StartWith<WeixinWorkMessageSendStep>()
                .Input(step => step.CorpId, data => data.Data[nameof(WeixinWorkMessageSendStep.CorpId)] as string)
                .Input(step => step.CorpSecret, data => data.Data[nameof(WeixinWorkMessageSendStep.CorpSecret)] as string)
                .Input(step => step.Message, data => data.Data[nameof(WeixinWorkMessageSendStep.Message)] as IMessage)
                ;
        }
    }
}