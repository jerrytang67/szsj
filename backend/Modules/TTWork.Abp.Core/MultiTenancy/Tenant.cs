using Abp.MultiTenancy;
using TTWork.Abp.Core.Authorization.Users;

namespace TTWork.Abp.Core.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
