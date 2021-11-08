using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TTWork.Abp.Timeline.Applications;
using TTWork.Abp.Timeline.Applications.Dtos;
using TtWork.ProjectName.Apis.Users.Dto;
using Xunit;

namespace TtWork.ProjectName.Tests.Modules.Timeline
{
    public class TimelineEventTests : AbpTestBase
    {
        private readonly TimelineEventAppService _service;

        public TimelineEventTests()
        {
            _service = Resolve<TimelineEventAppService>();
        }

        [Fact]
        public async Task Create_Test()
        {
            // Act
            await _service.CreateAsync(
                new TimelineEventCreateOrUpdateDto());

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.TimelineEvents.FirstOrDefaultAsync();
                entity.ShouldNotBeNull();
                // entity.Tag.ShouldBe("Trigger Created");
            });
        }
    }
}