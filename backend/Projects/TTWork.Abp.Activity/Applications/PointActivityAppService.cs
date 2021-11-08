using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.UI;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT.Extensions;
using TT.Extensions.Redis;
using TTWork.Abp.Activity.Definitions;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.Activity.Events.Commands;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.Core.Events.Queries;
using static System.Int64;

namespace TTWork.Abp.Activity.Applications
{
    public class PointActivityAppService : AbpAsyncCrudAppService<PointActivity,
        PointActivityDto, long, AppResultRequestDto, PointActivityCreateOrUpdateDto, PointActivityCreateOrUpdateDto>
    {
        private readonly IRepository<PointActivityUserLog, long> _pointActivityUserLogRepositiry;
        private readonly IRepository<UserPointLog, long> _userPointRepository;
        private readonly IMediator _mediator;
        private readonly IRedisClient _redisClient;

        public PointActivityAppService(
            IRepository<PointActivity, long> repository,
            IRepository<PointActivityUserLog, long> pointActivityUserLogRepositiry,
            IRepository<UserPointLog, long> userPointRepository,
            IMediator mediator,
            IRedisClient redisClient,
            IocManager iocManager
        ) : base(repository, iocManager)
        {
            _pointActivityUserLogRepositiry = pointActivityUserLogRepositiry;
            _userPointRepository = userPointRepository;
            _mediator = mediator;
            _redisClient = redisClient;
            EnableGetEdit = true;

            base.GetAllPermissionName = ActivityPermissions.Default;
            base.CreatePermissionName = ActivityPermissions.Admin;
            base.UpdatePermissionName = ActivityPermissions.Admin;
            base.DeletePermissionName = ActivityPermissions.Admin;
        }

        [AbpAuthorize(AppPermissions.Pages.Default)]
        [HttpPost]
        public async Task<PointActivityUserLogDto> PostGetPoint(GetPointRequestDto input)
        {
            long shareFrom;
            TryParse(input.ShareFrom, out shareFrom);

            var activity = await Repository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.ActivityId);

            if (shareFrom > 0 && AbpSession.UserId!.Value == shareFrom)
            {
                shareFrom = 0;
            }

            var startTime = Convert.ToDateTime($"{DateTime.Today:yyyy-MM-dd} {activity.TimeStart}");
            var endTime = Convert.ToDateTime($"{DateTime.Today:yyyy-MM-dd} {activity.TimeEnd}");

            //checktime
            if (DateTime.Now < activity.DatetimeStart || DateTime.Now < startTime)
            {
                throw new UserFriendlyException("活动未开始");
            }

            if (DateTime.Now > activity.DatetimeEnd || DateTime.Now > endTime)
            {
                throw new UserFriendlyException("活动已结束");
            }

            //检查自己是否已经参与过

            var todayCount = await _pointActivityUserLogRepositiry.GetAll().AsNoTracking().CountAsync(x => x.CreationTime > DateTime.Today &&
                                                                                                           x.ActivityId == input.ActivityId &&
                                                                                                           x.CreatorUserId == AbpSession.UserId
            );

            if (todayCount >= 1 + activity.HelpPerDay)
            {
                throw new UserFriendlyException("今天已经不能再抽奖了");
            }


            var shareCount = await _pointActivityUserLogRepositiry.GetAll().AsNoTracking()
                .CountAsync(x => x.CreationTime > DateTime.Today &&
                                 x.ActivityId == input.ActivityId &&
                                 x.SharedFrom == AbpSession.UserId);

            if (todayCount == 1 && shareCount == 0)
            {
                throw new UserFriendlyException($"今天已经抽过1次奖了,分享给好友还能再抽1次");
            }

            //没有参与过,新建记录
            var point = new Random().Next(activity.MinPoint, activity.MaxPoint);
            point = Convert.ToInt32(point.ToString().Replace("4", "3")); //去4

            var log = new PointActivityUserLog(input.ActivityId, shareFrom > 0 ? shareFrom : null, point);

            await _pointActivityUserLogRepositiry.InsertAsync(log);
            await CurrentUnitOfWork.SaveChangesAsync();

            var currentUser = await GetCurrentUserAsync();
            var nowPoint = currentUser.Jf;

            await _userPointRepository.InsertAsync(new UserPointLog()
            {
                UserId = AbpSession.UserId!.Value,
                EventType = EnumClass.UserPointEventType.Activity,
                EventId = log.Id.ToString(),
                BeforePoints = nowPoint,
                AfterPoints = nowPoint + point,
                Desc = activity.Title
            });

            currentUser.Jf += point;
            log.State = EnumClass.PointActivityUserState.已发放;

            await CurrentUnitOfWork.SaveChangesAsync();

            try
            {
                if (shareFrom > 0)
                {
                    var key = $"pointActivity:{activity.Id}:{DateTime.Today:d}";

                    var shared = await _redisClient.Database.HashGetAsync(key, shareFrom);

                    if (!shared.HasValue)
                    {
                        var openid = await _mediator.Send(new UserLoginKeyQuery(shareFrom, TTWorkConsts.LoginProvider.WeChatMiniProgram));
                        if (!openid.IsNullOrEmptyOrWhiteSpace())
                        {
                            await _mediator.Publish(new MessageSendCommand(MessageType.WechatTemplate, new SendWechatTemplateDetail(
                                ProjectApp.SZMLSJ_MINI,
                                new[] { openid },
                                "dwNvsUYLNMaQLoTVPedPtngxzztKG6GmmVBXBvE5zZc",
                                new
                                {
                                    thing2 = new { value = "分享成功" }, //通知内容
                                    time3 = new { value = $"{DateTime.Now:d}" }, //通知时间
                                    thing4 = new { value = $"点击消息进入活动" }, //提示说明
                                }, $"pages/activity/pointActivity?id={activity.Id}"
                            )));

                            await _redisClient.Database.HashSetAsync(key, shareFrom, $"{DateTime.Now}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // ignored
            }


            return ObjectMapper.Map<PointActivityUserLogDto>(log);
        }


        public override async Task DeleteAsync(EntityDto<long> input)
        {
            CheckDeletePermission();

            var find = await Repository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.Id);

            if (DateTime.Now > find.DatetimeStart)
            {
                throw new UserFriendlyException("活动已开始,不能删除");
            }

            await Repository.DeleteAsync(input.Id);
        }
    }
}