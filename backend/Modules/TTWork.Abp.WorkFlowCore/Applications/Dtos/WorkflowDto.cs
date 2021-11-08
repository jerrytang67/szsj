using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TTWork.Abp.WorkFlowCore.Models;
using WorkflowCore.Models;

namespace TTWork.Abp.WorkFlowCore.Applications.Dtos
{
    /// <summary>
    /// <see cref="PersistedWorkflow"/>
    /// </summary>
    public class WorkflowDto : EntityDto<Guid>
    {
        public string WorkflowDefinitionId { get; set; }

        public int Version { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public PersistedExecutionPointerCollection ExecutionPointers { get; set; }

        public long? NextExecution { get; set; }
        public DateTime? NextExecutionTime { get; set; }

        public string Data { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? CompleteTime { get; set; }

        public WorkflowStatus Status { get; set; }

        public string CreateUserIdentityName { get; set; }

        public List<ExecutionErrorDto> ExecutionErrors = new();
    }
}