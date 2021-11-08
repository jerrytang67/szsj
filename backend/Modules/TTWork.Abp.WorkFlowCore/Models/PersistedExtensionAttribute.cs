using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace TTWork.Abp.WorkFlowCore.Models
{
    public class PersistedExtensionAttribute : Entity<Guid>
    {
        public string ExecutionPointerId { get; set; }

        [MaxLength(100)]
        public string AttributeKey { get; set;  }

        public string AttributeValue { get; set; }

        [ForeignKey("ExecutionPointerId")]
        public PersistedExecutionPointer ExecutionPointer { get; set; }
    }
}