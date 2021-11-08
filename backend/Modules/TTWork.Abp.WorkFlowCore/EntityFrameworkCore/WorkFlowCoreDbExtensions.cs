using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TTWork.Abp.WorkFlowCore.Models;

namespace TTWork.Abp.WorkFlowCore.EntityFrameworkCore
{
    public static class WorkFlowCoreDbExtensions
    {
        public static void ConfigureWorkFlowCore(this ModelBuilder modelBuilder)
        {
            var workflows = modelBuilder.Entity<PersistedWorkflow>();
            workflows.ToTable(WorkFlowCoreConsts.DbTablePrefix + "Workflows", WorkFlowCoreConsts.DbSchema);
            workflows.HasIndex(x => x.Id).IsUnique();
            workflows.HasIndex(x => x.NextExecution);

            var executionPointers = modelBuilder.Entity<PersistedExecutionPointer>();
            executionPointers.ToTable(WorkFlowCoreConsts.DbTablePrefix + "ExecutionPointers", WorkFlowCoreConsts.DbSchema);

            var executionErrors = modelBuilder.Entity<PersistedExecutionError>();
            executionErrors.ToTable(WorkFlowCoreConsts.DbTablePrefix + "ExecutionErrors", WorkFlowCoreConsts.DbSchema);

            var extensionAttributes = modelBuilder.Entity<PersistedExtensionAttribute>();
            extensionAttributes.ToTable(WorkFlowCoreConsts.DbTablePrefix + "ExtensionAttributes", WorkFlowCoreConsts.DbSchema);

            var subscriptions = modelBuilder.Entity<PersistedSubscription>();
            subscriptions.ToTable(WorkFlowCoreConsts.DbTablePrefix + "Subscriptions", WorkFlowCoreConsts.DbSchema);
            subscriptions.HasIndex(x => x.EventName);
            subscriptions.HasIndex(x => x.EventKey);

            var events = modelBuilder.Entity<PersistedEvent>();
            events.ToTable(WorkFlowCoreConsts.DbTablePrefix + "Events", WorkFlowCoreConsts.DbSchema);
            events.HasIndex(x => new {x.EventName, x.EventKey});
            events.HasIndex(x => x.EventTime);
            events.HasIndex(x => x.IsProcessed);


            modelBuilder.Entity<PersistedWorkflowDefinition>(b =>
            {
                b.ToTable(WorkFlowCoreConsts.DbTablePrefix + "WorkflowDefinitions", WorkFlowCoreConsts.DbSchema);

                b.Property(x => x.Steps).HasConversion(
                    v => JsonConvert.SerializeObject(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}),
                    v => JsonConvert.DeserializeObject<List<AbpStepBody>>(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
            });
        }
    }

    public class WorkFlowCoreConsts
    {
        public const string DbTablePrefix = "T_WorkFlow_";
        public const string DbSchema = null;
        public const int MaxNameLength = 64;
        public const int MaxValueLength = 256;
    }
}