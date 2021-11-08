using System.Linq;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AutoMapper;
using TTWork.Abp.AppManagement;
using TTWork.Abp.AuditManagement.Applications.Dtos;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Extensions;

namespace TTWork.Abp.AuditManagement
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AppManagementModule)
    )]
    public class AuditManagementModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<AuditOptions>();
            Configuration.Modules.AuditModule().ValueProviders.Add<GlobalAuditValueProvider>();
            Configuration.Modules.AuditModule().ValueProviders.Add<TenantAuditValueProvider>();
            Configuration.Modules.AuditModule().ValueProviders.Add<OrganizationUnitAuditValueProvider>();

            // this line move to Web.Core , then test will fine
            // Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(AuditManagementModule).Assembly, moduleName: "AuditManagement", useConventionalHttpVerbs: true);


            Configuration.Modules.AbpAutoMapper().Configurators.Add(AuditManagementMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AuditManagementModule).GetAssembly());

            IocManager.RegisterMediatRAssembly<AuditManagementModule>();
        }
    }

    public class AuditManagementMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AuditFlowCreateOrEditDto, AuditFlow>()
                .ForMember(x => x.NodesMaxIndex,
                    opt => opt.MapFrom(x =>
                        x.AuditNodes.Max(xx => xx.Index)
                    ))
                .ReverseMap();

            cfg.CreateMap<User, UserDtoBase>();
        }
    }

    public static class AuditConfigurationExtensions
    {
        public static AuditOptions AuditModule(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<AuditOptions>();
        }
    }
}