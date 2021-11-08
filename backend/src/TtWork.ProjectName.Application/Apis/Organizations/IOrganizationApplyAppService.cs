using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using TTWork.Abp.Core.Applications.Dtos;
using TtWork.ProjectName.Apis.Dtos;
using TtWork.ProjectName.Apis.Organizations.Dtos;

namespace TtWork.ProjectName.Apis.Organizations
{
    public interface IOrganizationApplyAppService : IAsyncCrudAppService<OrganizationApplyDto, long, AppResultRequestDto, CreatorOrUpdateOrganizationApplyDto,
        CreatorOrUpdateOrganizationApplyDto>
    {
        Task<List<OrganizationApplyDto>> GetMyApplyList();
    }
}