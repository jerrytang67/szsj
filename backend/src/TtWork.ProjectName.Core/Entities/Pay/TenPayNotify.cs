using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Castle.Components.DictionaryAdapter;

namespace TtWork.ProjectName.Entities.Pay
{
    [Table("T_TenPayNotify")]
    public class TenPayNotify : Entity<int>
    {
        [StringLength(40)]
        public string out_trade_no { get; set; }

        [StringLength(40)]
        public string result_code { get; set; }

        [StringLength(40)]
        public string fee_type { get; set; }

        [StringLength(40)]
        public string return_code { get; set; }

        public string total_fee { get; set; }
        [StringLength(40)]
        public string mch_id { get; set; }

        public string cash_fee { get; set; }

        [StringLength(40)]
        public string openid { get; set; }
        [StringLength(40)]
        public string transaction_id { get; set; }
        [StringLength(40)]
        public string sign { get; set; }
        [StringLength(40)]
        public string bank_type { get; set; }
        [StringLength(40)]
        public string appid { get; set; }
        [StringLength(40)]
        public string time_end { get; set; }
        [StringLength(40)]
        public string trade_type { get; set; }
        [StringLength(40)]
        public string nonce_str { get; set; }
        [StringLength(40)]
        public string is_subscribe { get; set; }
    }

}