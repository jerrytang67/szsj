namespace TtWork.ProjectName.Apis.Poster
{
    public class BoxDetail : IBoxDetail
    {
        public BoxDetail()
        {
        }


        public BoxDetail(int x, int y, int width, int height, bool lockAspect = false)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            LockAspect = lockAspect;
        }

        public bool Enable { get; set; }
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Width { get; set; } = 100;
        public int Height { get; set; } = 100;
        public bool LockAspect { get; set; } = false;
    }
}