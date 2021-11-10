using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;

namespace TTWork.Abp.Activity.Domains
{
    public interface ILuckDrawBase
    {
        public string TitleImageUrl { get; set; }
        public string Title { get; set; }
        public EnumClass.LuckDrawType Type { get; set; }
    }

    public class LuckDrawBaseDto : Entity<long>, ILuckDrawBase
    {
        public string TitleImageUrl { get; set; }
        public string Title { get; set; }
        public EnumClass.LuckDrawType Type { get; set; }
    }

    /// <summary>
    /// <see cref="LuckDraw"/>
    /// </summary>
    public class LuckDrawDto : CreationAuditedEntityDto<long>, ILuckDrawBase
    {
        public string TitleImageUrl { get; set; }
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public EnumClass.LuckDrawType Type { get; set; }
        public EnumClass.PickupWay PickupWay { get; set; }

        public int Price { get; set; }

        public int MaxDrawTimes { get; set; }

        public int PrizeTotal { get; set; }

        public int PrizeCount { get; set; }

        public int State { get; set; }

        public string HtmlContext { get; set; }

        public Dictionary<string, string> Settings { get; set; }

        public DateTime DatetimeStart { get; set; }

        public DateTime DatetimeEnd { get; set; }

        public List<LuckDrawPrizeDto> LuckDrawPrizes { get; set; }

        [NotMapped] public int LuckTimes { get; set; }
    }
}