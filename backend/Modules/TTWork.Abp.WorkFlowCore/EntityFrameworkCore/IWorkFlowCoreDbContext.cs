using Microsoft.EntityFrameworkCore;
using TTWork.Abp.WorkFlowCore.Models;

namespace TTWork.Abp.WorkFlowCore.EntityFrameworkCore
{
    public interface IWorkFlowCoreDbContext
    {
        public DbSet<PersistedEvent> PersistedEvents { get; set; }
        public DbSet<PersistedExecutionError> PersistedExecutionErrors { get; set; }
        public DbSet<PersistedExecutionPointer> PersistedExecutionPointers { get; set; }
        public DbSet<PersistedExtensionAttribute> PersistedExtensionAttributes { get; set; }
        public DbSet<PersistedSubscription> PersistedSubscriptions { get; set; }
        public DbSet<PersistedWorkflow> PersistedWorkflows { get; set; }
        public DbSet<PersistedWorkflowDefinition> PersistedWorkflowDefinitions { get; set; }
    }
}