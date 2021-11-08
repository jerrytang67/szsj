using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using TTWork.Abp.Core.Authorization.Users;

namespace TtWork.ProjectName.Entities.Cms
{
    [Table("T_CmsContentEvents")]
    public class CmsContentEvent : Entity<int>, ICreationAudited<User>
    {
        public int CmsContentId { get; set; }

        public EventTypeEnum EventType { get; set; }

        [ForeignKey("CmsContentId")] public virtual CmsContent CmsContent { get; set; }

        #region interface property

        public User CreatorUser { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }

        #endregion
    }


    public class CmsContentEventDto : EntityDto<int>
    {
        public string EventType { get; set; }

        public DateTime CreationTime { get; set; }

        public CmsContent CmsContent { get; set; }

        public User CreatorUser { get; set; }
    }


    public enum EventTypeEnum
    {
        /// <summary>
        /// 收藏
        /// </summary>
        Favorite = 1,

        /// <summary>
        /// 分享
        /// </summary>
        Share = 2,

        /// <summary>
        /// 点赞
        /// </summary>
        Zan = 4,
    }
}