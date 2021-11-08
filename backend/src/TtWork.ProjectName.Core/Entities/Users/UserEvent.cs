using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using TTWork.Abp.Core.Authorization.Users;

namespace TtWork.ProjectName.Entities.Users
{
    [Table("T_UserEvents")]
    public class UserEvent : Entity<long>, ICreationAudited<User>, IExtendableObject, IMayHaveTenant, IMayHaveFromUser
    {
        public Dictionary<string, string> Value { get; set; }

        /// <summary>
        ///     1 扫码访问
        /// </summary>
        public int EventType { get; set; } = 0;

        [ForeignKey("FromUserId")] public User FromUser { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public User CreatorUser { get; set; }

        [StringLength(512)] public string ExtensionData { get; set; }

        public long? FromUserId { get; set; }

        public int? TenantId { get; set; }
    }
}