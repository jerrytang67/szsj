using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace TTWork.Abp.Activity.Domains
{
    public class LuckDrawPrize : AggregateRoot<long>, IMustHaveTenant
    {
        public LuckDrawPrize(long luckDrawId, string name, int totalCount, int stockCount)
        {
            Name = name;
            TotalCount = totalCount;
            StockCount = stockCount;
            LuckDrawId = luckDrawId;
        }

        [StringLength(256)] public string Name { get; private set; }

        [StringLength(256)] public string ImageUrl { get; set; }

        public int TotalCount { get; private set; }
        public int StockCount { get; set; }

        public EnumClass.PickupWay? PickupWay { get; set; }
        public long LuckDrawId { get; private set; }

        [ForeignKey("LuckDrawId")] public virtual LuckDraw LuckDraw { get; set; }

        public int TenantId { get; set; }
    }
}