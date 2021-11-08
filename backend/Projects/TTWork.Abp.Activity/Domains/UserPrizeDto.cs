using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace TTWork.Abp.Activity.Domains
{
    public class UserPrizeDto : CreationAuditedEntityDto<long>
    {
        public long PrizeId { get; set; }

        public long LuckDrawId { get; set; }
        
        public EnumClass.PickupWay PickupWay { get; set; }

        public string LuckDrawTitle { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int Count { get; set; }

        public string PhoneNumber { get; set; }

        public int State { get; set; }

        public string QrUrl { get; set; }

        public LuckDrawDto LuckDraw { get; set; }

        public DateTime? CheckTime { get; set; }

        public string CheckCode { get; set; }

        public long? CheckUserId { get; set; }

        public string CheckPhoneNumber { get; set; }

        public DateTime? ExpiredTime { get; set; }
        
        public AddressDetail Address { get; set; }
    }
}