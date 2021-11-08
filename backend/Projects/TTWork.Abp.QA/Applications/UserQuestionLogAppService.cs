using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Json;
using Abp.Linq.Extensions;
using Abp.UI;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT.Extensions;
using TT.Extensions.Redis;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.Core.Events.Queries;
using TTWork.Abp.LaborUnion.Definitions;
using TTWork.Abp.LaborUnion.Events.Commands;
using TTWork.Abp.QA.Applications.Dtos;
using TTWork.Abp.QA.Definitions;
using TTWork.Abp.QA.Domains;
using TtWork.Lib.Extensions;

namespace TTWork.Abp.QA.Applications
{
    public class UserQuestionLogAppService : AbpAppServiceBase
    {
        private readonly IRepository<QAPlan, long> _planRepository;
        private readonly IRepository<QAQuestion, Guid> _questionRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserQuestionLog, long> _repository;
        private readonly IRepository<UserPointLog, long> _userPointRepository;
        private readonly IRepository<UserPointLog, long> _userpontLogRepo;
        private readonly IRepository<UserLuckTime, long> _userLuckTimeReo;
        private readonly IRepository<UserPrize, long> _userPrizeRepo;
        private readonly IRepository<LuckDrawPrize, long> _luckDrawPrizeRepo;
        private readonly IRepository<LuckDraw, long> _luckDrawRepo;
        private readonly IRedisClient _redisClient;
        private readonly IMediator _mediator;

