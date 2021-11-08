using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TTWork.Abp.Timeline
{
    public class EnumClass
    {
        public enum TimelineEventState
        {
            草稿 = 0,
            已发布 = 1,
            置顶 = 11
        }

        public enum TimelineFileType
        {
            Unknown = 0,
            Image = 1,
            Doc = 21,
            Video = 31
        }
    }
}