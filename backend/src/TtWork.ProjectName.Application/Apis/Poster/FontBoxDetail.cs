using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;

namespace TtWork.ProjectName.Apis.Poster
{
    public class FontBoxDetail : IFontBoxDetail
    {
        public FontBoxDetail()
        {
        }

        public FontBoxDetail(int x, int y, int width, int height, int fontSize, int fontMaxLength, string fontAlign = "Left")
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            FontSize = fontSize;
            FontMaxLength = fontMaxLength;
            FontAlign = fontAlign;
        }

        public bool Enable { get; set; } = true;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Width { get; set; } = 100;
        public int Height { get; set; } = 100;
        public int FontSize { get; set; } = 16;
        public int FontMaxLength { get; set; } = 20;
        public string FontColor { get; set; } = "Black";

        public string FontAlign = "Left";

        public TextGraphicsOptions GetTextOptions()
        {
            return new TextGraphicsOptions()
            {
                TextOptions = new TextOptions
                {
                    WrapTextWidth = Width,
                    HorizontalAlignment = FontAlign == "Left" ? HorizontalAlignment.Left : HorizontalAlignment.Center
                },
            };
        }

        public Color GetColor()
        {
            return FontColor.StartsWith('#') ? Color.ParseHex(FontColor) : Color.Parse(FontColor);
        }
    }
}