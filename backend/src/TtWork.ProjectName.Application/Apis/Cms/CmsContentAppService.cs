using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TT.Extensions;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Extensions;
using TtWork.ProjectName.Apis.Cms.Dtos;
using TtWork.ProjectName.Apis.Dtos;
using TtWork.ProjectName.Entities.Cms;
using TtWork.ProjectName.Users;

namespace TtWork.ProjectName.Apis.Cms
{
    public interface ICmsContentAppService : IAsyncCrudAppService<CmsContentDto, int, AppResultRequestDto,
        CmsContentCreateOrUpdateDto, CmsContentCreateOrUpdateDto>
    {
        Task<GetForEditOutput<CmsContentCreateOrUpdateDto>> GetForEdit(EntityDto input);
        Task PostEvent(EntityEventDto<int> input);
    }


    /// <summary>
    /// 文章/内容
    /// </summary>
    public class
        CmsContentAppService :
            AsyncCrudAppService<CmsContent, CmsContentDto, int, AppResultRequestDto, CmsContentCreateOrUpdateDto,
                CmsContentCreateOrUpdateDto>, ICmsContentAppService
    {
        private readonly IRepository<CmsCategory> _categoryRepository;
        private readonly IRepository<CmsContentEvent> _cmsContentEventRepository;

        public CmsContentAppService(
            IRepository<CmsContent, int> repository,
            IRepository<CmsCategory> categoryRepository,
            IRepository<CmsContentEvent> cmsContentEventRepository) : base(repository)
        {
            _categoryRepository = categoryRepository;
            _cmsContentEventRepository = cmsContentEventRepository;
        }

        public async Task<GetForEditOutput<CmsContentCreateOrUpdateDto>> GetForEdit(EntityDto input)
        {
            CmsContent find = null;
            if (input.Id > 0)
                find = await Repository.FirstOrDefaultAsync(z => z.Id == input.Id);
            var schema = JToken.FromObject(new { });

            var categoryList = await _categoryRepository.GetAllListAsync();

            schema["categoryId"] = categoryList.GetSelection("number", "categoryId", @"{0}", new[] { "Name" }, "Id");

            schema["linkTypes"] = typeof(CmsContentLinkType).GetEnumSelection();

            return new GetForEditOutput<CmsContentCreateOrUpdateDto>(
                find != null ? ObjectMapper.Map<CmsContentCreateOrUpdateDto>(find) : new CmsContentCreateOrUpdateDto(),
                schema);
        }

        public override async Task<PagedResultDto<CmsContentDto>> GetAllAsync(AppResultRequestDto input)
        {
            return await base.GetAllAsync(input);
        }

        public async Task<PagedResultDto<CmsContentDto>> GetAllPublish(AppResultRequestDto input)
        {
            input.Sorting = "creationTime desc";
            input.Status = 1;
            return await base.GetAllAsync(input);
        }

        /// <summary>
        /// 让GetAll输出带有category
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<CmsContent> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                    .Include(x => x.Category)
                    .WhereIf(!string.IsNullOrEmpty(input.Keyword), s => s.Title.Contains(input.Keyword))
                    .WhereIf(input.Pid.HasValue, s => s.CategoryId == input.Pid)
                    .WhereIf(input.Status.HasValue, s => s.Status == input.Status)
                ;
        }


        public override async Task DeleteAsync(EntityDto<int> input)
        {
            await base.DeleteAsync(input);
        }

        /// <summary>
        /// 获取新闻单条记录
        /// </summary>
        [AbpAllowAnonymous]
        public override async Task<CmsContentDto> GetAsync(EntityDto<int> input)
        {
            var find = await Repository.GetAllIncluding(x => x.CreatorUser).FirstOrDefaultAsync(x => x.Id == input.Id);

            if (find == null)
                throw new UserFriendlyException($"该条信息[{input.Id}]已不存在");

            //新闻查看次数加1
            find.SetViewCount();

            var item = ObjectMapper.Map<CmsContentDto>(find);
            item.CreateUserName = find.CreatorUser.UserName;
            item.CreationTime = find.CreationTime;

            try
            {
                if (AbpSession.UserId > 0)
                {
                    var collectionEvent = from z in
                            _cmsContentEventRepository.GetAll()
                                .Where(z =>
                                    z.CmsContentId == item.Id && z.CreatorUserId == AbpSession.UserId)
                        select z.EventType;

                    item.UserEvents = (await collectionEvent.ToListAsync()).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, e);
            }

            return item;
        }


        /// <summary>
        /// 收藏/分享/点赞 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        [HttpPost]
        public async Task PostEvent(EntityEventDto<int> input)
        {
            var cmsContent = await Repository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (cmsContent == null)
                throw new UserFriendlyException($"该条信息[{input.Id}]已不存在");

            var myEvent = await _cmsContentEventRepository.FirstOrDefaultAsync(x =>
                x.CmsContentId == input.Id && x.CreatorUserId == AbpSession.UserId && (int)x.EventType == input.EventType);

            if (input.Event.ToLower() == "delete")
            {
                if (myEvent == null)
                    await Task.CompletedTask;
                else
                {
                    await _cmsContentEventRepository.DeleteAsync(myEvent);
                    await Task.CompletedTask;
                }
            }
            else
            {
                switch (input.EventType)
                {
                    case ((int)EventTypeEnum.Favorite): //收藏
                        if (myEvent == null)
                            await _cmsContentEventRepository.InsertAsync(new CmsContentEvent()
                                { EventType = EventTypeEnum.Favorite, CmsContentId = input.Id });
                        break;
                    case ((int)EventTypeEnum.Share): //分享
                        if (myEvent == null)
                            await _cmsContentEventRepository.InsertAsync(new CmsContentEvent()
                                { EventType = EventTypeEnum.Share, CmsContentId = input.Id });
                        break;
                }
            }
        }

        public override Task<CmsContentDto> CreateAsync(CmsContentCreateOrUpdateDto input)
        {
            var cu = CurrentUnitOfWork.Filters;
            var result = base.CreateAsync(input);
            return result;
        }

        public override async Task<CmsContentDto> UpdateAsync(CmsContentCreateOrUpdateDto input)
        {
            //var result = await base.UpdateAsync(input);

            CheckUpdatePermission();

            var entity = await Repository.GetAll()
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            if (!input.Content.IsNullOrEmptyOrWhiteSpace())
                input.Content = Regex.Replace(input.Content, @"(style=""height:\d+px; width:\d+px"")", @"class=""img""");

            ObjectMapper.Map(input, entity);

            await Repository.UpdateAsync(entity);

            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 发布文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Publish(EntityDto<int> input)
        {
            var entity = await this.Repository.GetAsync(input.Id);
            entity.Status = (int)EnumClass.CmsContentStatus.Published;
        }

        /// <summary>
        /// 取消发布
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CancelPublish(EntityDto<int> input)
        {
            var entity = await this.Repository.GetAsync(input.Id);
            entity.Status = (int)EnumClass.CmsContentStatus.Draft;
        }
    }
}