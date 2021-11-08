using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.Activity.Definitions;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization.Roles;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Definitions;
using TtWork.ProjectName.Authorization.Roles;

namespace TTWork.Abp.Activity.Applications
{
    public class UserPointLogAppService : AsyncCrudAppService<UserPointLog, UserPointLogDto, long, AppResultRequestDto, UserPointLogDto, UserPointLogDto>
    {
        private readonly IRepository<UserPointLog, long> _repository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;

        public UserPointLogAppService(
            IRepository<UserPointLog, long> repository,
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository,
            IRepository<UserRole, long> userRoleRepository
        ) : base(repository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            base.GetAllPermissionName = ActivityPermissions.Admin;
            base.GetPermissionName = ActivityPermissions.Admin;
        }

        public override async Task<PagedResultDto<UserPointLogDto>> GetAllAsync(AppResultRequestDto input)
        {
            CheckGetAllPermission();

            var dic = new Dictionary<long, UserDtoBase>();

            var query = CreateFilteredQuery(input);

            if (input.Status.HasValue)
            {
                var ogRole = await _roleRepository.FirstOrDefaultAsync(x => x.Name == StaticRoleNames.Tenants.Organize);

                var ogUserIds = await _userRoleRepository.GetAll().Where(x => x.RoleId == ogRole.Id).Select(x => x.UserId).ToListAsync();

                if (input.Status.Value == 1)
                {
                    query = query.Where(x => !ogUserIds.Contains(x.UserId));
                }

                else if (input.Status.Value == 2)
                {
                    query = query.Where(x => ogUserIds.Contains(x.UserId));
                }
            }

            int totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var pagedResultDto = new PagedResultDto<UserPointLogDto>(totalCount, (await AsyncQueryableExecuter.ToListAsync(query)).Select(MapToEntityDto).ToList());


            // read userDto from private cache
            foreach (var item in pagedResultDto.Items)
            {
                UserDtoBase userDto;
                if (dic.TryGetValue(item.UserId, out userDto))
                {
                    item.User = userDto;
                }
                else
                {
                    var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == item.UserId);
                    userDto = ObjectMapper.Map<UserDtoBase>(user);
                    dic.Add(item.Id, userDto);
                    item.User = userDto;
                }
            }

            return pagedResultDto;
        }

        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<ListResultDto<UserPointLog>> GetMyLogs(AppResultRequestDto input)
        {
            var query = _repository.GetAll().Where(x => x.UserId == AbpSession.UserId.Value);

            var total = await query.CountAsync();

            var list = await query.OrderByDescending(x => x.Id).PageBy(input).ToListAsync();

            return new PagedResultDto<UserPointLog>(total, ObjectMapper.Map<List<UserPointLog>>(list));
        }

        protected override IQueryable<UserPointLog> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                    .WhereIf(input.UserId.HasValue, x => x.UserId == input.UserId)
                ;
        }

        [HttpGet]
        public async Task<object> Anlayse(EntityDto<Guid> input)
        {
            var query = _repository.GetAll()
                // .Where(x => x.CouponPlanId == input.Id)
                ;

            var total = await query.Where(x => x.ChangePoints > 0).SumAsync(x => x.ChangePoints);

            return new {total};
        }


        [HttpGet]
        public async Task<object> DateAnlayse(AppResultRequestDto input)
        {
            var time = DateTime.Today.AddDays(-7);

            var query = await Repository.GetAll()
                .Where(x => x.CreationTime >= time && (x.AfterPoints - x.BeforePoints) == input.Status.Value)
                .GroupBy(row => new
                {
                    row.CreationTime.Year,
                    row.CreationTime.Month,
                    row.CreationTime.Date
                }).Select(grp => new
                {
                    Label = $"{grp.Key.Date.Month}月{grp.Key.Date.Day}日",
                    grp.Key.Year,
                    grp.Key.Month,
                    grp.Key.Date,
                    Count = grp.Count(),
                }).ToListAsync();
            return query.OrderBy(x => x.Year).ThenBy(x => x.Month).ThenBy(x => x.Date);
        }

        [HttpGet]
        public async Task<object> CountAnlayse(AppResultRequestDto input)
        {
            var query = await Repository.GetAll()
                .WhereIf(input.Status.HasValue, x => x.ChangePoints > 0)
                .WhereIf(input.From.HasValue, x => x.CreationTime >= input.From)
                .WhereIf(input.To.HasValue, x => x.CreationTime <= input.From)
                .GroupBy(row => new
                {
                    Date = row.CreationTime.Date,
                    Hour = row.CreationTime.Hour
                }).Select(grp => new
                {
                    Label = $"{grp.Key.Date.Day}日{grp.Key.Hour}时",
                    Date = grp.Key.Date,
                    Hour = grp.Key.Hour,
                    Total = grp.Sum(x => x.ChangePoints),
                }).ToListAsync();

            return query.OrderBy(x => x.Date).ThenBy(x => x.Hour);
        }

        [Obsolete]
        public override Task<UserPointLogDto> CreateAsync(UserPointLogDto input)
        {
            return null;
        }

        [Obsolete]
        public override Task<UserPointLogDto> UpdateAsync(UserPointLogDto input)
        {
            return null;
        }

        [Obsolete]
        public override Task DeleteAsync(EntityDto<long> input)
        {
            return null;
        }
    }
}