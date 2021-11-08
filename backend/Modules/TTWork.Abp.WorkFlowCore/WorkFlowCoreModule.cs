using System;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AutoMapper;
using TT.Extensions.Redis;
using TTWork.Abp.WorkFlowCore.Applications;
using TTWork.Abp.WorkFlowCore.Applications.Dtos;
using TTWork.Abp.WorkFlowCore.Models;

namespace TTWork.Abp.WorkFlowCore
{
    [DependsOn(typeof(AbpAspNetCoreModule))]
    public class WorkFlowCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<WorkFlowCoreOptions>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(WorkflowMapper.CreateMappings);

            //注册appservice
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(WorkFlowCoreModule).Assembly, moduleName: "WorkFlow", useConventionalHttpVerbs: true);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WorkFlowCoreModule).GetAssembly());
            IocManager.IocContainer.Install(new WorkflowInstaller(IocManager));
        }
    }

    public static class WorkFlowCoreConfigurationExtensions
    {
        public static WorkFlowCoreOptions WorkFlowCoreModule(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<WorkFlowCoreOptions>();
        }
    }

    public class WorkFlowCoreOptions
    {
    }


    public static class WorkflowMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<PersistedWorkflow, WorkflowDto>()
                .ForMember(x => x.CompleteTime, opt =>
                {
                    {
                        opt.PreCondition(t => t.CompleteTime.HasValue);
                        opt.MapFrom(t => t.CompleteTime.Value.ToLocalTime());
                    }
                })
                .ForMember(x => x.CreateTime, opt =>
                    opt.MapFrom(t => t.CreateTime.ToLocalTime())
                )
                .ForMember(x => x.NextExecutionTime, opt =>
                {
                    {
                        opt.PreCondition(t => t.NextExecution.HasValue);
                        opt.MapFrom(t => new DateTime(t.NextExecution.Value, DateTimeKind.Utc).ToLocalTime());
                    }
                })
                ;
        }
    }
}