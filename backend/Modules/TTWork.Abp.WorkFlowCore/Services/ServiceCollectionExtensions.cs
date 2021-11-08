using System;
using Microsoft.Extensions.DependencyInjection;
using TTWork.Abp.WorkFlowCore.Steps;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace TTWork.Abp.WorkFlowCore.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAbpWorkflow(this IServiceCollection services, Action<WorkflowOptions> setupAction = null )
        {
            services.AddSingleton<IPersistenceProvider, AbpPersistenceProvider>();
            
            services.AddWorkflow(options =>
            {
                options.UsePersistence(sp => sp.GetService<AbpPersistenceProvider>());
                
                setupAction?.Invoke(options);
            });
            services.AddWorkflowDSL();
            return services;
        }
    }
}