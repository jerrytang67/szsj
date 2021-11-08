using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using TTWork.Abp.Activity.Domains;

namespace TTWork.Abp.Activity.Applications
{
    /// <summary>
    /// <see cref="PointActivity"/>
    /// </summary>
    public class PointActivityCreateOrUpdateDto : EntityDto<long>
    {
        [StringLength(256)] public string TitleImageUrl { get; set; }

        [StringLength(64)] public string Title { get; set; }

        [StringLength(512)] public string SubTitle { get; set; }

        public int State { get; set; }

        public string HtmlContext { get; set; }

        public int ViewCount { get; init; }

        // 一天可以帮助几次
        public int HelpPerDay { get; set; }

        //页面显示相关字段
        public Dictionary<string, string> Settings { get; set; } = new();

        public DateTime DatetimeStart { get; set; }

        public DateTime DatetimeEnd { get; set; }

        [StringLength(16)] [CanBeNull] public string TimeStart { get; set; }

        [StringLength(16)] [CanBeNull] public string TimeEnd { get; set; }

        public int MinPoint { get; set; }

        public int MaxPoint { get; set; }
    }
}