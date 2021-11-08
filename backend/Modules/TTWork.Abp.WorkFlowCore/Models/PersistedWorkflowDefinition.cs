using System;
using System.Collections.Generic;
using Abp.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace TTWork.Abp.WorkFlowCore.Models
{
    public class PersistedWorkflowDefinition : Entity<string>
    {
        public int Version { get; set; }

        public string DataType { get; set; }
        
        public List<AbpStepBody> Steps { get; set; }
    }

    public class AbpStepBody
    {
        public string Id { get; set; }

        public string StepType { get; set; }
        
        public string NextStepId { get; set; }
        
        public Dictionary<string,string> Inputs { get; set; }
        
        public Dictionary<string,string> Outputs { get; set; }
        
        public string CancelCondition { get; set; }
    }
}