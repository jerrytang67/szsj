using System.Reflection;
using Abp.Dependency;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TTWork.Abp.WorkFlowCore.Interfaces;

namespace TTWork.Abp.WorkFlowCore
{
    internal class WorkflowInstaller : IWindsorInstaller
    {
        private readonly IIocResolver _iocResolver;

        private IAbpWorkflowRegisty _registy;

        public WorkflowInstaller(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            _registy = container.Resolve<IAbpWorkflowRegisty>();
            container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        private void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            if (!typeof(IAbpWorkflow).GetTypeInfo().IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                return;
            }

            var interfaces = handler.ComponentModel.Implementation.GetTypeInfo().GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!typeof(IAbpWorkflow).GetTypeInfo().IsAssignableFrom(@interface))
                {
                    continue;
                }

                _registy.RegisterWorkflow(handler.ComponentModel.Implementation);
            }
        }
    }
}