using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TTWork.Abp.Activity
{
    public class EnumClass
    {
        public enum UserPointEventType
        {
            Normal = 0,
            Mall = 1,
            Activity = 2,
            Register = 11,
            PointQr = 101,
            QA = 1001,
        }


        public enum UserPrizeState
        {
            过期 = -1,
            待领取 = 0,
            已领取 = 1
        }

        public enum PointActivityUserState
        {
            未完成 = 0,
            已发放 = 1
        }

        public enum PickupWay
        {
            Qr = 0, //上门核销
            Express = 1 //快递
        }


        public enum LuckDrawType
        {
            Default = 0,
            Points = 1,
            UserLuckyTimes = 2
        }

        public enum VotePlanState
        {
            关闭 = 0,
            开启 = 1
        }


        public enum UserLuckTimeStatus
        {
            未使用 = 0,
            已使用 = 1
        }


        public enum ListState
        {
            [Display(Name = "不通过")] [EnumMember(Value = "不通过")]
            UnPass = -1,

            [Display(Name = "草稿")] [EnumMember(Value = "草稿")]
            Saved = 0,

            [Display(Name = "退回")] [EnumMember(Value = "退回")]
            SendBacked = 1,

            [Display(Name = "已提交/待审核")] [EnumMember(Value = "已提交/待审核")]
            Submitted = 5,

            [Display(Name = "审核通过")] [EnumMember(Value = "审核通过")]
            Approved = 6
        }
    }
}