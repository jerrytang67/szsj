using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using AutoMapper;
using MediatR;
using TTWork.Abp.Activity.Definitions;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Extensions;

namespace TTWork.Abp.Activity.Applications
{
    public class VotePlanAppService : AbpAsyncCrudAppService<VotePlan, VotePlanDto, long, AppResultRequestDto, VotePlanCreateOrUpdateDto, VotePlanCreateOrUpdateDto>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public VotePlanAppService(
            IMediator mediator,
            IMapper mapper,
            IRepository<VotePlan, long> repository,
            IocManager iocManager) : base(repository, iocManager)
        {
            _mediator = mediator;
            _mapper = mapper;
            EnableGetEdit = true;

            base.GetAllPermissionName = ActivityPermissions.Default;
            base.DeletePermissionName = ActivityPermissions.Admin;
            base.CreatePermissionName = ActivityPermissions.Admin;
            base.UpdatePermissionName = ActivityPermissions.Admin;
        }

        public override async Task<VotePlanDto> GetAsync(EntityDto<long> input)
        {
            CheckGetPermission();
            var entity = await GetEntityByIdAsync(input.Id);
            entity.Visit();
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        [AbpAuthorize(ActivityPermissions.Admin)]
        public override async Task<GetForEditOutput<VotePlanCreateOrUpdateDto>> GetForEdit(EntityDto<long> input)
        {
            var result = await base.GetForEdit(input);
            result.Schema["State"] = typeof(EnumClass.VotePlanState).GetEnumSelection();
            return result;
        }
        
        
    }
}