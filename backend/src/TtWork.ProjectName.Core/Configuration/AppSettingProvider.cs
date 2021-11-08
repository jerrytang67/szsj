using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;
using Microsoft.Extensions.Configuration;

namespace TtWork.ProjectName.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        private readonly IConfigurationRoot _appConfiguration;

        public AppSettingProvider(IAppConfigurationAccessor appConfiguration)
        {
            _appConfiguration = appConfiguration.Configuration;
        }

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return GetHostSettings().Union(GetTenantSettings())
                .Union(GetClientSettings())
                .Union(GetNotifySettings());
        }

        private IEnumerable<SettingDefinition> GetHostSettings()
        {
            return new[]
            {
                new SettingDefinition(AppSettings.UiTheme, "red",
                    scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User,
                    isVisibleToClients: true)
            };
        }

        private IEnumerable<SettingDefinition> GetTenantSettings()
        {
            return new[]
            {
                // weixin
                new SettingDefinition(AppSettings.WeixinManagement.PayMchId, GetFromAppSettings(AppSettings.WeixinManagement.PayMchId, ""), scopes: SettingScopes.Tenant, isVisibleToClients: true),

                new SettingDefinition(AppSettings.WeixinManagement.PayKey, GetFromAppSettings(AppSettings.WeixinManagement.PayKey, ""), scopes: SettingScopes.Tenant),

                new SettingDefinition(AppSettings.WeixinManagement.PayNotify, GetFromAppSettings(AppSettings.WeixinManagement.PayNotify, ""), scopes: SettingScopes.Tenant, isVisibleToClients: true),

                new SettingDefinition(AppSettings.WeixinManagement.TenPay_AppId, GetFromAppSettings(AppSettings.WeixinManagement.TenPay_AppId, ""), scopes: SettingScopes.Tenant),

                new SettingDefinition(AppSettings.WeixinManagement.TenPay_AppSecret, GetFromAppSettings(AppSettings.WeixinManagement.TenPay_AppSecret, ""), scopes: SettingScopes.Tenant,
                    isVisibleToClients: true),

                new SettingDefinition(AppSettings.WeixinManagement.TenPay_RefundAccount, GetFromAppSettings(AppSettings.WeixinManagement.TenPay_RefundAccount, "REFUND_SOURCE_RECHARGE_FUNDS"),
                    scopes: SettingScopes.Tenant, isVisibleToClients: true),


                //oss 
                new SettingDefinition(AppSettings.OssManagement.BucketName, GetFromAppSettings(AppSettings.OssManagement.BucketName, ""), scopes: SettingScopes.Tenant, isVisibleToClients: true),

                new SettingDefinition(AppSettings.OssManagement.UserName, GetFromAppSettings(AppSettings.OssManagement.UserName, ""), scopes: SettingScopes.Tenant, isVisibleToClients: true),

                new SettingDefinition(AppSettings.OssManagement.Password, GetFromAppSettings(AppSettings.OssManagement.Password, ""), scopes: SettingScopes.Tenant, isVisibleToClients: true),

                new SettingDefinition(AppSettings.OssManagement.DomainHost, GetFromAppSettings(AppSettings.OssManagement.DomainHost, ""), scopes: SettingScopes.Tenant, isVisibleToClients: true),
            };
        }


        private IEnumerable<SettingDefinition> GetClientSettings()
        {
            return new[]
            {
                new SettingDefinition(AppSettings.ClientManagement.WechatMini,
                    GetFromAppSettings(AppSettings.ClientManagement.WechatMini, "true"), scopes: SettingScopes.Tenant,
                    isVisibleToClients: true)
            };
        }

        private IEnumerable<SettingDefinition> GetNotifySettings()
        {
            return new[]
            {
                new SettingDefinition(AppSettings.NotifyManagement.Phone,
                    GetFromAppSettings(AppSettings.NotifyManagement.Phone, ""), scopes: SettingScopes.Tenant,
                    isVisibleToClients: true),

                new SettingDefinition(AppSettings.NotifyManagement.NewOrderSendStatus,
                    GetFromAppSettings(AppSettings.NotifyManagement.NewOrderSendStatus, "false"), scopes: SettingScopes.Tenant,
                    isVisibleToClients: true),

                new SettingDefinition(AppSettings.NotifyManagement.RefundSendStatus,
                    GetFromAppSettings(AppSettings.NotifyManagement.RefundSendStatus, "false"), scopes: SettingScopes.Tenant,
                    isVisibleToClients: true),

                new SettingDefinition(AppSettings.NotifyManagement.AdminOpenid,
                    GetFromAppSettings(AppSettings.NotifyManagement.AdminOpenid, ""), scopes: SettingScopes.Tenant,
                    isVisibleToClients: true),
            };
        }

        private string GetFromAppSettings(string name, string defaultValue = null)
        {
            return GetFromSettings("App:" + name, defaultValue);
        }

        private string GetFromSettings(string name, string defaultValue = null)
        {
            return _appConfiguration[name] ?? defaultValue;
        }
    }
}