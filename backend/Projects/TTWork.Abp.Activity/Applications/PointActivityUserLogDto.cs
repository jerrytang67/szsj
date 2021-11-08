using Abp.Application.Services.Dto;
using TTWork.Abp.Activity.Domains;

namespace TTWork.Abp.Activity.Applications
{
    /// <summary>
    /// <see cref="PointActivityUserLog"/>
    /// </summary>
    public class PointActivityUserLogDto : EntityDto<long>
    {
        public long ActivityId { get; init; }
        public int HelperNum { get; set; }
        public int Point { get; private set; }
        public EnumClass.PointActivityUserState State { get; set; }
        public long? SharedFrom { get; init; }
    }
}