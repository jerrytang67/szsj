using System;
using TTWork.Abp.WorkFlowCore.Models;

namespace TTWork.Abp.WorkFlowCore.Applications.Dtos
{
    /// <summary>
    /// <see cref="PersistedExecutionError"/>
    /// </summary>
    public class ExecutionErrorDto
    {
        public DateTime ErrorTime { get; set; }

        public string Message { get; set; }
    }
}