using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT.Extensions;
using TTWork.Abp.Activity.Definitions;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Events.Queries;

namespace TTWork.Abp.Activity.Applications
{
    public class LuckDrawPrizeAppService : AbpAsyncCrudAppService<LuckDrawPrize, LuckDrawPrizeDto, long, AppResultRequestDto, LuckDrawPrizeCreateOrUpdateDto, LuckDrawPrizeCreateOrUpdateDto>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<LuckDraw, long> _luckDrawRepository;
        private readonly IRepository<UserPrize, long> _userPrizeRepository;

        public LuckDrawPrizeAppService(
            IMediator mediator,
            IRepository<LuckDrawPrize, long> repository,
            IRepository<LuckDraw, long> luckDrawRepository,
            IRepository<UserPrize, long> userPrizeRepository,
            IocManager iocManager) :
            base(repository, iocManager)
        {
            _mediator = mediator;
            _luckDrawRepository = luckDrawRepository;
            _userPrizeRepository = userPrizeRepository;
            EnableGetEdit = true;
            base.GetAllPermissionName = ActivityPermissions.Default;
            base.CreatePermissionName = ActivityPermissions.Admin;
            base.UpdatePermissionName = ActivityPermissions.Admin;
            base.DeletePermissionName = ActivityPermissions.Admin;
        }

        public override async Task<PagedResultDto<LuckDrawPrizeDto>> GetAllAsync(AppResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            if (await IsAdminAsync("UnionAdmin"))
            {
                var cache = new Dictionary<long, string>();

                foreach (var dto in result.Items)
                {
                    var checkedCount = await _userPrizeRepository.GetAll().AsNoTracking()
                        .CountAsync(x => x.PrizeId == dto.Id && x.State == EnumClass.UserPrizeState.已领取);
                    dto.CheckedCount = checkedCount;

                    if (!cache.ContainsKey(dto.LuckDrawId))
                    {
                        var luckDraw = await _luckDrawRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == dto.LuckDrawId);
                        if (luckDraw != null)
                        {
                            cache.Add(dto.LuckDrawId, luckDraw.Title);
                        }
                    }

                    dto.LuckDrawTitle = cache[dto.LuckDrawId];
                }
            }

            return result;
        }

        protected override IQueryable<LuckDrawPrize> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                    .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), x => x.Name.Contains(input.Keyword))
                ;
        }

        public override async Task<GetForEditOutput<LuckDrawPrizeCreateOrUpdateDto>> GetForEdit(EntityDto<long> input)
        {
            var result = await base.GetForEdit(input);

            var drawList = await _luckDrawRepository.GetAll().AsNoTracking().ToListAsync();

            result.Schema["luckDrawId"] = drawList.GetSelection("number", "luckDrawId", @"{0}", new[] { "Title" }, "Id");
            result.Schema["PickupWay"] = typeof(EnumClass.PickupWay).GetEnumSelection();

            return result;
        }
    }

    public class LuckDrawPrizeMessageInput
    {
        public long PrizeId { get; set; }

        public string Text { get; set; }
    }
}