using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Events.Bus;
using Abp.Runtime.Session;
using Abp.UI;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Net.Sms;
using TtWork.ProjectName.Application.Configuration.Tenants;
using TtWork.ProjectName.Configuration;
using TtWork.ProjectName.Configuration.Tenants.Dto;

namespace TtWork.ProjectName.Apis.Configuration.Tenants
{
    public class TenantSettingsAppService : AbpAppServiceBase, ITenantSettingsAppService
    {
        private readonly ISmsSender _smsSender;
        public IEventBus EventBus { get; set; }


        public TenantSettingsAppService(ISmsSender smsSender)
        {
            _smsSender = smsSender;
            EventBus = NullEventBus.Instance;
        }

        #region Get setting

        public async Task<TenantSettingsEditDto> GetAllSettings()
        {
            var setting = new TenantSettingsEditDto
            {
                Weixin = await GetWeixinSettingsAsync(),
                Oss = await GetOssSettingsAsync(),
                Client = await GetClientSettingsAsync(),
                Notify = await GetNotifySettingsAsync()
            };
            return setting;
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
                Pay_Notify = await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.PayNotify),
                TenPay_AppId = await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.TenPay_AppId),
                TenPay_AppSecret =
                    await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.TenPay_AppSecret),
                TenPay_RefundAccount =
                    await SettingManager.GetSettingValueAsync(AppSettings.WeixinManagement.TenPay_RefundAccount),
            };
        }

        private async Task<OssSettingEditDto> GetOssSettingsAsync()
        {
            return new OssSettingEditDto
            {
                BucketName = await SettingManager.GetSettingValueAsync(AppSettings.OssManagement.BucketName),
                UserName = await SettingManager.GetSettingValueAsync(AppSettings.OssManagement.UserName),
                Password = await SettingManager.GetSettingValueAsync(AppSettings.OssManagement.Password),
                DomainHost = await SettingManager.GetSettingValueAsync(AppSettings.OssManagement.DomainHost)
            };
        }

        private async Task<ClientSettingEditDto> GetClientSettingsAsync()
        {
            return new ClientSettingEditDto
            {
                WechatMini = await SettingManager.GetSettingValueAsync<bool>(AppSettings.ClientManagement.WechatMini)
            };
        }

        private async Task<NotifySettingEditDto> GetNotifySettingsAsync()
        {
            return new NotifySettingEditDto
            {
                Phone = await SettingManager.GetSettingValueAsync(AppSettings.NotifyManagement.Phone),

                NewOrderSendStatus =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.NotifyManagement.NewOrderSendStatus),

                RefundSendStatus =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.NotifyManagement.RefundSendStatus),

                AdminOpenid =
                    await SettingManager.GetSettingValueAsync(AppSettings.NotifyManagement.AdminOpenid)
            };
        }

        #endregion

        #region Update Setting

        public async Task UpdateAllSettings(TenantSettingsEditDto input)
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new UserFriendlyException("只有租户才能设置");
            }

            await UpdateWeixinManagementSettingsAsync(input.Weixin);
            await UpdateOssManagementSettingsAsync(input.Oss);
            await UpdateClientManagementSettingsAsync(input.Client);
            await UpdateNotifyManagementSettingsAsync(input.Notify);

//            await _smsSender.SendAsync("18012728118",
//                $"UpdateAllSettings by ${(await GetCurrentUserAsync()).UserName}");


            //await EventBus.TriggerAsync(new TaskCompletedEventData { TaskId = 42, CompletorUser = await GetCurrentUserAsync() });
        }


        private async Task UpdateOssManagementSettingsAsync(OssSettingEditDto settings)
        {
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.OssManagement.BucketName,
                settings.BucketName.Trim()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.OssManagement.UserName,
                settings.UserName.Trim()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.OssManagement.Password,
                settings.Password.Trim()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.OssManagement.DomainHost,
                settings.DomainHost.Trim()
            );
        }

        private async Task UpdateWeixinManagementSettingsAsync(WeixinSettingsEditDto settings)
        {
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
                AppSettings.WeixinManagement.PayNotify,
                settings.Pay_Notify.Trim()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.WeixinManagement.TenPay_AppId,
                settings.TenPay_AppId.Trim()
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.WeixinManagement.TenPay_AppSecret,
                settings.TenPay_AppSecret.Trim()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.WeixinManagement.TenPay_RefundAccount,
                settings.TenPay_RefundAccount.Trim()
            );
        }

        private async Task UpdateClientManagementSettingsAsync(ClientSettingEditDto settings)
        {
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.ClientManagement.WechatMini,
                settings.WechatMini.ToString()
            );
        }

        private async Task UpdateNotifyManagementSettingsAsync(NotifySettingEditDto settings)
        {
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.NotifyManagement.Phone,
                settings.Phone.Trim()
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.NotifyManagement.NewOrderSendStatus,
                settings.NewOrderSendStatus.ToString()
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.NotifyManagement.RefundSendStatus,
                settings.RefundSendStatus.ToString()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.NotifyManagement.AdminOpenid,
                settings.AdminOpenid.Trim()
            );
        }

        #endregion
    }
}