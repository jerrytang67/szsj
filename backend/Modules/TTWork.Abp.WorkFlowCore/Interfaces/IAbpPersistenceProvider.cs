using System;
using System.Threading.Tasks;
using TTWork.Abp.WorkFlowCore.Models;
using WorkflowCore.Interface;

namespace TTWork.Abp.WorkFlowCore.Interfaces
{
    public interface IAbpPersistenceProvider : IPersistenceProvider
    {
        Task<PersistedWorkflow> GetPersistedWorkflow(Guid id);

        Task<PersistedExecutionPointer> GetPersistedExecutionPointer(string id);
        Task<PersistedWorkflowDefinition> GetPersistedWorkflowDefinition(string id, int version);
    }
}