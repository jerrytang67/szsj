using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using FluentValidation;

namespace TTWork.Abp.Activity.Domains
{
    /// <summary>
    /// <see cref="LuckDraw"/>
    /// </summary>
    public class LuckDrawCreateOrUpdateDto : EntityDto<long>
    {
        public string TitleImageUrl { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public EnumClass.LuckDrawType Type { get; set; } = EnumClass.LuckDrawType.Points;
        public EnumClass.PickupWay PickupWay { get; set; } = EnumClass.PickupWay.Qr;
        public int MaxDrawTimes { get; set; } = -1;

        public int? MaxWinTimes { get; set; }

        public int Price { get; set; }

        public int State { get; set; }

        public int PrizeTotal { get; set; }

        public double? DefaultWinningChance { get; set; } = 0.2;

        public string HtmlContext { get; set; }

        public Dictionary<string, string> Settings { get; set; } = new();

        public DateTime? DatetimeStart { get; set; } = DateTime.Today.AddDays(1);

        public DateTime? DatetimeEnd { get; set; } = DateTime.Today.AddDays(2);

        public DateTime? PrizeExpiredTime { get; set; } = DateTime.Today.AddDays(1).AddMonths(1);

        public List<string> CheckCodes { get; set; }
    }

    public class LuckDrawCreateOrUpdateDtoValidater : AbstractValidator<LuckDrawCreateOrUpdateDto>
    {
        public LuckDrawCreateOrUpdateDtoValidater()
        {
            RuleFor(x => x.DefaultWinningChance)
                .NotNull().WithMessage("中奖机率 必填")
                .GreaterThan(0).WithMessage("中奖机率 必须大于0");
        }
    }
}