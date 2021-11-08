using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using TtWork.ProjectName.Apis.Dtos;
using TtWork.ProjectName.Events.Queries;
using MediatR;
using Newtonsoft.Json.Linq;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;

namespace TtWork.ProjectName.Apis.Poster
{
    public interface IPosterAppService : IAsyncCrudAppService<PosterDto, int, AppResultRequestDto, PosterDto, PosterDto>
    {
        Task<string> GetPosterImage(EntityDto input);
    }

    public class PosterAppService :
        AbpAsyncCrudAppService<Entities.Posters.Poster, PosterDto, int, AppResultRequestDto, PosterDto, PosterDto>,
        IPosterAppService
    {
        private readonly IMediator _mediator;

        public PosterAppService(
            IMediator mediator,
            IRepository<Entities.Posters.Poster, int> repository,
            IocManager iocManager) : base(repository, iocManager)
        {
            _mediator = mediator;
            base.EnableGetEdit = true;
        }

        public override async Task<GetForEditOutput<PosterDto>> GetForEdit(EntityDto<int> input)
        {
            var find = await Repository.FirstOrDefaultAsync(z => z.Id == input.Id);

            var schema = JToken.FromObject(new { });

            return new GetForEditOutput<PosterDto>(ObjectMapper.Map<PosterDto>(find) ?? new PosterDto(), schema);
        }

        [UnitOfWork]
        public async Task<string> GetPosterImage(EntityDto input)
        {
            var url = await _mediator.Send(new GetPosterPreviewImageUrl(input.Id));

            return $"{url}";
        }
    }
}