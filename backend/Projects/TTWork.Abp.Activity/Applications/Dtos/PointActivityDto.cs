using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TTWork.Abp.Activity.Domains;

namespace TTWork.Abp.Activity.Applications
{
    /// <summary>
    /// <see cref="PointActivity"/>
    /// </summary>
    public class PointActivityDto : EntityDto<long>
    {
        public string TitleImageUrl { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public int State { get; set; }

        public string HtmlContext { get; set; }

        public int ViewCount { get; init; }

        // 一天可以帮助几次
        public int HelpPerDay { get; set; }

        //页面显示相关字段
        public Dictionary<string, string> Settings { get; set; }

        public DateTime DatetimeStart { get; set; }

        public DateTime DatetimeEnd { get; set; }

        public string TimeStart { get; set; }

        public string TimeEnd { get; set; }

        public int MinPoint { get; set; }

        public int MaxPoint { get; set; }

        public bool IsEnd { get; set; }
    }
}