using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using WorkflowCore.Models;

namespace TTWork.Abp.WorkFlowCore.Models
{
    public class PersistedWorkflow : Entity<Guid>
    {
        [MaxLength(200)] public string WorkflowDefinitionId { get; set; }

        public int Version { get; set; }

        [MaxLength(500)] public string Description { get; set; }

        [MaxLength(200)] public string Reference { get; set; }

        public virtual PersistedExecutionPointerCollection ExecutionPointers { get; set; } = new PersistedExecutionPointerCollection();

        public long? NextExecution { get; set; }

        public string Data { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? CompleteTime { get; set; }

        public WorkflowStatus Status { get; set; }

        [MaxLength(64)] public string CreateUserIdentityName { get; set; }

        // public virtual ICollection<PersistedExecutionError> ExecutionErrors { get; set; } = new List<PersistedExecutionError>();
    }
}