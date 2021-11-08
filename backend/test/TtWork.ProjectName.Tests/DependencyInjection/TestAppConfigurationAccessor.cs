using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using TT.Extensions.Redis;
using TT.HttpClient.Weixin;
using TTWork.Abp.Core.Applications.Wechat;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Domains.Weixin;
using TTWork.Abp.Oss.UpYun;
using TtWork.ProjectName.Configuration;

namespace TtWork.ProjectName.Tests.DependencyInjection
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(AbpTestModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }

    public class TestWeixinManger : WeixinManger
    {
        public TestWeixinManger(IRepository<WechatUserinfo, string> wxuseRepository, IIocManager iocManager, IRepository<UserLogin, long> userLoginRepository, IRepository<User, long> userRepository,
            IWeixinApi weixinApi, UpYunClient ossClient, IRedisClient redisClient) : base(wxuseRepository, iocManager, userLoginRepository, userRepository, weixinApi, ossClient, redisClient)
        {
        }

        
        public override async Task<string> GetAccessTokenAsync(string appid, string appsec)
        {
            return await Task.FromResult("00000000");
            // return base.GetAccessTokenAsync(tenantId, isMini);
        }
    }
}