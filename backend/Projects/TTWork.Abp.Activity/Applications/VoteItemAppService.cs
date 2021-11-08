using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using AwsomeApi.WeixinWork.Message;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.Activity.Definitions;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.AuditManagement.Applications;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.LaborUnion.Events.Commands;

namespace TTWork.Abp.Activity.Applications
{
    public class VoteItemAppService : AuditAsyncCrudAppService<VoteItem, VoteItemDto, Guid, AppResultRequestDto, VoteItemCreateOrUpdateDto, VoteItemCreateOrUpdateDto>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<VotePlan, long> _votePlanRepository;

        public VoteItemAppService(
            IMediator mediator,
            IRepository<VoteItem, Guid> repository,
            IRepository<VotePlan, long> votePlanRepository,
            IocManager iocManager)
            : base(repository, iocManager)
        {
            _mediator = mediator;
            _votePlanRepository = votePlanRepository;
            base.GetAllPermissionName = ActivityPermissions.Default;
            base.DeletePermissionName = ActivityPermissions.Admin;
            base.CreatePermissionName = AppPermissions.Pages.Default;
            base.UpdatePermissionName = AppPermissions.Pages.Default;

            EnableGetEdit = true;
            AuditName = VoteAudit.VoteItem;

            AuditedCanEdit = true;
        }


        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<GetForEditOutput<VoteItemCreateOrUpdateDto>> GetForEditFromPlan(EntityDto<long> input)
        {
            var plan = await _votePlanRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.Id);

            if (plan == null)
                throw new UserFriendlyException($"找不到投票活动 Id {input.Id}");

            var entity = await Repository.FirstOrDefaultAsync(x => x.VotePlanId == plan.Id && x.CreatorUserId == AbpSession.UserId.Value);

            if (entity == null)
            {
                entity = new VoteItem()
                {
                    VotePlanId = plan.Id,
                    State = 0,
                    Sort = 0,
                    CreatorUserId = AbpSession.UserId,
                    TenantId = plan.TenantId
                };
                await Repository.InsertOrUpdateAndGetIdAsync(entity);
                await CurrentUnitOfWork.SaveChangesAsync();
            }


            return new GetForEditOutput<VoteItemCreateOrUpdateDto>(ObjectMapper.Map<VoteItemCreateOrUpdateDto>(entity));
        }


        public override Task<GetForEditOutput<VoteItemCreateOrUpdateDto>> GetForEdit(EntityDto<Guid> input)
        {
            return base.GetForEdit(input);
        }

        public override async Task<VoteItemDto> UpdateAsync(VoteItemCreateOrUpdateDto input)
        {
            var result = await base.UpdateAsync(input);
            return result;
        }

        protected override Task<VoteItem> GetEntityByIdAsync(Guid id)
        {
            return Repository.GetAll().Include(x => x.VotePlan).FirstOrDefaultAsync(x => x.Id == id);
        }


        protected override async Task BeforeUpdateAsync(VoteItemCreateOrUpdateDto input = default)
        {
            var plan = await _votePlanRepository.FirstOrDefaultAsync(x => x.Id == input.VotePlanId);

            if (plan == null)
                throw new UserFriendlyException($"找不到投票活动 Id {input!.VotePlanId}");

            if (plan.UploadEndTime.HasValue && DateTime.Now > plan.UploadEndTime)
                throw new UserFriendlyException($"已超过上传结束时间 {plan.UploadEndTime}");

            if (input!.State is 5 or 6)
            {
                throw new UserFriendlyException("审核中或已通过不能进行修改");
            }
        }


        protected override async Task AfterUpdateAsync(VoteItem entity, VoteItemCreateOrUpdateDto input = default)
        {
            entity.State = EnumClass.ListState.Submitted;
            await CurrentUnitOfWork.SaveChangesAsync();
            await StartAudit(entity);

            #region 发送通知到企业微信

            try
            {
                var data = new SendWechatWorkAppDetail(new MarkdownMessage
                    {
                        agentid = 1000004,
                        touser = "TangJiaWei",
                        markdown = new MessageContentWrap
                        {
                            content = $@"{entity.VotePlan.Title}
> 书屋名称：{entity.Form["name"]}
> 书屋地址：{entity.Form["address"]}
> 书屋简介：{entity.Form["desc"]}
> 如需查看详细，请点击：[查看详细](https://szsj.wujiangapp.com/#/vote/voteItemList)"
                        }
                    }
                );

                await _mediator.Publish(new MessageSendCommand(MessageType.WechatWorkApp, data));
            }
            catch (Exception)
            {
                // ignored
            }

            #endregion
        }

        protected override IQueryable<VoteItem> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                    .WhereIf(input.Status.HasValue, x => (int)x.State == input.Status)
                ;
        }
    }
}