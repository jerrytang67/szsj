using Abp.Application.Services.Dto;

namespace TtWork.ProjectName.Apis.Cms.Dtos
{
    public class CmsCategoryDto : EntityDto<int>
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}