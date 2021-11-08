using TTWork.Abp.Core.Applications.Dtos;

namespace TtWork.ProjectName.Apis.Pays.Dtos
{
    public class PayOrderRequestDto : AppResultRequestDto
    {
        public virtual string BillNo { get; set; }
    }
}