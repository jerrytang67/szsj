using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using TTWork.Abp.Core.Authorization.Users;

namespace TtWork.ProjectName.Entities.Cms
{
    [Table("T_CmsContents")]
    public class CmsContent : Entity<int>, IAudited<User>, ISoftDelete, IMayHaveTenant
    {
        public int CategoryId { get; set; }
        [StringLength(256)] public string Title { get; set; }

        [StringLength(256)] public string TitleImageUrl { get; set; }
        public int Status { get; set; } = 1; // 0 :关闭  1:显示 2:TOP

        public int Sort { get; set; }

        public int LinkType { get; set; } = 0;

        [StringLength(256)] public string LinkUrl { get; set; }

        public string Content { get; set; }
        public virtual CmsCategory Category { get; set; }

        /// <summary>
        /// 查看次数
        /// </summary>
        public int ViewCount { get; private set; }

        #region implememnts

        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public User CreatorUser { get; set; }
        public User LastModifierUser { get; set; }
        public bool IsDeleted { get; set; }

        #endregion

        public void SetViewCount()
        {
            this.ViewCount += 1;
        }

        public int? TenantId { get; set; }

        public virtual ICollection<CmsContentEvent> CmsContentEvents { get; set; }
    }

    [Table("T_CmsCategories")]
    public class CmsCategory : Entity<int>, IMayHaveTenant
    {
        [StringLength(64)] public string Name { get; set; }

        [StringLength(256)] public string ImageUrl { get; set; }

        public virtual ICollection<CmsContent> Contents { get; set; }

        public int? TenantId { get; set; }
    }
}