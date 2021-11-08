using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Newtonsoft.Json;
using TTWork.Abp.Core.Authorization.Users;

namespace TTWork.Abp.Core.Domains.Weixin
{
    [Table("T_WechatUserinfos")]
    public class WechatUserinfo : Entity<string>, IMustHaveTenant, ICreationAudited<User>, IModificationAudited<User>
    {
        [JsonIgnore] [NotMapped] public override string Id => openid;

        [StringLength(32)] public string openid { get; set; }

        [StringLength(32)] public string unionid { get; set; }

        [StringLength(32)] public string nickname { get; set; }

        [StringLength(256)] public string headimgurl { get; set; }

        [StringLength(32)] public string city { get; set; }

        [StringLength(32)] public string province { get; set; }

        [StringLength(32)] public string country { get; set; }

        public int sex { get; set; }


        /// <summary>
        /// <see cref="ClientTypeEnum"/>
        /// </summary>
        public int FromClient { get; protected set; } = 1;


        [StringLength(32)] public string appid { get; protected set; }

        [StringLength(32)] public string appName { get; protected set; }

        public int TenantId { get; set; }


        public WechatUserinfo(string openid, string unionid, string nickname, string headimgurl, string city, string province, string country, int sex, int fromClient = 1, string appName = null,
            string
                appid =
                null)
        {
            this.openid = openid;
            this.unionid = unionid;
            this.nickname = nickname;
            this.headimgurl = headimgurl;
            this.city = city;
            this.province = province;
            this.country = country;
            this.sex = sex;
            FromClient = fromClient;
            this.appName = appName;
            this.appid = appid;
        }

        public void Update(string unionid, string nickname, string headimgurl, string city, string province,
            string country, int sex, int fromClient = 1, string appName = null,
            string appid =
                null)
        {
            this.unionid = unionid;
            this.nickname = nickname;
            this.headimgurl = headimgurl;
            this.city = city;
            this.province = province;
            this.country = country;
            this.sex = sex;
            FromClient = fromClient;
            this.appName = appName;
            this.appid = appid;
        }

        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public User CreatorUser { get; set; }

        [NotMapped]
        public DateTime? LastModificationTime
        {
            get => CreationTime;
            set => CreationTime = value ?? DateTime.Now;
        }

        [NotMapped]
        public long? LastModifierUserId
        {
            get => CreatorUserId;
            set => CreatorUserId = value;
        }

        [NotMapped]
        public User LastModifierUser
        {
            get => CreatorUser;
            set => CreatorUser = value;
        }
    }
}