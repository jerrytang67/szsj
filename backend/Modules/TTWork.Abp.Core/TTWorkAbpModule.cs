using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.FluentValidation;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using FluentValidation;
using MediatR;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.Core.Localization;
using TTWork.Abp.Core.Organizations;

namespace TTWork.Abp.Core
{
    [DependsOn(
        typeof(AbpFluentValidationModule),
        typeof(AbpAutoMapperModule)
    )]
    // ReSharper disable once InconsistentNaming
    public class TTWorkAbpCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<AbpAuthorizationProvider>();

            if (!IocManager.IsRegistered<ICurrentOrganizationAccessor>())
            {
                IocManager.Register<ICurrentOrganizationAccessor, AsyncLocalCurrentShopAccessor>(DependencyLifeStyle.Singleton);
            }

            var container = this.IocManager.IocContainer;
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
            container.Kernel.AddHandlersFilter(new ContravariantFilter());

            container.Register(Component.For<IMediator>().ImplementedBy<Mediator>().LifestylePerThread());
            container.Register(Component.For<ServiceFactory>().UsingFactoryMethod<ServiceFactory>(k => (type =>
            {
                var enumerableType = type
                    .GetInterfaces()
                    .Concat(new[] {type})
                    .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                return enumerableType != null ? k.ResolveAll(enumerableType.GetGenericArguments()[0]) : k.Resolve(type);
            })));

            TtWorkLocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void PostInitialize()
        {
            ValidatorOptions.LanguageManager.Culture = new CultureInfo("zh-CN");
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TTWorkAbpCoreModule).GetAssembly());
            
            IocManager.RegisterMediatRAssembly<TTWorkAbpCoreModule>();
        }
    }
}