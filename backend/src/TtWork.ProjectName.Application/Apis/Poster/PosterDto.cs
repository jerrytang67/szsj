using Abp.Application.Services.Dto;

namespace TtWork.ProjectName.Apis.Poster
{
    public class PosterDto : EntityDto<int>
    {
        public string Url { get; set; }
        public string BgImageUrl { get; set; } = "";
        public int BgWidth { get; set; } = 375; // w:h = 9:16
        public int BgHeight { get; set; } = 667;
        public BoxDetail MainImage { get; set; } = new BoxDetail(0, 150, 350, 150, false);
        public BoxDetail QrImage { get; set; } = new BoxDetail(140, 470, 80, 80, true);
        public BoxDetail HeadImage { get; set; } = new BoxDetail(140, 20, 68, 68, true);
        public FontBoxDetail TitleText { get; set; } = new FontBoxDetail(0, 110, 350, 30, 18, 20, "Center");
        public FontBoxDetail DescText { get; set; } = new FontBoxDetail(0, 330, 350, 80, 18, 40);
    }
}