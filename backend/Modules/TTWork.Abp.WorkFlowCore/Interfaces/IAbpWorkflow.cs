using Abp.Dependency;
using TTWork.Abp.WorkFlowCore.Models;
using WorkflowCore.Interface;

namespace TTWork.Abp.WorkFlowCore.Interfaces
{
    public interface IAbpWorkflow : IWorkflow<WorkflowParamDictionary>
    {
    }
}