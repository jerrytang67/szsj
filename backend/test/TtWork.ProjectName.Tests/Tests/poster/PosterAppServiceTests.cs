using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using TtWork.ProjectName.Apis.Poster;
using TtWork.ProjectName.Dto;
using Shouldly;
using Xunit;

namespace TtWork.ProjectName.Tests.Tests.poster
{
    public class PosterAppServiceTests : AbpTestBase
    {
        private readonly IPosterAppService _service;

        public PosterAppServiceTests()
        {
            _service = Resolve<IPosterAppService>();
        }

        // [Fact]
        public async Task GeneratePosterImage()
        {
            AbpSession.UserId = 2;
            AbpSession.TenantId = 1;

            var bgUrl = "https://img2.gongyi.la/hudongdaxue/2020/06/104303_Snipaste_2020-06-11_10-41-35.png";

            // Act
            await _service.CreateAsync(new PosterDto
            {
                BgImageUrl = bgUrl,
                BgWidth = 375,
                BgHeight = 667,
                HeadImage = new BoxDetail(10, 20, 40, 40, true)
            });
            var dto = await _service.GetAsync(new EntityDto
            {
                Id = 1
            });

            // Assert
            dto.BgImageUrl.ShouldBe(bgUrl);
            dto.BgWidth.ShouldBe(375);
            dto.BgHeight.ShouldBe(667);
            dto.HeadImage.Height.ShouldBe(40);
            dto.HeadImage.LockAspect.ShouldBe(true);


            // Act 
            var url = await _service.GetPosterImage(new EntityDto {Id = 1});

            url.Length.ShouldBeGreaterThan(0);

            var dto2 = await _service.GetAsync(new EntityDto {Id = 1});

            dto2.Url.ShouldBe(url);

            // dto.Url.Length.ShouldBeGreaterThan(0);
        }
    }
}