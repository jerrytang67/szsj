using System;
using System.Diagnostics;
using Abp.Dependency;
using Castle.Core.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace TTWork.Abp.WorkFlowCore.Steps
{
    public class HelloWorld : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Hello world");
            return ExecutionResult.Next();
        }
    }
}