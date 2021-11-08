using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using TtWork.ProjectName.Authorization.Users;
using TTWork.Abp.Core.Authorization.Users;

namespace TtWork.ProjectName.Entities
{
    [Table("AdSwiper")]
    public class Swiper : Entity<int>, IMustHaveTenant, IMayHaveOrganizationUnit, IExtendableObject, IAudited<User>, ISoftDelete
    {
        public int GroupId { get; set; }

        [StringLength(64)] public string AppName { get; set; }

        public int SwiperType { get; set; } = 1; // 1 小程序导航

        public int Status { get; set; }

        [StringLength(64)] public string Title { get; set; }

        [StringLength(256)] public string ImagePath { get; set; }

        [StringLength(256)] public string Url { get; set; }

        public int Index { get; set; } = 0;

        #region implement

        [StringLength(512)] public string ExtensionData { get; set; }
        public int TenantId { get; set; }
        public long? OrganizationUnitId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public User CreatorUser { get; set; }
        public User LastModifierUser { get; set; }
        public bool IsDeleted { get; set; }

        #endregion
    }

    public class SwiperDto : EntityDto<int>
    {
        public int GroupId { get; set; }
        public int SwiperType { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Index { get; set; }
        public long? OrganizationUnitId { get; set; }

        public int Status { get; set; }
    }
}