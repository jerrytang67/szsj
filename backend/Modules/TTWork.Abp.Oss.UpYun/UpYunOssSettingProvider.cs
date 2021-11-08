using System.Collections.Generic;
using Abp.Configuration;
using Microsoft.Extensions.Configuration;
using TtWork.ProjectName.Configuration;

namespace TTWork.Abp.Oss.UpYun
{
    public class UpYunOssSettingProvider : SettingProvider
    {
        private readonly IConfigurationRoot _appConfiguration;

        public UpYunOssSettingProvider(IAppConfigurationAccessor appConfiguration)
        {
            _appConfiguration = appConfiguration.Configuration;
        }

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(OssSetting.Upyun.BucketName,
                    GetFromAppSettings(OssSetting.Upyun.BucketName, ""),
                    scopes: SettingScopes.All, isVisibleToClients: true),

                new SettingDefinition(OssSetting.Upyun.UserName,
                    GetFromAppSettings(OssSetting.Upyun.UserName, ""),
                    scopes: SettingScopes.All,
                    isVisibleToClients: true),

                new SettingDefinition(OssSetting.Upyun.Password,
                    GetFromAppSettings(OssSetting.Upyun.Password, ""),
                    scopes: SettingScopes.All, isVisibleToClients: false
                ),

                new SettingDefinition(OssSetting.Upyun.DomainHost,
                    GetFromAppSettings(OssSetting.Upyun.DomainHost, ""),
                    scopes: SettingScopes.All, isVisibleToClients: true),
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