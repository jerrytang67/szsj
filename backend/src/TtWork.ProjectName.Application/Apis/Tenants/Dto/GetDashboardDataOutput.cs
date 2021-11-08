using System.Collections.Generic;

namespace TtWork.ProjectName.Apis.Tenants.Dto
{
    public class GetDashboardDataOutput
    {
        public int NewOrders { get; set; }

        public int NewUsers { get; set; }

        public decimal? TotalSales { get; set; }

        public int CheckOutOrders { get; set; }
        
        /// <summary>
        /// 机构申请数
        /// </summary>
        public int OrganizationApplyCount { get; set; }

        /// <summary>
        /// 退款订单数
        /// </summary>
        public int RefundCount { get; set; }

        public List<object> ChatList { get; set; }
    }

    public class GetDashboardInput
    {
        public SalesSummaryDatePeriod SalesSummaryDatePeriod { get; set; } = SalesSummaryDatePeriod.Daily;
    }

    public enum SalesSummaryDatePeriod
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3
    }
}