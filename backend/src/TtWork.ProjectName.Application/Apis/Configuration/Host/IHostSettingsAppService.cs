using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Runtime.Session;
using Abp.UI;
using TTWork.Abp.Core;
using TtWork.ProjectName.Configuration;
using TtWork.ProjectName.Configuration.Tenants.Dto;

namespace TtWork.ProjectName.Apis.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);
    }

    public class HostSettingsEditDto
    {
    }

    public class HostSettingsAppService : AbpAppServiceBase, IHostSettingsAppService
    {
        #region Get setting

        public async Task<HostSettingsEditDto> GetAllSettings()
        {
            var setting = new HostSettingsEditDto { };
            return await Task.FromResult(setting);
        }

        private async Task<WeixinSettingsEditDto> GetWeixinSettingsAsync()
        {
            return new WeixinSettingsEditDto
            {
                AppId = await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.AppId),
                AppSecret = await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.AppSecret),
                Mini_AppId = await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.MiniAppId),
                Mini_AppSecret = await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.MiniAppSecret),
                Pay_MchId = await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.PayMchId),
                Pay_Key = await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.PayKey),
                TenPay_AppId = await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.TenPay_AppId),
                TenPay_AppSecret =
                    await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.TenPay_AppSecret),
                TenPay_RefundAccount =
                    await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.TenPay_RefundAccount),
            };
        }

        #endregion

        #region Update Setting

        public async Task UpdateAllSettings(HostSettingsEditDto input)
        {
            await Task.CompletedTask;
            //await UpdateWeixinManagementSettingsAsync(input.Weixin);
        }

        private async Task UpdateWeixinManagementSettingsAsync(WeixinSettingsEditDto settings)
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new UserFriendlyException("只有租户才能设置");
            }

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.WeixinManagement.AppId,
                settings.AppId.Trim()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.WeixinManagement.AppSecret,
                settings.AppSecret.Trim()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.WeixinManagement.MiniAppId,
                settings.Mini_AppId.Trim()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.WeixinManagement.MiniAppSecret,
                settings.Mini_AppSecret.Trim()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.WeixinManagement.PayMchId,
                settings.Pay_MchId.Trim()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.WeixinManagement.PayKey,
                settings.Pay_Key.Trim()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.WeixinManagement.TenPay_RefundAccount,
                settings.TenPay_RefundAccount.Trim()
            );
        }

        #endregion
    }
}