using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TtWork.ProjectName.Entities.Cms;

namespace TtWork.ProjectName.Apis.Cms.Dtos
{
    [AutoMapFrom(typeof(CmsContent))]
    public class CmsContentDto : EntityDto<int>
    {
        public int CategoryId { get; set; }

        public string Title { get; set; }

        public string TitleImageUrl { get; set; }

        public CmsContentLinkType LinkType { get; set; } = 0;

        public string LinkUrl { get; set; }
        
        public int Sort { get; set; }

        public string Content { get; set; }

        public CmsCategoryDto Category { get; set; }

        public EnumClass.CmsContentStatus Status { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        public int ViewCount { get; set; }


        /// <summary>
        /// 存放用户关注等事件
        /// </summary>
        public List<EventTypeEnum> UserEvents { get; set; } = new List<EventTypeEnum>();
    }

    public enum CmsContentLinkType
    {
        不跳转 = 0,
        跳转到小程序内容 = 1,
        跳转到小程序路径 = 2,
        公众号图文消息 = 11,
        H5 = 21
    }
}