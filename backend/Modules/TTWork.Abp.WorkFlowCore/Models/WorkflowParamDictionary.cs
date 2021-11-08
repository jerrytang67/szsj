using System.Collections.Generic;

namespace TTWork.Abp.WorkFlowCore.Models
{
    public class WorkflowParamDictionary
    {
        public Dictionary<string, object> Data { get; set; } = new();
    }
}