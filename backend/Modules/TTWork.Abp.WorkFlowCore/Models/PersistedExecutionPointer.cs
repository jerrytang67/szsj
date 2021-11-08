using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using WorkflowCore.Models;

namespace TTWork.Abp.WorkFlowCore.Models
{
    public class PersistedExecutionPointer : Entity<string>
    {
        public Guid WorkflowId { get; set; }
        
        public int StepId { get; set; }

        public bool Active { get; set; }

        public DateTime? SleepUntil { get; set; }

        public string PersistenceData { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [MaxLength(100)]
        public string EventName { get; set; }

        [MaxLength(100)]
        public string EventKey { get; set; }

        public bool EventPublished { get; set; }

        public string EventData { get; set; }

        [MaxLength(100)]
        public string StepName { get; set; }
        
        public int RetryCount { get; set; }

        public string Children { get; set; }

        public string ContextItem { get; set; }

        [MaxLength(100)]
        public string PredecessorId { get; set; }

        public string Outcome { get; set; }

        public PointerStatus Status { get; set; }

        public string Scope { get; set; }
        
        public ICollection<PersistedExtensionAttribute> ExtensionAttributes { get; set; } = new List<PersistedExtensionAttribute>();

        [ForeignKey("WorkflowId")]
        public PersistedWorkflow Workflow { get; set; }
    }
}