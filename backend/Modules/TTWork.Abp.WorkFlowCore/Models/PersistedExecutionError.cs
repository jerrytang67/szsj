using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace TTWork.Abp.WorkFlowCore.Models
{
    public class PersistedExecutionError : Entity<Guid>
    {
        public string WorkflowId { get; set; }

        public string ExecutionPointerId { get; set; }

        public DateTime ErrorTime { get; set; }

        public string Message { get; set; }
    }
}