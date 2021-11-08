using Abp.Application.Services.Dto;

namespace TTWork.Abp.Activity.Domains
{
    /// <summary>
    /// <see cref="LuckDrawPrize"/>
    /// </summary>
    public class LuckDrawPrizeDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public int TotalCount { get; set; }

        public int StockCount { get; set; }
        
        public EnumClass.PickupWay? PickupWay { get; set; }
        
        public int CheckedCount { get; set; }
        public long LuckDrawId { get; set; }
        
        public string LuckDrawTitle { get; set; }
    }
}