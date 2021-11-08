using System;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace TtWork.ProjectName.Apis.Pays.Dtos
{
    public class RefundLogResultRequestDto : PagedResultRequestDto, IShouldNormalize
    {
        public string Sorting { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 退款金额（元）
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int? AuditStatus { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        public bool? IsSuccess { get; set; }

        public DateTime? CreationTime { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(this.Sorting))
                this.Sorting = "Id descending";
        }
    }
}