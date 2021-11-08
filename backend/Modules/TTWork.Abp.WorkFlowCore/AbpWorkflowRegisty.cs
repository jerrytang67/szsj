using System;
using Abp;
using Abp.Dependency;
using TTWork.Abp.WorkFlowCore.Interfaces;
using TTWork.Abp.WorkFlowCore.Models;
using WorkflowCore.Interface;

namespace TTWork.Abp.WorkFlowCore
{
    //Abp工作流注册实现
    public class AbpWorkflowRegisty : IAbpWorkflowRegisty, ISingletonDependency
    {
        private IWorkflowRegistry _workflowRegistry;
        private readonly IIocManager _iocManager;

        public AbpWorkflowRegisty(
            IWorkflowRegistry workflowRegistry,
            IIocManager iocManager)
        {
            _workflowRegistry = workflowRegistry;
            _iocManager = iocManager;
        }


        public void RegisterWorkflow(Type type)
        {
            var workflow = _iocManager.Resolve(type);
            if (!(workflow is IAbpWorkflow))
            {
                throw new AbpException("RegistType must implement from AbpWorkflow!");
            }

            _workflowRegistry.RegisterWorkflow(workflow as IWorkflow<WorkflowParamDictionary>);
        }
    }
}