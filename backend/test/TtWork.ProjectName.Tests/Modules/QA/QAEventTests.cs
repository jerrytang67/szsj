using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Shouldly;
using TTWork.Abp.QA.Applications;
using TTWork.Abp.QA.Applications.Dtos;
using TTWork.Abp.Timeline.Applications;
using Xunit;

namespace TtWork.ProjectName.Tests.Modules.QA
{
    public class QAPlanTests : AbpTestBase
    {
        private readonly QAPlanAppService _service;

        public QAPlanTests()
        {
            _service = Resolve<QAPlanAppService>();
        }

        [Fact]
        public async Task Create_Test()
        {
            var j = new JObject {{"img", "src"}, {"img2", 8}};
            // Act
            var dto = await _service.CreateAsync(
                new QAPlanCreateOrUpdateDto()
                {
                    Settings = j
                });

            var dto2 = await _service.CreateAsync(
                new QAPlanCreateOrUpdateDto()
                {
                    Settings = null
                });

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.QAPlans.ToListAsync();
                entity[0].ShouldNotBeNull();
                entity[0].Settings["img"].ShouldBe("src");
                entity[0].Settings["img2"].ShouldBe(8);
                
                entity[1].ShouldNotBeNull();
                entity[1].Settings.ShouldNotBe(null);
                entity[1].Settings.Count.ShouldBe(0);
            });
        }
    }
}