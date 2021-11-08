using JetBrains.Annotations;

namespace TTWork.Abp.QA.Applications
{
    public class RankListItem
    {
        public string ImageUrl { get; set; }
        [CanBeNull] public string Name { get; set; }

        public string PhoneNumber { get; set; }

        [CanBeNull] public string Town { get; set; }
        public long UserId { get; set; }
        public int RightCount { get; set; }
        public int PointCount { get; set; }
        
        public int SpendTime { get; set; }
    }
}