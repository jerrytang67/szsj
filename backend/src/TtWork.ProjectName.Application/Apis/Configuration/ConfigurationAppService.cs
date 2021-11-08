using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using TTWork.Abp.Core;
using TtWork.ProjectName.Configuration;
using TtWork.ProjectName.Configuration.Dto;

namespace TtWork.ProjectName.Apis.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : AbpAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
