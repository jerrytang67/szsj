using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using TtWork.ProjectName.Authorization.Users;
using JetBrains.Annotations;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Authorization.Users;

namespace TtWork.ProjectName.Entities
{
    [Table("T_Comments")]
    public class Comment : Entity<int>, IFullAudited<User>, ISoftDelete, IExtendableObject
    {
        [NotNull]
        [StringLength(256)] public virtual string HeadImgUrl { get; set; }

        [NotNull]
        [StringLength(32)] public virtual string Nickname { get; set; }

        public virtual int HostId { get; set; }

        /// <summary>
        /// <permission cref="HostTypeEnum"></permission>
        /// </summary>
        public virtual HostTypeEnum HostType { get; set; }

        /// <summary>
        /// <see cref="ClientTypeEnum"/>
        /// </summary>
        public virtual ClientTypeEnum FromClient { get; set; }

        [StringLength(512)]
        public virtual string Content { get; set; }

        [StringLength(512)]
        public virtual string ReplyContent { get; set; }

        /// <summary>
        /// 1 
        /// </summary>
        public virtual int Status { get; set; } = 0;       // 第1位审核位 ,第2位待定


        [NotMapped]
        public string formId
        {
            get => this.GetData<string>("formId") ?? "";
            set => this.SetData("formId", value);
        }
        [NotMapped]
        public string openid
        {
            get => this.GetData<string>("openid") ?? "";
            set => this.SetData("openid", value);
        }
        
        [StringLength(512)]
        public virtual string ExtensionData { get; set; }

        #region IFullAudited
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public User CreatorUser { get; set; }
        public User LastModifierUser { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }
        public User DeleterUser { get; set; }

        #endregion
    }

    

    public enum HostTypeEnum
    {
        [Display(Name = "Default")] Default = 0
    }
}