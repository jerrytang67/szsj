namespace TtWork.ProjectName.Entities.Cms
{
    public class CreateOrUpdateCmsContentEventDto
    {
        /// <summary>
        /// <see cref="EventTypeEnum"/>
        /// </summary>
        public EventTypeEnum EventType { get; set; }

        public int CmsContentId { get; set; }

    }
}
