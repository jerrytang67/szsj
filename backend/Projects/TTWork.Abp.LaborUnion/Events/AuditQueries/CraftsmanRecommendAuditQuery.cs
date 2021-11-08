using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Core.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TT.Extensions;
using TTWork.Abp.AppManagement.Events;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.AuditManagement.Events;
using TTWork.Abp.AuditManagement.Events.Queries;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Events.Queries;
using TTWork.Abp.Core.Extensions;
using TTWork.Abp.LaborUnion.Definitions;
using TTWork.Abp.LaborUnion.Domains;
using TTWork.Abp.LaborUnion.Events.Commands;

namespace TTWork.Abp.LaborUnion.Events.AuditQueries
{
    public class CraftsmanRecommendAuditQuery : IAuditQueryBase
    {
        public AuditUserLog Input { get; set; }
        public AuditFlow Flow { get; set; }
        public AuditNode Node { get; set; }

        public class CraftsmanRecommendAuditQueryHandle : AuditQueryHandlerBase<CraftsmanRecommendAuditQuery, CraftsmanRecommend, long>
        {
            private readonly IRepository<CraftsmanRecommend, long> _repository;
            private readonly IRepository<Craftsman, long> _Repo;
            private readonly ILogger _logger;
            private readonly IObjectMapper _mapper;
            private readonly IUnitOfWorkManager _unitOfWorkManager;
            private readonly IAbpSession _abpSession;
            private readonly IMediator _mediator;
            private readonly IWxSubscribeMessageSender _sender;

            public CraftsmanRecommendAuditQueryHandle(
                IRepository<CraftsmanRecommend, long> repository,
                IIocManager iocManager,
                ILogger logger,
                IObjectMapper mapper,
                IUnitOfWorkManager unitOfWorkManager,
                IAbpSession abpSession,
                IMediator mediator,
                IWxSubscribeMessageSender sender, IRepository<Craftsman, long> repo) : base(repository, iocManager)
            {
                _repository = repository;
                _logger = logger;
                _mapper = mapper;
                _unitOfWorkManager = unitOfWorkManager;
                _abpSession = abpSession;
                _mediator = mediator;
                _sender = sender;
                _Repo = repo;
            }

            [UnitOfWork]
            protected override async Task Do(CraftsmanRecommend entity, CraftsmanRecommendAuditQuery request = null)
            {
                if (entity.State == RecomandState.推荐成功)
                {
                    throw new UserFriendlyException("已经推荐成功了");
                }

                var t1 = await _Repo.GetAll().AsNoTracking().Where(x => x.RedpacketRecived).SumAsync(x => x.Redpacket);
                var t2 = await _repository.GetAll().AsNoTracking().Where(x => x.RedpacketRecived).SumAsync(x => x.Redpacket);
                
                if ((t1 + t2) > 10000)
                {
                    // throw new UserFriendlyException("红包发放总金额已达上限");
                    entity.RedpacketRecived = true;
                    entity.Redpacket = 0;
                }
                else
                {
                    entity.Redpacket = new Random().Next(100, 100) / 100m;
                }

                entity.SecurityStamp ??= Guid.NewGuid();
                entity.State = RecomandState.推荐成功;
                var openid = await _mediator.Send(new UserLoginKeyQuery(entity.CreatorUserId!.Value, TTWorkConsts.LoginProvider.WeChatMiniProgram));
                if (!openid.IsNullOrEmptyOrWhiteSpace())
                {
                    var detail = new SendWechatTemplateDetail(
                        ProjectApp.ZGH_MINI,
                        new[] {openid},
                        "ebDtVHLQyqngwwHxsODBfi054-eeHrfUDyeFD6DmiZc",
                        new
                        {
                            name1 = new {value = $"{entity.Detail.Realname}"}, //申请人
                            thing3 = new {value = "推荐吴江“红色工匠”候选人"}, //申请类型
                            phrase7 = new {value = "完成审核"}, //审核结果
                            date4 = new {value = $"{DateTime.Now:d}"}, //审核时间
                            // thing5 = new {value = $"点击消息去领取红包"}, //备注
                            thing5 = new {value = $"点击查看"}, //备注
                        }, "pages/craftsman/myRecommendOther"
                    );

                    await _mediator.Publish(new MessageSendCommand(MessageType.WechatTemplate, detail));
                }

                entity.State = RecomandState.推荐成功;
            }

            [UnitOfWork]
            protected override async Task Reject(CraftsmanRecommend entity, CraftsmanRecommendAuditQuery request = null)
            {
                var openid = await _mediator.Send(new UserLoginKeyQuery(entity.CreatorUserId!.Value, TTWorkConsts.LoginProvider.WeChatMiniProgram));
                if (!openid.IsNullOrEmptyOrWhiteSpace())
                {
                    await _mediator.Publish(new MessageSendCommand(MessageType.WechatTemplate, new SendWechatTemplateDetail(
                        ProjectApp.ZGH_MINI,
                        new[] {openid},
                        "ebDtVHLQyqngwwHxsODBfi054-eeHrfUDyeFD6DmiZc",
                        new
                        {
                            name1 = new {value = $"{entity.Detail.Realname}"}, //申请人
                            thing3 = new {value = "推荐吴江“红色工匠”候选人"}, //申请类型
                            phrase7 = new {value = "审核失败"}, //审核结果
                            date4 = new {value = $"{DateTime.Now:d}"}, //审核时间
                            thing5 = new {value = $"{request?.Input.Desc ?? ""} 点击修改"}, //备注
                        }, $"pages/craftsman/formRecommendOther?id={entity.Id}"
                    )));
                }

                if (request != null)
                    entity.RejectText = request!.Input.Desc;

                entity.State = RecomandState.推荐失败;
            }
        }
    }
}