using TTWork.Abp.Core.Applications.Dtos;
using TtWork.ProjectName.Apis.Organizations.Dtos;
using WechatUserinfoDto = TtWork.ProjectName.Apis.Wechat.Dtos.WechatUserinfoDto;

namespace TtWork.ProjectName.Apis.Pays.Dtos
{
    public class RefundDetailDto
    {
        public UserDtoBase User { get; set; }

        public WechatUserinfoDto WechatUserinfo { get; set; }

        public PayOrderDto PayOrder { get; set; }

        public RefundLogDto RefundLog { get; set; }

        public ProjectNameOrganizationUnitDtoBase OrganizationUnit { get; set; }
    }
}