using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TtWork.ProjectName.Definitions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TTWork.Abp.Core.Organizations;
using TTWork.Abp.FeatureManagement.Domain;
using TTWork.Abp.FeatureManagement.Features;
using Xunit;

namespace TtWork.ProjectName.Tests.Modules
{
    public class FeatureTests : AbpTestBase
    {
        private IFeatureDefinitionManager _definitionManager;
        private IFeatureValueProviderManager _valueProvider;
        private IRepository<AbpFeature, Guid> _repository;
        private IFeatureProvider _provider;
        private ICurrentOrganization _currentOrganization;
        private IAbpFeatureChecker _checker;

        public FeatureTests()
        {
            _definitionManager = Resolve<IFeatureDefinitionManager>();

            _valueProvider = Resolve<IFeatureValueProviderManager>();

            _repository = Resolve<IRepository<AbpFeature, Guid>>();

            _provider = Resolve<IFeatureProvider>();

            _currentOrganization = Resolve<ICurrentOrganization>();

            _checker = Resolve<IAbpFeatureChecker>();
        }

        [Fact]
        public async Task DefinitionTest()
        {
            var audits = _definitionManager.GetAll();

            // define in mall Module
            audits.Count.ShouldBe(2);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task ValueProviderTest()
        {
            var providers = _valueProvider.Providers;

            // define in Module
            providers.Count.ShouldBe(3);

            await Task.CompletedTask;
        }


        [Fact]
        public async Task FeaturesTests()
        {
            AbpSession.UserId = 2;
            AbpSession.TenantId = 1;

            var gEntity = new AbpFeature(
                ProjectNameFeature.Activity.Fassion.Enable,
                true,
                "G",
                null
            );

            var tEntity = new AbpFeature(
                ProjectNameFeature.Activity.Fassion.Enable,
                true,
                "T",
                "1",
                "false");

            var oEntity = new AbpFeature(
                ProjectNameFeature.Activity.Fassion.Enable,
                true,
                "O",
                "1",
                "true");

            await UsingDbContextAsync(async context =>
            {
                await context.AbpFeatures.AddAsync(gEntity);

                await context.AbpFeatures.AddAsync(tEntity);

                await context.AbpFeatures.AddAsync(oEntity);

                await context.SaveChangesAsync();

                var list = await context.AbpFeatures.ToListAsync();

                list.Count.ShouldBe(3);
            });


            //Act
            var result = await _provider.GetOrNullAsync(ProjectNameFeature.Activity.Fassion.Enable);

            //Assert
            gEntity.Enable.ShouldBe(true);
            gEntity.ProviderName.ShouldBe("G");
            gEntity.Name.ShouldBe(ProjectNameFeature.Activity.Fassion.Enable);

            result.ShouldNotBeNull();
            result.ShouldBe(tEntity.Id);

            (await _checker.IsEnabledAsync(ProjectNameFeature.Activity.Fassion.Enable)).ShouldBe(false);


            _currentOrganization.Change(1);

            var result_ou = await _provider.GetOrNullAsync(ProjectNameFeature.Activity.Fassion.Enable);
            result_ou.ShouldNotBeNull();
            result_ou.ShouldBe(oEntity.Id);

            (await _checker.IsEnabledAsync(ProjectNameFeature.Activity.Fassion.Enable)).ShouldBe(true);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task FeatureCheckerTests()
        {
            AbpSession.UserId = 2;
            AbpSession.TenantId = 1;
            
            var oEntity = new AbpFeature(
                ProjectNameFeature.Activity.Fassion.Enable,
                true,
                "O",
                "1",
                "true");
            _currentOrganization.Change(1);

            await UsingDbContextAsync(async context => { await context.AbpFeatures.AddAsync(oEntity); });

            (await _checker.IsEnabledAsync(ProjectNameFeature.Activity.Fassion.Enable)).ShouldBe(true);
            (await _checker.GetValueAsync<bool>(ProjectNameFeature.Activity.Fassion.Enable)).ShouldBe(true);
            (await _checker.GetValueAsync<string>(ProjectNameFeature.Activity.Fassion.Enable)).ShouldBe("true");
            (await _checker.GetValueAsync(ProjectNameFeature.Activity.Fassion.Enable)).ShouldBe("true");
        }
    }
}