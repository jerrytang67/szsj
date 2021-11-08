using Abp.Authorization.Users;
using Abp.Events.Bus;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.MultiTenancy;

namespace TtWork.ProjectName.Events
{
    public class AccountLoginEvent : EventData
    {
        public AbpLoginResult<Tenant, User> LoginResult;

        public AccountLoginEvent(AbpLoginResult<Tenant, User> loginResult)
        {
            LoginResult = loginResult;
        }
    }
}