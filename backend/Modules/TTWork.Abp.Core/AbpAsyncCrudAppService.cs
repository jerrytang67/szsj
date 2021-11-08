using System;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Extensions;

namespace TTWork.Abp.Core
{
    public class AbpAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> :
        AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected bool EnableGetEdit = false;
        public UserManager UserManager { get; set; }

        public IWxTemplateMsgSender WxSender;

        public AbpAsyncCrudAppService(
            IRepository<TEntity, TPrimaryKey> repository,
            IocManager iocManager
        )
            : base(repository)
        {
            LocalizationSourceName = AbpConsts.LocalizationSourceName;
            Logger = NullLogger.Instance;
            WxSender = iocManager.Resolve<IWxTemplateMsgSender>();
        }


        public virtual async Task<GetForEditOutput<TCreateInput>> GetForEdit(EntityDto<TPrimaryKey> input)
        {
            if (!EnableGetEdit)
                throw new UserFriendlyException("GetForEdit is not enable");

            var entity = input.Id switch
            {
                (int and > 0) or (long and > 0) => await Repository.FirstOrDefaultAsync(z => z.Id.Equals(input.Id)),
                Guid => await Repository.FirstOrDefaultAsync(z => z.Id.Equals(input.Id)),
                _ => null
            };

            var schema = JToken.FromObject(new { });

            return new GetForEditOutput<TCreateInput>(
                entity != null
                    ? ObjectMapper.Map<TCreateInput>(entity)
                    : Activator.CreateInstance<TCreateInput>(),
                schema);
        }


        public override async Task<TEntityDto> UpdateAsync(TUpdateInput input)
        {
            CheckUpdatePermission();
            TEntity entity = await GetEntityByIdAsync(input.Id).ConfigureAwait(false);
            MapToEntity(input, entity);
            Repository.GetDbContext().Entry(entity).State = EntityState.Modified;
            await Repository.UpdateAsync(entity);
            //如果不加这句,更新时含有json convention的会不更新
            await CurrentUnitOfWork.SaveChangesAsync().ConfigureAwait(false);

            TEntityDto entityDto = MapToEntityDto(entity);
            entity = default(TEntity);
            return entityDto;
        }


        protected async Task<bool> IsInRoleAsync(long userId, string roleName)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var roles = await UserManager.GetRolesAsync(user);
            return roles.Any(x => String.Equals(x, roleName,
                StringComparison.CurrentCultureIgnoreCase));
        }

        protected async Task<bool> IsAdminAsync(string roleName = "admin")
        {
            if (!AbpSession.UserId.HasValue)
                return false;
            return await IsInRoleAsync(AbpSession.UserId.Value, roleName);
        }

        protected async Task<User> GetCurrentUserAsync()
        {
            return await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
        }
    }
}