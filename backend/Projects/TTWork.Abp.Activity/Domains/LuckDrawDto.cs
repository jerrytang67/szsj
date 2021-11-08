using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;

namespace TTWork.Abp.Activity.Domains
{
    /// <summary>
    /// <see cref="LuckDraw"/>
    /// </summary>
    public class LuckDrawDto : CreationAuditedEntityDto<long>
    {
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
        
        [NotMapped]
        public int LuckTimes { get; set; }
        
    }
}