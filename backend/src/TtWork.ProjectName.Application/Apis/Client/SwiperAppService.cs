using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using TtWork.ProjectName.Apis.Dtos;
using TtWork.ProjectName.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Definitions;

namespace TtWork.ProjectName.Apis.Client
{
    public interface ISwiperAppService : IAsyncCrudAppService<SwiperDto, int, AppResultRequestDto, SwiperDto, SwiperDto>
    {
        Task<List<SwiperDto>> GetByGroupId(EntityDto input);
        Task<GetForEditOutput<SwiperDto>> GetForEdit(EntityDto input);
    }

    [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
    public class SwiperAppService : AsyncCrudAppService<Swiper, SwiperDto, int, AppResultRequestDto, SwiperDto, SwiperDto>, ISwiperAppService
    {
        private readonly IRepository<Swiper> _swiperRepository;

        public SwiperAppService(IRepository<Swiper> swiperRepository) : base(swiperRepository)
        {
            _swiperRepository = swiperRepository;
        }

        [AbpAllowAnonymous]
        public async Task<List<SwiperDto>> GetByGroupId(EntityDto input)
        {
            var list = await _swiperRepository
                .GetAll()
                .Where(z => z.GroupId == input.Id
                            && z.Status == 1
                ).ToListAsync();

            return ObjectMapper.Map<List<SwiperDto>>(list);
        }

        public async Task<GetForEditOutput<SwiperDto>> GetForEdit(EntityDto input)
        {
            Swiper find = null;
            if (input.Id > 0)
                find = await Repository.FirstOrDefaultAsync(z => z.Id == input.Id);

            var schema = JToken.FromObject(new { });

            return new GetForEditOutput<SwiperDto>(
                find != null ? ObjectMapper.Map<SwiperDto>(find) : new SwiperDto(),
                schema);
        }


        public override async Task<PagedResultDto<SwiperDto>> GetAllAsync(AppResultRequestDto input)
        {
            var query = _swiperRepository
                    .GetAll()
                    .WhereIf(input.Pid.HasValue, x => x.GroupId == input.Pid)
                ;


            var count = await query.CountAsync();

            var items = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<SwiperDto>(count, ObjectMapper.Map<List<SwiperDto>>(items));
        }
    }
}