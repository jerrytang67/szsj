using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TtWork.ProjectName.Apis.MultiTenancy.Dto;
using TtWork.ProjectName.MultiTenancy.Dto;

namespace TtWork.ProjectName.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

