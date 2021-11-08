using Abp.Application.Services.Dto;
using FluentValidation;

namespace TTWork.Abp.Activity.Domains
{
    public class LuckDrawPrizeCreateOrUpdateDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int TotalCount { get; set; }
        public int StockCount { get; set; }
        
        public EnumClass.PickupWay? PickupWay { get; set; }
        public long? LuckDrawId { get; set; }
    }

    public class LuckDrawPrizeCreateOrUpdateDtoValidator : AbstractValidator<LuckDrawPrizeCreateOrUpdateDto>
    {
        public LuckDrawPrizeCreateOrUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入奖品名称");
            RuleFor(x => x.LuckDrawId).NotNull().WithMessage("请选择活动");
            RuleFor(x => x.TotalCount).GreaterThan(0).WithMessage("请输入奖品数量");
        }
    }
}