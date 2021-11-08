using Abp.Application.Services.Dto;

namespace TTWork.Abp.Timeline.Domains
{
    /// <summary>
    ///<see cref=""TimelineCategory/>
    /// </summary>
    public class TimelineCategoryDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}