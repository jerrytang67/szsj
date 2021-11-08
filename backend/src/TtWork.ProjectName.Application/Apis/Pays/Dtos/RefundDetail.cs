using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Domains.Weixin;
using TtWork.ProjectName.Entities.Organizations;
using TtWork.ProjectName.Entities.Pay;

namespace TtWork.ProjectName.Apis.Pays
{
    public class RefundDetail
    {
        public User User { get; set; }

        public WechatUserinfo WechatUserinfo { get; set; }

        public PayOrder PayOrder { get; set; }

        public RefundLog RefundLog { get; set; }

        public ProjectNameOrganizationUnit OrganizationUnit { get; set; }
    }
}