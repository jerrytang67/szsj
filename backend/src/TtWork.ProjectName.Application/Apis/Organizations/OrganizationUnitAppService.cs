using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.Runtime.Session;
using Abp.UI;
using TtWork.ProjectName.Apis.Dtos;
using TtWork.ProjectName.Apis.Organizations.Dtos;
using TtWork.ProjectName.Authorization.Roles;
using TtWork.ProjectName.Entities.Organizations;
using TtWork.ProjectName.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TT.Extensions;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.Core.Net.Sms;
using TTWork.Abp.Core.Organizations;

namespace TtWork.ProjectName.Apis.Organizations
{
    public class OrganizationUnitAppService : AbpAppServiceBase, IOrganizationUnitAppService
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<ProjectNameOrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationEvent, long> _organizationEventRepository;
        private readonly ICurrentOrganization _currentOrganization;
        private readonly ISmsSender _smsSender;


        public OrganizationUnitAppService(
            OrganizationUnitManager organizationUnitManager,
            IRepository<ProjectNameOrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationEvent, long> organizationEventRepository,
            ICurrentOrganization currentOrganization,
            ISmsSender smsSender)
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationEventRepository = organizationEventRepository;
            _currentOrganization = currentOrganization;
            _smsSender = smsSender;
        }

        [AbpAllowAnonymous]
        public async Task<OrganizationUnitDto> GetOrganizationUnit(EntityDto<int> input)
        {
            var query = await _organizationUnitRepository.GetAll().FirstOrDefaultAsync(z => z.Id == input.Id);

            var resultDto = ObjectMapper.Map<OrganizationUnitDto>(query);
            try
            {
                if (AbpSession.GetUserId() > 0)
                {
                    var collectionEvent = await (from z in _organizationEventRepository.GetAll().Where(z =>
                            z.OrganizationUnitId == input.Id && z.CreatorUserId == AbpSession.GetUserId())
                        select z.EventType).ToListAsync();

                    resultDto.UserEvents = collectionEvent.Distinct().ToList();
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return resultDto;
        }

        [AbpAllowAnonymous]
        public async Task<ListResultDto<OrganizationUnitDtoBase>> GetAllMinifyAsync(AppResultRequestDto input)
        {
            var query = _organizationUnitRepository.GetAll()
                    .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), x => x.DisplayName.Contains(input.Keyword))
                    .Where(x => x.Status == 1)
                ;

            var items = await query.ToListAsync();

            return new ListResultDto<OrganizationUnitDtoBase>(
                items.Select(item =>
                {
                    var dto = ObjectMapper.Map<OrganizationUnitDtoBase>(item);
                    return dto;
                }).ToList());
        }

        [AbpAllowAnonymous]
        public async Task<PagedResultDto<ProjectNameOrganizationUnitDtoBase>> GetAllPublicAsync(AppResultRequestDto input)
        {
            var query = _organizationUnitRepository.GetAll()
                    .OrderBy(x => x.DisplayName)
                    .WhereIf(input.Status.HasValue, x => x.Status == input.Status)
                    .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), x => x.DisplayName.Contains(input.Keyword))
                    .Where(x => x.DisplayName != "演示用商户")
                ;

            var total = await query.CountAsync();

            query = query.PageBy(input);
            var items = await query.ToListAsync();

            return new PagedResultDto<ProjectNameOrganizationUnitDtoBase>(total,
                items.Select(item =>
                {
                    var dto = ObjectMapper.Map<ProjectNameOrganizationUnitDtoBase>(item);
                    return dto;
                }).ToList());
        }

        [AbpAllowAnonymous]
        public async Task<ProjectNameOrganizationUnitDtoBase> GetPublicAsync(EntityDto<int> input)
        {
            var query = await _organizationUnitRepository.GetAll().FirstOrDefaultAsync(z => z.Id == input.Id);

            return ObjectMapper.Map<ProjectNameOrganizationUnitDtoBase>(query);
        }


        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        public async Task<ListResultDto<OrganizationUnitDto>> GetAllOrganizationUnits(AppResultRequestDto input)
        {
            var query = _organizationUnitRepository.GetAll()
                    .WhereIf(input.Status.HasValue, x => x.Status == input.Status)
                    .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), x => x.DisplayName.Contains(input.Keyword))
                ;

            var items = await query
                .ToListAsync();

            return new ListResultDto<OrganizationUnitDto>(
                items.Select(item =>
                {
                    var dto = ObjectMapper.Map<OrganizationUnitDto>(item);
                    return dto;
                }).ToList());
        }


        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnits(AppResultRequestDto input)
        {
            var query = _organizationUnitRepository.GetAll()
                    .WhereIf(input.Status.HasValue, x => x.Status == input.Status)
                    .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), x => x.DisplayName.Contains(input.Keyword))
                ;

            var currentUserOuIds = (await UserManager.GetOrganizationUnitsAsync(await GetCurrentUserAsync())).Select(x => x.Id).ToList();
            var items = await query.Where(z => currentUserOuIds.Contains(z.Id))
                .ToListAsync();

            return new ListResultDto<OrganizationUnitDto>(
                items.Select(item =>
                {
                    var dto = ObjectMapper.Map<OrganizationUnitDto>(item);
                    return dto;
                }).ToList());
        }

        [AbpAllowAnonymous]
        public async Task<OrganizationUnitDto> GetCurrent()
        {
            var ouId = AbpSession.Get_OrganizationUnitId(); //  _currentOrganization.Id;
            if (!ouId.HasValue)
            {
                throw new UserFriendlyException("当前没有登录门店信息");
            }

            var ou = await _organizationUnitRepository.FirstOrDefaultAsync(x => x.Id == ouId);
            return ObjectMapper.Map<OrganizationUnitDto>(ou);
        }

        [AbpAllowAnonymous]
        public async Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input)
        {
            var query = from uou in _userOrganizationUnitRepository.GetAll()
                join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
                join user in UserManager.Users on uou.UserId equals user.Id
                where uou.OrganizationUnitId == input.Id
                select new {uou, user};

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(z => z.user.UserName).PageBy(input).ToListAsync();
            return new PagedResultDto<OrganizationUnitUserListDto>(
                totalCount,
                items.Select(item =>
                {
                    var dto = ObjectMapper.Map<OrganizationUnitUserListDto>(item.user);
                    dto.AddedTime = item.uou.CreationTime;
                    return dto;
                }).ToList());
        }


        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        public async Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input)
        {
            var organizationUnit = new ProjectNameOrganizationUnit(AbpSession.TenantId, input.DisplayName, input.ParentId);

            organizationUnit.Detail = input.Detail;

            await _organizationUnitManager.CreateAsync(organizationUnit);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnitDto>(organizationUnit);
        }

        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        public async Task<GetForEditOutput<CreateOrganizationUnitInput>> GetForEdit(EntityDto input)
        {
            ProjectNameOrganizationUnit find = null;
            if (input.Id > 0)
                find = await _organizationUnitRepository.FirstOrDefaultAsync(z => z.Id == input.Id);
            var schema = JToken.FromObject(new { });

            return new GetForEditOutput<CreateOrganizationUnitInput>(
                find != null ? ObjectMapper.Map<CreateOrganizationUnitInput>(find) : new CreateOrganizationUnitInput(),
                schema);
        }

        /// <summary>
        /// 更新机构信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        public async Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(input.Id);

            ObjectMapper.Map(input, organizationUnit);

            //organizationUnit.Resfuse("后台修改机构资料");

            await _organizationUnitManager.UpdateAsync(organizationUnit);

            return await CreateOrganizationUnitDto(organizationUnit);
        }

        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        public async Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input)
        {
            await _organizationUnitManager.MoveAsync(input.Id, input.NewParentId);

            return await CreateOrganizationUnitDto(
                await _organizationUnitRepository.GetAsync(input.Id)
            );
        }

        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        public async Task DeleteOrganizationUnit(EntityDto<long> input)
        {
            await _organizationUnitManager.DeleteAsync(input.Id);
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        public async Task RemoveUsersFromOrganizationUnit(UsersToOrganizationUnitInput input)
        {
            var count = await _userOrganizationUnitRepository.CountAsync(z =>
                z.OrganizationUnitId == input.OrganizationUnitId);

            if (count - input.UserIds.Length <= 0)
                throw new UserFriendlyException("机构最少需要一名成员,不能全部删除");
            foreach (var userId in input.UserIds)
            {
                await UserManager.RemoveFromOrganizationUnitAsync(userId, input.OrganizationUnitId);
                await CurrentUnitOfWork.SaveChangesAsync();

                var count2 = await _userOrganizationUnitRepository.CountAsync(x => x.UserId == userId);
                if (count2 == 0)
                    await UserManager.RemoveFromRoleAsync(
                        await UserManager.GetUserByIdAsync(userId), StaticRoleNames.Tenants.Organize);
            }
        }

        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        public async Task RemoveUserFromOrganizationUnit(UserToOrganizationUnitInput input)
        {
            await UserManager.RemoveFromOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
            await CurrentUnitOfWork.SaveChangesAsync();
            var count = await _userOrganizationUnitRepository.CountAsync(x => x.UserId == input.UserId);
            if (count == 0)
                await UserManager.RemoveFromRoleAsync(
                    await UserManager.GetUserByIdAsync(input.UserId), StaticRoleNames.Tenants.Organize);
        }

        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        public async Task AddUsersToOrganizationUnit(UsersToOrganizationUnitInput input)
        {
            foreach (var userId in input.UserIds)
            {
                await UserManager.AddToOrganizationUnitAsync(userId, input.OrganizationUnitId);
                await UserManager.AddToRoleAsync(await UserManager.GetUserByIdAsync(userId), StaticRoleNames.Tenants.Organize);
            }
        }

        [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindOrganizationUnitUsersInput input)
        {
            var userIdsInOrganizationUnit = _userOrganizationUnitRepository.GetAll()
                .Where(uou => uou.OrganizationUnitId == input.OrganizationUnitId)
                .Select(uou => uou.UserId);

            var query = UserManager.Users
                .Where(u => !userIdsInOrganizationUnit.Contains(u.Id))
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter)
                );

            var userCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<NameValueDto>(
                userCount,
                users.Select(u =>
                    new NameValueDto(
                        u.FullName + " (" + u.EmailAddress + ")",
                        u.Id.ToString()
                    )
                ).ToList()
            );
        }

        [HttpPost]
        public async Task<string> PostEvent(EntityEventDto<int> input)
        {
            try
            {
                var find = await _organizationEventRepository.GetAll()
                    .FirstOrDefaultAsync(z =>
                        z.EventType == input.EventType && z.OrganizationUnitId == input.Id &&
                        z.CreatorUserId == AbpSession.GetUserId());
                if (input.Event.ToLower() == "delete")
                {
                    if (find == null)
                        throw new UserFriendlyException(L("NotFind"));
                    await _organizationEventRepository.DeleteAsync(find);
                }
                else
                {
                    switch (input.EventType)
                    {
                        case (2): //关注
                            if (input.Event.ToLower() == "create")
                            {
                                if (find == null)
                                {
                                    await _organizationEventRepository.InsertAsync(new OrganizationEvent
                                        {EventType = 2, OrganizationUnitId = input.Id});
                                }
                                else
                                {
                                    throw new UserFriendlyException("已关注");
                                }
                            }

                            break;
                    }
                }

                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        private async Task<OrganizationUnitDto> CreateOrganizationUnitDto(OrganizationUnit organizationUnit)
        {
            var dto = ObjectMapper.Map<OrganizationUnitDto>(organizationUnit);
            dto.MemberCount = await _userOrganizationUnitRepository.CountAsync(uou => uou.OrganizationUnitId == organizationUnit.Id);
            return dto;
        }
    }
}