using TTWork.Abp.Core.Applications.Dtos;
using TtWork.ProjectName.Entities.Cms;

namespace TtWork.ProjectName.Apis.Cms.Dtos
{
    public class CmsContentEventGetAllInputDto : AppResultRequestDto
    {
        /// <summary>
        /// <see cref="EventTypeEnum"/>
        /// </summary>
        public EventTypeEnum? EventType { get; set; }


        public string CmsContentCategoryName { get; set; }

    }
}
