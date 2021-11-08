using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Session;
using TtWork.ProjectName.Configuration.Tenants.Dto;
using Castle.Core.Logging;
using Newtonsoft.Json;

namespace TtWork.ProjectName.Application.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);
    }
}

