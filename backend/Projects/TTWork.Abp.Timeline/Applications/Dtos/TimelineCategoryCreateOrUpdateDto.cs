using Abp.Application.Services.Dto;

namespace TTWork.Abp.Timeline.Applications.Dtos
{
    /// <summary>
    ///<see cref=""TimelineCategory/>
    /// </summary>
    public class TimelineCategoryCreateOrUpdateDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}