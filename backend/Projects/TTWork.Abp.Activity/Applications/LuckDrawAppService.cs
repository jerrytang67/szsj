using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using TT.Extensions;
using TT.Extensions.Redis;
using TTWork.Abp.Activity.Definitions;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.Activity.Events;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.Core.Extensions;
using TtWork.Lib.Extensions;
using static System.Double;

namespace TTWork.Abp.Activity.Applications
{
    public class LuckDrawAppService : AbpAsyncCrudAppService<LuckDraw, LuckDrawDto, long, AppResultRequestDto, LuckDrawCreateOrUpdateDto, LuckDrawCreateOrUpdateDto>
    {
        private readonly IRepository<UserPrize, long> _userPrizeRepository;
        private readonly IRepository<UserLuckTime, long> _userLuckTimeRepository;
        private readonly IRedisClient _redisClient;
        private readonly IConfiguration _appConfiguration;

        public LuckDrawAppService(
            IRepository<LuckDraw, long> repository,
            IRepository<UserPrize, long> userPrizeRepository,
            IRepository<UserLuckTime, long> userLuckTimeRepository,
            IRedisClient redisClient,
            IocManager iocManager,
            IConfiguration appConfiguration) :
            base(repository, iocManager)
        {
            _userPrizeRepository = userPrizeRepository;
            _userLuckTimeRepository = userLuckTimeRepository;
            _redisClient = redisClient;
            _appConfiguration = appConfiguration;

            EnableGetEdit = true;
            base.GetAllPermissionName = ActivityPermissions.Default;
            base.CreatePermissionName = ActivityPermissions.Admin;
            base.UpdatePermissionName = ActivityPermissions.Admin;
            base.DeletePermissionName = ActivityPermissions.Admin;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<UserPrizeDto> LuckDraw(LuckDrawRequestDto input)
        {
            var appName = _appConfiguration["ApplicationName"] ?? "UnKnowApplicationName";

            var activity = await GetEntityByIdAsync(input.Id);
            
            long shareFrom;
            long.TryParse(input.ShareFrom, out shareFrom);
            if (shareFrom > 0 && AbpSession.UserId!.Value == shareFrom)
            {
                shareFrom = 0;
            }

            if (activity == null)
                throw new UserFriendlyException(L("notfind"));

            if (activity.PrizeCount <= 0)
                throw new UserFriendlyException("活动已结束");

            if (activity.State == 0)
                throw new UserFriendlyException("活动已关闭");

            if (DateTime.Now > activity.DatetimeEnd)
                throw new UserFriendlyException("活动已结束");

            if (DateTime.Now < activity.DatetimeStart)
                throw new UserFriendlyException("活动未开始");

            var currentUser = await GetCurrentUserAsync();


            #region 设置的中奖机率

            var keyy = $"{appName}:LuckDraw:{input.Id}:中奖几率";
            var 中奖几率Cache = await _redisClient.Database.StringGetAsync(keyy);
            if (!中奖几率Cache.HasValue)
            {
                中奖几率Cache = activity.DefaultWinningChance ?? 0.1;
                await _redisClient.Database.StringSetAsync(keyy, 中奖几率Cache);
            }

            var 中奖几率 = 0.3;
            TryParse(s: 中奖几率Cache, out 中奖几率);

            #endregion

            var 当前用户中奖次数 = await _userPrizeRepository.GetAll().AsNoTracking().CountAsync(x =>
                x.LuckDrawId == activity.Id
                && x.CreatorUserId == AbpSession.UserId.Value);

            var 随机数最大值 = activity.PrizeTotal / 中奖几率 * Math.Pow(10, 当前用户中奖次数);

            var 随机数 = new Random().Next(1, Convert.ToInt32(随机数最大值));

            if (activity.MaxWinTimes.HasValue && 当前用户中奖次数 >= activity.MaxWinTimes) //达到最大中奖次数,不能再中了
                随机数 = int.MaxValue;

            UserLuckTime userLuckTime = null;

            if (activity.Type == EnumClass.LuckDrawType.Points)
            {
                if (currentUser.Jf < activity.Price)
                    throw new UserFriendlyException("积分不足");

                await EventBus.Default.TriggerAsync(
                    new UserPointLogEvent(
                        currentUser.Id,
                        currentUser.Jf,
                        currentUser.Jf - activity.Price!.Value,
                        EnumClass.UserPointEventType.Activity,
                        input.Id.ToString(),
                        activity.Title));
                currentUser.Jf -= activity.Price!.Value;
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            else if (activity.Type == EnumClass.LuckDrawType.UserLuckyTimes)
            {
                #region 分享加次数

                if (shareFrom > 0 && !await _userLuckTimeRepository.GetAll().AsNoTracking()
                        .AnyAsync(x => x.UserId == shareFrom
                                       && x.LuckDrawId == activity.Id
                                       && x.ShareFrom.HasValue
                                       && x.CreationTime >= DateTime.Today
                        ))
                {
                    await _userLuckTimeRepository.InsertAsync(new UserLuckTime()
                    {
                        UserId = shareFrom, LuckDrawId = activity.Id,
                        CreationTime = DateTime.Now,
                        CreatorUserId = shareFrom,
                        ShareFrom = AbpSession.UserId
                    });
                    await CurrentUnitOfWork.SaveChangesAsync();
                }

                #endregion

                var times = await _userLuckTimeRepository.GetAll().AsNoTracking()
                    .Where(x => x.UserId == AbpSession.UserId.Value
                                && x.LuckDrawId == activity.Id
                                && x.Status == EnumClass.UserLuckTimeStatus.未使用).CountAsync();

                if (times <= 0)
                    throw new UserFriendlyException("没有抽奖次数");

                userLuckTime = await _userLuckTimeRepository.GetAll()
                    .Where(x => x.CreatorUserId == AbpSession.UserId.Value &&
                                x.Status == EnumClass.UserLuckTimeStatus.未使用).FirstOrDefaultAsync();

                userLuckTime.Status = EnumClass.UserLuckTimeStatus.已使用;
                userLuckTime.LuckDrawTime = DateTime.Now;
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            //未中奖,返回空 (这里的原理是随机数小于奖品总数就没有中奖)
            if (随机数 > activity.PrizeTotal)
                return null;

            var PRIZEKEY = $"{appName}:LuckDraw:{activity.Id}:Prizes";
            var prizeCache = await _redisClient.Database.ListRightPopAsync(PRIZEKEY);
            if (!prizeCache.HasValue)
            {
                var prizeList = new List<LuckDrawPrizeDto>();
                //打平奖品数组
                foreach (var priz in activity.LuckDrawPrizes)
                {
                    for (var i = 0; i < priz.StockCount; i++)
                    {
                        // 这里一定要注意改了DTO要给这里赋值
                        prizeList.Add(new LuckDrawPrizeDto()
                        {
                            ImageUrl = priz.ImageUrl,
                            Name = priz.Name,
                            LuckDrawId = activity.Id,
                            Id = priz.Id,
                            PickupWay = priz.PickupWay
                        });
                    }
                }

                //2次随机排序数组,更分散
                prizeList = prizeList.RandomSortList().RandomSortList();

                foreach (var item in prizeList)
                    await _redisClient.Database.ListRightPushAsync(PRIZEKEY, JsonConvert.SerializeObject(item), flags: CommandFlags.FireAndForget);

                prizeCache = await _redisClient.Database.ListRightPopAsync(PRIZEKEY);
            }

            var 中奖奖品 = JsonConvert.DeserializeObject<LuckDrawPrizeDto>(prizeCache);

            if (中奖奖品 == null)
                throw new UserFriendlyException("出错了");

            var prize = new UserPrize()
            {
                ImageUrl = 中奖奖品.ImageUrl,
                Name = 中奖奖品.Name,
                PrizeId = 中奖奖品.Id,
                LuckDrawId = 中奖奖品.LuckDrawId,
                PickupWay = 中奖奖品.PickupWay ?? activity.PickupWay,
                PhoneNumber = currentUser.PhoneNumber,
                ExpiredTime = activity.PrizeExpiredTime,
                State = 0,
                Count = 1
            };

            await _userPrizeRepository.InsertAsync(prize);
            await CurrentUnitOfWork.SaveChangesAsync();

            activity.LuckDrawPrizes.FirstOrDefault(x => x.Id == 中奖奖品.Id)!.StockCount--;
            activity.PrizeCount = Convert.ToInt32(await _redisClient.Database.ListLengthAsync(PRIZEKEY));

            if (userLuckTime is { Status: EnumClass.UserLuckTimeStatus.已使用 })
                userLuckTime.UserPrizeId = prize.Id;

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<UserPrizeDto>(prize);
        }


        public override async Task<LuckDrawDto> GetAsync(EntityDto<long> input)
        {
            var result = await base.GetAsync(input);

            if (AbpSession.UserId.HasValue && result.Type == EnumClass.LuckDrawType.UserLuckyTimes && DateTime.Now >= result.DatetimeStart.Date && DateTime.Now <= result.DatetimeEnd)
            {
                // 如果当天没有送过一次抽奖机会
                if (!await _userLuckTimeRepository.GetAll().AsNoTracking().AnyAsync(x => x.UserId == AbpSession.UserId.Value
                                                                                         && x.LuckDrawId == result.Id
                                                                                         && x.ShareFrom == null
                                                                                         && x.CreationTime > DateTime.Today)
                   )
                {
                    await _userLuckTimeRepository.InsertAsync(new UserLuckTime
                    {
                        UserId = AbpSession.UserId.Value,
                        LuckDrawId = result.Id
                    });
                    await CurrentUnitOfWork.SaveChangesAsync();
                }

                var times = await _userLuckTimeRepository.GetAll().AsNoTracking()
                    .Where(x => x.CreatorUserId == AbpSession.UserId.Value
                                && x.LuckDrawId == result.Id
                                && x.Status == EnumClass.UserLuckTimeStatus.未使用).CountAsync();

                result.LuckTimes = times;
            }


            return result;
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

        protected override Task<LuckDraw> GetEntityByIdAsync(long id)
        {
            return Repository.GetAll().Include(x => x.LuckDrawPrizes).FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<PagedResultDto<LuckDrawBaseDto>> GetAllPublicAsync(AppResultRequestDto input)
        {
            input.Status = 1;
            var query = CreateFilteredQuery(input);
            int totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var pagedResultDto = await query.ToListAsync();

            return new PagedResultDto<LuckDrawBaseDto>(totalCount, ObjectMapper.Map<List<LuckDrawBaseDto>>(pagedResultDto));
        }

        public override async Task<GetForEditOutput<LuckDrawCreateOrUpdateDto>> GetForEdit(EntityDto<long> input)
        {
            var result = await base.GetForEdit(input);

            result.Schema["Type"] = typeof(EnumClass.LuckDrawType).GetEnumSelection();
            result.Schema["PickupWay"] = typeof(EnumClass.PickupWay).GetEnumSelection();
            return result;
        }


        protected override IQueryable<LuckDraw> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                    .Include(x => x.LuckDrawPrizes)
                    .WhereIf(input.Status.HasValue, x => x.State == input.Status)
                    .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), x => x.Title.Contains(input.Keyword))
                ;
        }
    }
}