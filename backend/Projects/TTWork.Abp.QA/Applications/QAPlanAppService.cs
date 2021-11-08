using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TT.Extensions;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.QA.Applications.Dtos;
using TTWork.Abp.QA.Definitions;
using TTWork.Abp.QA.Domains;

namespace TTWork.Abp.QA.Applications
{
    // ReSharper disable once InconsistentNaming
    public class QAPlanAppService : AbpAsyncCrudAppService<QAPlan, QAPlanDto, long, AppResultRequestDto, QAPlanCreateOrUpdateDto, QAPlanCreateOrUpdateDto>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public QAPlanAppService(
            IMediator mediator,
            IMapper mapper,
            IRepository<QAPlan, long> repository,
            IocManager iocManager) : base(repository, iocManager)
        {
            _mediator = mediator;
            _mapper = mapper;
            EnableGetEdit = true;

            base.GetAllPermissionName = QAPermissions.Default;
            base.DeletePermissionName = QAPermissions.Admin;
            base.CreatePermissionName = QAPermissions.Admin;
            base.UpdatePermissionName = QAPermissions.Admin;
        }

        public override async Task<QAPlanDto> GetAsync(EntityDto<long> input)
        {
            CheckGetPermission();
            var entity = await GetEntityByIdAsync(input.Id);
            entity.Visit();
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        [AbpAuthorize(QAPermissions.Admin)]
        public override async Task<GetForEditOutput<QAPlanCreateOrUpdateDto>> GetForEdit(EntityDto<long> input)
        {
            var result = await base.GetForEdit(input);

            result.Schema["Type"] = typeof(EnumClass.QAPlanType).GetEnumSelection();
            result.Schema["State"] = typeof(EnumClass.QAPlanState).GetEnumSelection();

            return result;
        }


        #region 用户信息

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<object> CheckUserInfo()
        {
            var user = await GetCurrentUserAsync();
            if (user.Town.IsNullOrEmptyOrWhiteSpace())
            {
                return new { success = false, message = "请完善个人信息", data = new { user.Surname, user.Town, user.PhoneNumber } };
            }

            return new { success = true };
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task PostUserInfo(QAUserRequestDto input)
        {
            var user = await GetCurrentUserAsync();

            user.Town = input.Town;
            user.Surname = input.Surname;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        #endregion
    }
}