        public UserQuestionLogAppService(
            IRepository<QAPlan, long> planRepository,
            IRepository<QAQuestion, Guid> questionRepository,
            IRepository<User, long> userRepository,
            IRepository<UserQuestionLog, long> repository,
            IRepository<UserPointLog, long> userPointRepository,
            IRepository<UserPointLog, long> userpontLogRepo,
            IRepository<UserLuckTime, long> userLuckTimeReo,
            IRepository<UserPrize, long> userPrizeRepo,
            IRepository<LuckDrawPrize, long> luckDrawPrizeRepo,
            IRedisClient redisClient,
            IMediator mediator, IRepository<LuckDraw, long> luckDrawRepo)
        {
            _planRepository = planRepository;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _repository = repository;
            _userPointRepository = userPointRepository;
            _userpontLogRepo = userpontLogRepo;
            _userLuckTimeReo = userLuckTimeReo;
            _userPrizeRepo = userPrizeRepo;
            _luckDrawPrizeRepo = luckDrawPrizeRepo;
            _redisClient = redisClient;
            _mediator = mediator;
            _luckDrawRepo = luckDrawRepo;
        }


        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<UserQuestionLogDto> PostUserQuestion(UserQuestionRequest input)
        {
            var log = await _repository.FirstOrDefaultAsync(x => x.Id >= input.Id
                                                                 && x.CreatorUserId == AbpSession.UserId!.Value);

            log.QuestionItems[input.QuestionIndex].UserSelectIndex ??= input.AnswerIndex;
            log.QuestionItems[input.QuestionIndex].FinishTime = DateTime.Now;

            log.RightCount = log.QuestionItems.Count(x => x.UserSelectIndex == x.AnswerIndex);

            if (log.QuestionItems.All(x => x.UserSelectIndex != null))
            {
                log.State = UserQuestionLogEnum.完成答题;

                log.SpendTime = Convert.ToInt32((DateTime.Now - log.CreationTime).TotalSeconds);

                #region 给分享加积分

                if (log.ShareFrom.HasValue && log.ShareFrom > 0)
                {
                    try
                    {
                        var key = $"WJZGH:QA:QAPlan:{log.PlanId}:{DateTime.Today:d}";

                        var shared = await _redisClient.Database.HashGetAsync(key, log.ShareFrom.Value);

                        if (!shared.HasValue) //如果已分享加积分
                        {
                            var openid = await _mediator.Send(new UserLoginKeyQuery(log.ShareFrom.Value, TTWorkConsts.LoginProvider.WeChatMiniProgram));
                            if (!openid.IsNullOrEmptyOrWhiteSpace())
                            {
                                await _mediator.Publish(new MessageSendCommand(MessageType.WechatTemplate, new SendWechatTemplateDetail(
                                    ProjectApp.ZGH_MINI,
                                    new[] { openid },
                                    "dwNvsUYLNMaQLoTVPedPtngxzztKG6GmmVBXBvE5zZc",
                                    new
                                    {
                                        thing2 = new { value = "分享成功" }, //通知内容
                                        time3 = new { value = $"{DateTime.Now:d}" }, //通知时间
                                        thing4 = new { value = $"点击消息进入小程序" }, //提示说明
                                    }, $"pages/qa/index?id={log.PlanId}"
                                )));
                                await _redisClient.Database.HashSetAsync(key, log.ShareFrom.Value, $"{DateTime.Now}");

                                #region 发积分

                                var shareUser = await _mediator.Send(new UserQuery(log.ShareFrom.Value));
                                if (shareUser != null)
                                {
                                    var plan = await _planRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == log.PlanId);
                                    if (plan != null && plan.SharePoints > 0)
                                    {
                                        var nowPoint = shareUser.Jf;

                                        await _userPointRepository.InsertAsync(new UserPointLog()
                                        {
                                            UserId = shareUser.Id,
                                            EventType = Activity.EnumClass.UserPointEventType.QA,
                                            EventId = log.Id.ToString(),
                                            BeforePoints = nowPoint,
                                            AfterPoints = nowPoint + plan.SharePoints,
                                            Desc = $"分享 {plan.Title}"
                                        });
                                        shareUser.Jf += plan.SharePoints;

                                        await CurrentUnitOfWork.SaveChangesAsync();
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }

                #endregion
            }

            await _repository.UpdateAsync(log);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<UserQuestionLogDto>(log);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<UserQuestionLogDto> GetAsync(EntityDto<long> input)
        {
            var log = await _repository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.Id);
            return ObjectMapper.Map<UserQuestionLogDto>(log);
        }


        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<UserQuestionLogDto> GetUserQuestion(GetUserQuestionDto input)
        {
            long shareFrom;

            long.TryParse(input.ShareFrom, out shareFrom);

            var plan = await _planRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.Id);
            if (plan == null)
                throw new UserFriendlyException("找不到这个活动");

            if (DateTime.Now < plan.DatetimeStart)
                throw new UserFriendlyException("答题时间未开始");

            if (plan.DatetimeEnd < DateTime.Now)
                throw new UserFriendlyException("答题时间已结束");

            var 当天未完成答题log = await _repository.GetAll().OrderByDescending(x => x.CreationTime)
                .FirstOrDefaultAsync(x => x.CreationTime >= DateTime.Today
                                          && x.State == UserQuestionLogEnum.未完成
                                          && x.CreatorUserId == AbpSession.UserId!.Value
                                          && x.PlanId == input.Id);

            var 当天答题Logs = await _repository.GetAll().AsNoTracking().OrderByDescending(x => x.CreationTime)
                .Where(x => x.CreationTime >= DateTime.Today
                            && x.CreatorUserId == AbpSession.UserId!.Value
                            && x.PlanId == input.Id).ToListAsync();

            var 当天转发次数 = await _repository.GetAll().AsNoTracking()
                .Where(x => x.CreationTime >= DateTime.Today
                            && x.ShareFrom == AbpSession.UserId!.Value
                            && x.PlanId == input.Id).CountAsync();


            if (shareFrom > 0 && AbpSession.UserId!.Value == shareFrom)
                shareFrom = 0; //转自自己,不记数


            if (当天未完成答题log == null)
            {
                //当天答题次数 = 问题默认次数+转发奖励
                //TODO:分享不奖励的怎么弄
                if (当天答题Logs.Count >= (plan.AnswerPerDay + (当天转发次数 > 0 ? plan.HelpPerDay : 0)))
                    throw new UserFriendlyException("今天答题次数已用完");

                var questions = await _questionRepository.GetAll().AsNoTracking()
                    .Where(x => x.PlanId == input.Id && x.State == EnumClass.QAQuestionState.开启).ToListAsync();
                questions = questions.RandomSortList().RandomSortList().Take(plan.QuestionCount).ToList();

                var questionItems = questions.Select(q => new QuestionItem
                {
                    Id = q.Id,
                    Title = q.Title,
                    AnswerIndex = q.AnswerIndex,
                    AnswerList = q.AnswerList
                }).ToList();

                当天未完成答题log = new UserQuestionLog
                {
                    PlanId = input.Id,
                    State = UserQuestionLogEnum.未完成,
                    QuestionItems = questionItems,
                    ShareFrom = shareFrom == 0 ? null : shareFrom
                };
                await _repository.InsertAsync(当天未完成答题log);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return ObjectMapper.Map<UserQuestionLogDto>(当天未完成答题log);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<UserQuestionLogDto> GetPoints(EntityDto<long> input)
        {
            var log = await _repository.FirstOrDefaultAsync(x => x.Id >= input.Id
                                                                 && x.CreatorUserId == AbpSession.UserId!.Value);

            if (log.State == UserQuestionLogEnum.未完成 || log.QuestionItems.Any(x => x.UserSelectIndex == null))
            {
                throw new UserFriendlyException("未完成所有题目");
            }

            if (log.State == UserQuestionLogEnum.已领奖)
            {
                throw new UserFriendlyException("已领奖");
            }

            var plan = await _planRepository.GetAll().AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == log.PlanId);

            if (log.Points == null)
            {
                var points = (from rule in plan.PointRules.OrderByDescending(x => x.Points) where log.RightCount >= rule.Count select rule.Points).FirstOrDefault();
                var luckTimes = (from rule in plan.PointRules.OrderByDescending(x => x.LuckTime) where log.RightCount >= rule.Count select rule.LuckTime).FirstOrDefault();
                log.Points = points;
                log.State = UserQuestionLogEnum.已领奖;

                #region

                //加积分
                if (points is > 0)
                {
                    var currentUser = await GetCurrentUserAsync();
                    var nowPoint = currentUser.Jf;
                    await _userPointRepository.InsertAsync(new UserPointLog()
                    {
                        UserId = AbpSession.UserId!.Value,
                        EventType = Activity.EnumClass.UserPointEventType.QA,
                        EventId = log.Id.ToString(),
                        BeforePoints = nowPoint,
                        AfterPoints = nowPoint + points.Value,
                        Desc = plan.Title
                    });
                    currentUser.Jf += points.Value;
                }

                if (luckTimes is > 0 && plan.LuckDrawId.HasValue)
                {
                    var luckTime = new UserLuckTime()
                    {
                        Host = nameof(QAPlan),
                        HostId = $"{plan.Id}",
                        UserId = AbpSession.UserId!.Value,
                        LuckDrawId = plan.LuckDrawId!.Value,
                        Status = Activity.EnumClass.UserLuckTimeStatus.未使用
                    };
                    var userLuckTimeId = await _userLuckTimeReo.InsertOrUpdateAndGetIdAsync(luckTime);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    log.UserLuckTimeId = userLuckTimeId;
                }

                #endregion


                await CurrentUnitOfWork.SaveChangesAsync();
            }


            #region 自动发券

            try
            {
                if (plan.Id == 9) //国庆活动
                {
                    var 用户有没有拿过20券 = await _userPrizeRepo.FirstOrDefaultAsync(x => x.PrizeId == 21 && x.CreatorUserId == AbpSession.UserId.Value);
                    if (用户有没有拿过20券 == null)
                    {
                        var activity = await _luckDrawRepo.FirstOrDefaultAsync(x => x.Id == 4);
                        var 券 = await _luckDrawPrizeRepo.FirstOrDefaultAsync(x => x.Id == 21);
                        var currentUser = await GetCurrentUserAsync();
                        if (券 != null && 券.StockCount > 0)
                        {
                            await _userPrizeRepo.InsertAsync(new UserPrize()
                            {
                                LuckDrawId = 4,
                                PrizeId = 21,
                                PickupWay = Activity.EnumClass.PickupWay.Qr,
                                Name = 券.Name,
                                ImageUrl = 券.ImageUrl,
                                PhoneNumber = currentUser.PhoneNumber,
                                ExpiredTime = activity.PrizeExpiredTime,
                                State = 0,
                                Count = 1
                            });
                            券.StockCount--;
                            await CurrentUnitOfWork.SaveChangesAsync();
                            Logger.Info($"自动发券成功,剩余{券.StockCount}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }

            #endregion


            return ObjectMapper.Map<UserQuestionLogDto>(log);
        }


        [AbpAuthorize(QAPermissions.Default)]
        public async Task<PagedResultDto<UserQuestionLogDto>> GetAllAsync(AppResultRequestDto input)
        {
            var query = _repository.GetAll().AsNoTracking();
            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(z => z.Id).PageBy(input).ToListAsync();

            return new PagedResultDto<UserQuestionLogDto>(totalCount, ObjectMapper.Map<List<UserQuestionLogDto>>(items));
        }

        [HttpGet]
        public async Task<RankListDto> GetRankList(EntityDto<long> input)
        {
            var key = $"WJZGH:QA:RankList:{input.Id}";

            var cache = await _redisClient.Database.StringGetAsync(key);

            if (cache.HasValue)
            {
                return cache.ToString().FromJsonString<RankListDto>();
            }

            var list = await _repository.GetAll().AsNoTracking()
                .Where(x => x.RightCount.HasValue && x.Points.HasValue && x.CreatorUserId.HasValue && x.PlanId == input.Id)
                .OrderBy(x => x.CreationTime)
                .GroupBy(x => x.CreatorUserId).Select(x => new RankListItem()
                {
                    UserId = x.Key.Value,
                    PointCount = x.Sum(c => c.Points.Value),
                    RightCount = x.Sum(c => c.RightCount.Value),
                    SpendTime = x.Sum(c => c.SpendTime.Value)
                })
                .OrderByDescending(x => x.RightCount)
                .ThenByDescending(x => x.PointCount)
                .ThenBy(x => x.SpendTime)
                .Take(20)
                .ToListAsync();

            var shareList = await _repository.GetAll().AsNoTracking().Where(x => x.ShareFrom.HasValue)
                    .GroupBy(x => x.ShareFrom)
                    .Select(x => new RankShareListItem()
                    {
                        UserId = x.Key.Value,
                        Count = x.Count()
                    })
                    .OrderByDescending(x => x.Count)
                    .Take(20)
                    .ToListAsync()
                ;

            var total = await _repository.GetAll().AsNoTracking().Where(x => x.PlanId == input.Id)
                // .GroupBy(x => x.CreatorUserId)
                .CountAsync();

            int j = 1;

            var i1 = "https://img.wujiangapp.com/wjzgh/2021-05-24/upload_zb6zzmv9l3iv8dnfdxqc8gtf424g2o8w.png";
            var i2 = "https://img.wujiangapp.com/wjzgh/2021-05-24/upload_9wa8hfwwlbqb9erwbl7vs8d77dmjx4ym.png";
            var i3 = "https://img.wujiangapp.com/wjzgh/2021-05-24/upload_fw9f0tdf8sd0lz4b3xdn7tj4b6njv1fn.png";

            foreach (var i in list)
            {
                var user = await _mediator.Send(new UserQuery(i.UserId));
                if (user != null)
                {
                    i.PhoneNumber = user.PhoneNumber.MaskPhoneNumber();
                    i.Town = user.Town;
                    i.Name = user.Surname;
                }

                i.ImageUrl = j++ switch
                {
                    1 => i1,
                    2 => i2,
                    3 => i3,
                    _ => ""
                };
            }

            j = 1;

            foreach (var i in shareList)
            {
                var user = await _mediator.Send(new UserQuery(i.UserId));
                if (user != null)
                {
                    i.PhoneNumber = user.PhoneNumber.MaskPhoneNumber();
                    i.Town = user.Town;
                    i.Name = user.Surname;
                }

                i.ImageUrl = j++ switch
                {
                    1 => i1,
                    2 => i2,
                    3 => i3,
                    _ => ""
                };
            }

            var result = new RankListDto
            {
                CreationTime = DateTime.Now,
                Items = list,
                Total = total,
                ShareItems = shareList
            };

            await _redisClient.Database.StringSetAsync(key, result.ToJsonString(), TimeSpan.FromMinutes(5));

            return result;
        }

        [AbpAuthorize(QAPermissions.Admin)]
        [Obsolete]
        public async Task DeleteAsync(EntityDto<long> input)
        {
            throw new UserFriendlyException("notuse");
            // await _repository.DeleteAsync(input.Id);
        }
    }
}