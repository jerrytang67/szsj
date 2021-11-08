using System;

namespace TTWork.Abp.WorkFlowCore.Interfaces
{
    public interface IAbpWorkflowRegisty
    {
        void RegisterWorkflow(Type type);
    }
}