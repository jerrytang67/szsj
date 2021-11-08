using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Definitions;
using TtWork.ProjectName.Apis.Cms.Dtos;
using TtWork.ProjectName.Entities.Cms;

namespace TtWork.ProjectName.Apis.Cms
{
    public interface ICmsCategoryAppService : IAsyncCrudAppService<CmsCategoryDto, int, AppResultRequestDto,
        CmsCategoryCreateOrUpdateDto, CmsCategoryCreateOrUpdateDto>
    {
    }


    /// <summary>
    /// 文章/栏目 分类
    /// </summary>
    public class CmsCategoryAppService :
        AbpAsyncCrudAppService<CmsCategory, CmsCategoryDto, int, AppResultRequestDto, CmsCategoryCreateOrUpdateDto,
            CmsCategoryCreateOrUpdateDto>, ICmsCategoryAppService
    {
        private readonly IRepository<CmsContent, int> _cmsContentRepository;

        public CmsCategoryAppService(IRepository<CmsCategory, int> repository, IocManager iocManager,
            IRepository<CmsContent, int> cmsContentRepository) : base(repository,
            iocManager)
        {
            _cmsContentRepository = cmsContentRepository;
            base.UpdatePermissionName = AppPermissions.Pages.Administration.Default;
            base.DeletePermissionName = AppPermissions.Pages.Administration.Default;
            base.CreatePermissionName = AppPermissions.Pages.Administration.Default;

            base.EnableGetEdit = true;
        }


        public override async Task<PagedResultDto<CmsCategoryDto>> GetAllAsync(AppResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);
            return result;
        }

        public override async Task<GetForEditOutput<CmsCategoryCreateOrUpdateDto>> GetForEdit(EntityDto<int> input)
        {
            CmsCategory find = null;
            if (input.Id > 0)
                find = await Repository.FirstOrDefaultAsync(z => z.Id == input.Id);
            var schema = JToken.FromObject(new { });

            return new GetForEditOutput<CmsCategoryCreateOrUpdateDto>(
                find != null
                    ? ObjectMapper.Map<CmsCategoryCreateOrUpdateDto>(find)
                    : new CmsCategoryCreateOrUpdateDto(),
                schema);
        }
    }
}