using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TT.Extensions;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.AuditManagement.Events;
using TTWork.Abp.AuditManagement.Events.Queries;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Events.Queries;
using TTWork.Abp.LaborUnion.Definitions;
using TTWork.Abp.LaborUnion.Events.Commands;

namespace TTWork.Abp.Activity.Events
{
    public class VoteItemAuditQuery : IAuditQueryBase
    {
        public AuditUserLog Input { get; set; }
        public AuditFlow Flow { get; set; }
        public AuditNode Node { get; set; }


        public class VoteItemAuditQueryHandle : AuditQueryHandlerBase<VoteItemAuditQuery, VoteItem, Guid>
        {
            private readonly IRepository<VotePlan, long> _votePlanRepository;
            private readonly IMediator _mediator;

            public VoteItemAuditQueryHandle(
                IRepository<VoteItem, Guid> repository,
                IRepository<VotePlan, long> votePlanRepository,
                IMediator mediator,
                IIocManager iocManager) : base(repository, iocManager)
            {
                _votePlanRepository = votePlanRepository;
                _mediator = mediator;
            }

            [UnitOfWork]
            protected override async Task Do(VoteItem entity, VoteItemAuditQuery request = null)
            {
                entity.State = EnumClass.ListState.Approved;

                // 发送审核通过消息
                var openid = await _mediator.Send(new UserLoginKeyQuery(entity.CreatorUserId!.Value, TTWorkConsts.LoginProvider.WeChatMiniProgram));
                if (!openid.IsNullOrEmptyOrWhiteSpace())
                {
                    var plan = await _votePlanRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.VotePlanId);
                    var detail = new SendWechatTemplateDetail(
                        ProjectApp.ZGH_MINI,
                        new[] { openid },
                        "ebDtVHLQyqngwwHxsODBfi054-eeHrfUDyeFD6DmiZc",
                        new
                        {
                            name1 = new { value = $"{entity.Form["name"]}" }, //申请人
                            thing3 = new { value = $"{plan.Title}" }, //申请类型
                            phrase7 = new { value = "完成审核" }, //审核结果
                            date4 = new { value = $"{DateTime.Now:d}" }, //审核时间
                            thing5 = new { value = $"点击查看" }, //备注
                        }, $"pages/vote/index?id={entity.VotePlanId}"
                    );

                    await _mediator.Publish(new MessageSendCommand(MessageType.WechatTemplate, detail));
                }


                await Task.CompletedTask;
            }

            [UnitOfWork]
            protected override async Task Reject(VoteItem entity, VoteItemAuditQuery request = null)
            {
                entity.State = EnumClass.ListState.SendBacked;
                if (request is { Input: { } }) entity.RejectText = request.Input.Desc;

                var openid = await _mediator.Send(new UserLoginKeyQuery(entity.CreatorUserId!.Value, TTWorkConsts.LoginProvider.WeChatMiniProgram));
                if (!openid.IsNullOrEmptyOrWhiteSpace())
                {
                    var plan = await _votePlanRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.VotePlanId);

                    await _mediator.Publish(new MessageSendCommand(MessageType.WechatTemplate, new SendWechatTemplateDetail(
                        ProjectApp.ZGH_MINI,
                        new[] { openid },
                        "ebDtVHLQyqngwwHxsODBfi054-eeHrfUDyeFD6DmiZc",
                        new
                        {
                            name1 = new { value = $"{entity.Form["name"]}" }, //申请人
                            thing3 = new { value = $"{plan.Title}" }, //申请类型
                            phrase7 = new { value = "审核失败" }, //审核结果
                            date4 = new { value = $"{DateTime.Now:d}" }, //审核时间
                            thing5 = new { value = $"{request?.Input.Desc ?? ""} 点击修改" }, //备注
                        }, $"pages/vote/index?id={entity.VotePlanId}"
                    )));
                }

                await Task.CompletedTask;
            }
        }
    }
}