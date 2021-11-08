using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;

namespace TTWork.Abp.Activity.Domains
{
    public class UserPrize : CreationAuditedAggregateRoot<long>, IMustHaveTenant
    {
        public long PrizeId { get; set; }

        public long LuckDrawId { get; set; }

        public EnumClass.PickupWay PickupWay { get; set; }

        [StringLength(256)] public string Name { get; set; }

        [StringLength(256)] public string ImageUrl { get; set; }

        public int Count { get; set; }

        [StringLength(32)] public string PhoneNumber { get; set; }

        /// <summary>
        /// <see cref="EnumClass.UserPrizeState"/>
        /// </summary>
        public EnumClass.UserPrizeState State { get; set; }

        [StringLength(256)] public string QrUrl { get; set; }

        public int TenantId { get; set; }

        public DateTime? CheckTime { get; set; }

        [StringLength(64)] public string CheckCode { get; set; }

        public long? CheckUserId { get; set; }

        [StringLength(64)] public string CheckPhoneNumber { get; set; }

        public DateTime? ExpiredTime { get; set; }

        public AddressDetail Address { get; set; }

        public UserPrize GetTestEntity(bool isChecked = false)
        {
            Id = -1;
            LuckDrawId = -1;
            PhoneNumber = "18000000000";
            State = 0;
            Count = 1;
            ImageUrl = "https://img.wujiangapp.com/wjzgh/2021-05-06/upload_mimt2k3dvu27o54vvaxeuw98dpf5pr4v.jpg";
            Name = "测试用取货码,核销后10秒将还原";
            ExpiredTime = DateTime.Today.AddDays(1);
            if (isChecked)
            {
                CheckTime = DateTime.Now;
                CheckPhoneNumber = "19999999999";
                State = EnumClass.UserPrizeState.已领取;
            }

            return this;
        }
    }

    [Owned]
    public class AddressDetail
    {
        [StringLength(64)] public string UserName { get; set; } //收货人姓名
        [StringLength(64)] public string PostalCode { get; set; } //邮编
        [StringLength(64)] public string ProvinceName { get; set; } //	收货人地址第一级地址
        [StringLength(64)] public string CityName { get; set; } //	国标收货地址第二级地址
        [StringLength(64)] public string CountyName { get; set; } //	国标收货地址第三级地址
        [StringLength(64)] public string DetailInfo { get; set; } //	详细收货地址信息
        [StringLength(64)] public string NationalCode { get; set; } //	收货地址国家码
        [StringLength(64)] public string TelNumber { get; set; } //	收货人手机号码
    }
}