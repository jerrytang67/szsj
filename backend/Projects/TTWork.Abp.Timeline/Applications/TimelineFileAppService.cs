using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using AwsomeApi.WeixinWork.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT.Extensions;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.LaborUnion.Events.Commands;
using TTWork.Abp.Timeline.Definitions;
using TTWork.Abp.Timeline.Domains;
using Z.EntityFramework.Extensions;

namespace TTWork.Abp.Timeline.Applications
{
    public class TimelineFileAppService : AbpAppServiceBase
    {
        private readonly IRepository<TimelineFile, Guid> _repository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IMediator _mediator;

        public TimelineFileAppService(
            IRepository<TimelineFile, Guid> repository,
            IGuidGenerator guidGenerator,
            IMediator mediator
        )
        {
            _repository = repository;
            _guidGenerator = guidGenerator;
            _mediator = mediator;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<TimelineFileDto> CreateAsync(TimelineFileCreateOrUpdateDto input)
        {
            var entity = ObjectMapper.Map<TimelineFile>(input);
            entity.Sort = 99999;
            await _repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return ObjectMapper.Map<TimelineFileDto>(entity);
        }

        [HttpGet]
        public async Task<PagedResultDto<TimelineFileDto>> GetAllAsync(AppResultRequestDto input)
        {
            input.MaxResultCount = 500;

            var query = _repository.GetAll()
                    .AsNoTracking()
                    .WhereIf(input.Pid.HasValue, x => x.EventId == input.Pid)
                    .WhereIf(input.Status.HasValue, x => x.State == input.Status)
                    .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), x => x.Desc.Contains(input.Keyword))
                ;

            var totalCount = await query.CountAsync();

            var items = await query.OrderByDescending(z => z.Sort).PageBy(input).ToListAsync();

            return new PagedResultDto<TimelineFileDto>(totalCount, ObjectMapper.Map<List<TimelineFileDto>>(items));
        }

        [HttpPost]
        [AbpAuthorize(TimelinePermissions.Admin)]
        public async Task PostPublishList(FilePublishRequestDto input)
        {
            var updateCount = 0;
            var sw = new Stopwatch();
            sw.Start();

            var list = await _repository.GetAll().Where(x => x.EventId == input.EventId).ToListAsync();

            foreach (var item in list)
            {
                if (input.State0List.Contains(item.Id))
                {
                    if (item.State != 0)
                    {
                        item.State = 0;
                        updateCount++;
                    }
                }

                if (input.State1List.Contains(item.Id))
                {
                    if (item.State != 1)
                    {
                        item.State = 1;
                        updateCount++;
                    }
                }
            }

            var sortmax0 = input.State1List.Count;

            foreach (var a in input.State0List.Select(fileId => list.FirstOrDefault(x => x.Id == fileId)))
            {
                if (a != null && a.Sort != sortmax0)
                    a.Sort = sortmax0;

                sortmax0--;
                updateCount++;
            }

            var sortmax1 = input.State1List.Count;
            foreach (var a in input.State1List.Select(fileId => list.FirstOrDefault(x => x.Id == fileId)))
            {
                if (a != null && a.Sort != sortmax1)
                    a.Sort = sortmax1;

                sortmax1--;
                updateCount++;
            }

            EntityFrameworkManager.ContextFactory = _ => _repository.GetDbContext();

            await _repository.GetDbContext().BulkUpdateAsync(list);
            await _repository.GetDbContext().BulkSaveChangesAsync();

            sw.Stop();
            Logger.Warn($"更新{updateCount}条TimelineFile记录耗时 {sw.ElapsedMilliseconds / 1000.0}秒");
        }


        #region 小程序端

        [AbpAuthorize(AppPermissions.Pages.Default)]
        [HttpPost]
        public async Task PostAddFiles(AddFilesReqestDto input)
        {
            var entities = new List<TimelineFile>();

            foreach (var _t in input.ImageList.Select(img => new TimelineFile(input.EventId, EnumClass.TimelineFileType.Image, img)))
            {
                foreach (KeyValuePair<string, string> item in input.Data)
                {
                    _t.Id = _guidGenerator.Create();
                    _t.TenantId = AbpSession.TenantId ?? 1;
                    _t.CreationTime = DateTime.Now;
                    _t.CreatorUserId = AbpSession.UserId;


                    if (item.Key == "communityName" && item.Value.IsNullOrEmptyOrWhiteSpace())
                        throw new UserFriendlyException("社团名称必填");
                    if (item.Key == "datetime" && item.Value.IsNullOrEmptyOrWhiteSpace())
                        throw new UserFriendlyException("活动时间必填");
                    if (item.Key == "address" && item.Value.IsNullOrEmptyOrWhiteSpace())
                        throw new UserFriendlyException("活动地点必填");

                    _t.SetData(item.Key, item.Value);
                }

                entities.Add(_t);
            }

            if (entities.Count > 0)
            {
                EntityFrameworkManager.ContextFactory = _ => _repository.GetDbContext();

                await _repository.GetDbContext().BulkInsertAsync(entities);
                await _repository.GetDbContext().BulkSaveChangesAsync();


                var data = new SendWechatWorkAppDetail(new MarkdownMessage
                    {
                        agentid = 1000004,
                        touser = "TangJiaWei",
                        markdown = new MessageContentWrap
                        {
                            content = $@"时间轴活动[编号:{input.EventId}]有{entities.Count}张新图片上传
> 如需查看详细，请点击：[查看详细](https://szsj.wujiangapp.com/admin/index.html?file=&file=#/timeline/timelineEventList)"
                        }
                    }
                );

                await _mediator.Publish(new MessageSendCommand(MessageType.WechatWorkApp, data));
            }
        }

        #endregion
    }

    public class AddFilesReqestDto
    {
        public long? EventId { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public List<string> ImageList { get; set; }
    }

    public class FilePublishRequestDto
    {
        public long EventId { get; set; }

        public List<Guid> State0List { get; set; }

        public List<Guid> State1List { get; set; }
    }
}