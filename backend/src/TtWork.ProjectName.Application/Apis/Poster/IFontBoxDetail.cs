namespace TtWork.ProjectName.Apis.Poster
{
    public interface IFontBoxDetail : IBoxDetail
    {
        public int FontSize { get; set; }

        public int FontMaxLength { get; set; }
    }
}