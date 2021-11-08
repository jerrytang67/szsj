using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using TTWork.Abp.AppManagement.Apps;
using TTWork.Abp.AppManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using JetBrains.Annotations;
using TTWork.Abp.Core.Definitions;


namespace TTWork.Abp.AppManagement.Applications
{
    namespace TT.Abp.AppManagement.Application
    {
        public interface IAppAppService : IAsyncCrudAppService<AppDto, Guid, PagedAndSortedResultRequestDto, AppCreateOrUpdateDto, AppCreateOrUpdateDto>
        {
        }

        public class AppAppService : AsyncCrudAppService<App, AppDto, Guid, PagedAndSortedResultRequestDto, AppCreateOrUpdateDto, AppCreateOrUpdateDto>, IAppAppService
        {
            private readonly IAppDefinitionManager _appDefinitionManager;
            private readonly IAppProvider _appProvider;

            public AppAppService(
                IRepository<App, Guid> repository,
                IAppDefinitionManager appDefinitionManager,
                IAppProvider appProvider
            ) : base(repository)
            {
                _appDefinitionManager = appDefinitionManager;
                _appProvider = appProvider;
                base.GetAllPermissionName = AppPermissions.Pages.Administration.Default;
                base.GetPermissionName = AppPermissions.Pages.Administration.Default;
                base.CreatePermissionName = AppPermissions.Pages.Administration.Default;
                base.UpdatePermissionName = AppPermissions.Pages.Administration.Default;
                base.DeletePermissionName = AppPermissions.Pages.Administration.Default;
            }

            protected override IQueryable<App> CreateFilteredQuery(PagedAndSortedResultRequestDto input)
            {
                var query = Repository.GetAll()
                        .WhereIf(AbpSession.TenantId.HasValue, x => x.ProviderName == "T" && x.ProviderKey == AbpSession.TenantId.Value.ToString())
                        .WhereIf(!AbpSession.TenantId.HasValue, x => x.ProviderName == "T" && x.ProviderKey == null)
                    ;

                return query;
            }

            public override Task<PagedResultDto<AppDto>> GetAllAsync(PagedAndSortedResultRequestDto input)
            {
                return base.GetAllAsync(input);
            }

            public async Task<ListResultDto<AppDto>> GetPublishList()
            {
                var list = _appDefinitionManager.GetAll();

                List<AppDto> result = new List<AppDto>();
                foreach (var s in list)
                {
                    result.Add(
                        new AppDto
                        {
                            Name = s.Name,
                            ClientName = s.ClientName,
                            ClientType = s.ClientType,
                            Value = await _appProvider.GetOrNullAsync(s.Name)
                        });
                }

                return new ListResultDto<AppDto>(result); // await Task.FromResult(list);
            }
        }


        /// <summary>
        /// <see cref="App"/>
        /// </summary>
        public class AppDto : EntityDto<Guid>
        {
            public string Name { get; set; }

            public string ClientName { get; set; }

            public string ClientType { get; set; }

            public Dictionary<string, string> Value { get; set; } = new();

            public string ProviderName { get; set; }

            public string ProviderKey { get; set; }

            public string TryGetValue(string key)
            {
                return Value.ContainsKey(key) ? Value[key] : null;
            }

            public string GetValue(string key)
            {
                return Value[key] ?? throw new UserFriendlyException($"App:{Name} appid未设置");
            }
        }

        public class AppCreateOrUpdateDto : EntityDto<Guid>
        {
            [NotNull] public string Name { get; set; }

            [NotNull] public string ClientName { get; set; }

            [NotNull] public string ProviderName { get; set; }

            [CanBeNull] public string ProviderKey { get; set; }

            public Dictionary<string, string> Value { get; set; } = new();
        }
    }
}