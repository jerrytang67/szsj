using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using TtWork.ProjectName.Apis.Dtos;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Definitions;
using TtWork.ProjectName.Entities.Users;

namespace TtWork.ProjectName.Apis.Users
{
    public class UserEventAppService : AsyncCrudAppService<UserEvent, UserEventDto, long, AppResultRequestDto, UserEventDto, UserEventDto>
    {
        public UserEventAppService(IRepository<UserEvent, long> repository) : base(repository)
        {
            base.CreatePermissionName = AppPermissions.Pages.Administration.Default;
            base.UpdatePermissionName = AppPermissions.Pages.Administration.Default;
            base.DeletePermissionName = AppPermissions.Pages.Administration.Default;
        }

        protected override IQueryable<UserEvent> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                .Include(x => x.CreatorUser)
                .Include(x => x.FromUser);
        }

        [Obsolete]
        public override Task<UserEventDto> CreateAsync(UserEventDto input)
        {
            return base.CreateAsync(input);
        }

        [Obsolete]
        public override Task<UserEventDto> UpdateAsync(UserEventDto input)
        {
            return base.UpdateAsync(input);
        }

        [Obsolete]
        public override Task DeleteAsync(EntityDto<long> input)
        {
            return base.DeleteAsync(input);
        }
    }

    [AutoMapFrom(typeof(UserEvent))]
    public class UserEventDto : EntityDto<long>
    {
        /// <summary>
        /// 1 扫码访问 
        /// </summary>
        public int EventType { get; set; } = 0;

        public Dictionary<string, string> Value { get; set; }

        public DateTime CreationTime { get; set; }

        public UserDtoBase CreatorUser { get; set; }


        public UserDtoBase FromUser { get; set; }
    }
}