using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Json;
using Abp.Linq.Extensions;
using Abp.UI;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using TT.Extensions;
using TT.Extensions.Redis;
using TTWork.Abp.Activity.Definitions;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.AppManagement.Events;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Applications.Wechat;
using TTWork.Abp.Core.Definitions;

namespace TTWork.Abp.Activity.Applications
{
    public class UserPrizeAppService : AbpAsyncCrudAppService<UserPrize, UserPrizeDto, long, AppResultRequestDto, UserPrizeDto, UserPrizeDto>
    {
        private readonly IRepository<LuckDraw, long> _luckDrawRepository;
        private readonly IRedisClient _redisClient;
        private readonly WeixinManger _weixinManger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IMediator _mediator;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<UserPrize, long> _userPrizeRepository;

        public UserPrizeAppService(
            IRepository<UserPrize, long> repository,
            IRepository<LuckDraw, long> luckDrawRepository,
            IocManager iocManager,
            IRedisClient redisClient,
            WeixinManger weixinManger,
            IUnitOfWorkManager unitOfWorkManager,
            IMediator mediator, IHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor, IRepository<UserPrize, long> userPrizeRepository) : base(repository, iocManager)
        {
            _luckDrawRepository = luckDrawRepository;
            _redisClient = redisClient;
            _weixinManger = weixinManger;
            _unitOfWorkManager = unitOfWorkManager;
            _mediator = mediator;
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _userPrizeRepository = userPrizeRepository;
        }

        protected override IQueryable<UserPrize> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                .WhereIf(input.Pid.HasValue, x => x.LuckDrawId == input.Pid.Value)
                .WhereIf(input.UserId.HasValue, x => x.CreatorUserId == input.UserId.Value)
                .WhereIf(input.Status is 0, x => x.State == 0)
                .WhereIf(input.Status is 1, x => x.State == EnumClass.UserPrizeState.已领取)
                .WhereIf(input.Status is -1, x => x.State == EnumClass.UserPrizeState.过期)
                .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), x => x.PhoneNumber == input.Keyword
                                                                          || x.CheckPhoneNumber == input.Keyword
                                                                          || x.CheckCode == input.Keyword)
                .OrderByDescending(x => x.Id);
        }


        public const string TESTKEY = "UserPrize:Test";

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<object> Check(UserPrizeCheckInputDto input)
        {
            #region 测试码

            if (input.Id == -1)
            {
                await _redisClient.Database.StringSetAsync(TESTKEY, "1", TimeSpan.FromSeconds(10));
                return true;
            }

            #endregion

            var userPrize = await Repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (userPrize == null)
                throw new UserFriendlyException("找不到这条中奖记录{input.Id}");

            if (userPrize.State != 0)
                throw new UserFriendlyException("奖品已领取或已过期");

            if (userPrize.ExpiredTime.HasValue && userPrize.ExpiredTime < DateTime.Now && userPrize.State == EnumClass.UserPrizeState.待领取)
            {
                using var unitOfWork = _unitOfWorkManager.Begin(new UnitOfWorkOptions() { });
                userPrize.State = EnumClass.UserPrizeState.过期;
                await CurrentUnitOfWork.SaveChangesAsync();
                await unitOfWork.CompleteAsync();

                throw new UserFriendlyException("奖品已过期");
            }

            var luckDraw = await _luckDrawRepository.GetAll().AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == userPrize.LuckDrawId);

            if (luckDraw == null)
                throw new UserFriendlyException($"找不到活动{userPrize.LuckDrawId}");

            var codes = luckDraw.CheckCodes.FromJsonString<List<string>>();

            if (codes == null || codes.IndexOf(input.Code) <= -1) throw new UserFriendlyException("门店码错误");

            userPrize.State = EnumClass.UserPrizeState.已领取;
            userPrize.CheckCode = input.Code;
            userPrize.CheckTime = DateTime.Now;
            userPrize.CheckUserId = AbpSession.UserId!.Value;
            userPrize.CheckPhoneNumber = (await GetCurrentUserAsync()).PhoneNumber;
            return true;
        }

        protected override async Task<UserPrize> GetEntityByIdAsync(long id)
        {
            #region 测试码

            if (id == -1)
            {
                var testCache = await _redisClient.Database.StringGetAsync(TESTKEY);
                return new UserPrize().GetTestEntity(testCache.HasValue);
            }

            #endregion

            return await base.GetEntityByIdAsync(id);
        }


        public override async Task<PagedResultDto<UserPrizeDto>> GetAllAsync(AppResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            if (!await IsAdminAsync("UnionAdmin"))
            {
                foreach (var dto in result.Items)
                {
                    if (!dto.PhoneNumber.IsNullOrEmptyOrWhiteSpace())
                        dto.PhoneNumber = Regex.Replace(dto.PhoneNumber, "(\\d{3})\\d{5}(\\d{3})", "$1*****$2");
                }
            }

            if (await IsAdminAsync("UnionAdmin"))
            {
                var cache = new Dictionary<long, string>();

                foreach (var dto in result.Items)
                {
                    if (!cache.ContainsKey(dto.LuckDrawId))
                    {
                        var luckDraw = await _luckDrawRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == dto.LuckDrawId);
                        if (luckDraw != null)
                        {
                            cache.Add(dto.LuckDrawId, luckDraw.Title);
                        }
                    }

                    dto.LuckDrawTitle = cache[dto.LuckDrawId];
                }
            }

            return result;
        }

        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<PagedResultDto<UserPrizeDto>> GetAllMyAsync(AppResultRequestDto input)
        {
            input.UserId = AbpSession.UserId!.Value;
            CheckGetAllPermission();
            var query = CreateFilteredQuery(input);
            int totalCount = await query.CountAsync();
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var list = await query.ToListAsync();

            var expiredCount = 0;
            foreach (var item in list)
            {
                if (item.ExpiredTime < DateTime.Now && item.State == 0)
                {
                    item.State = EnumClass.UserPrizeState.过期;
                    expiredCount++;
                }
            }

            if (expiredCount > 0)
                await CurrentUnitOfWork.SaveChangesAsync();

            list = list.WhereIf(input.Status.HasValue, x => (int)x.State == input.Status).ToList();

            var pagedResultDto = new PagedResultDto<UserPrizeDto>(totalCount, ObjectMapper.Map<List<UserPrizeDto>>(list));


            return pagedResultDto;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<UserPrizeDto> SetExpress(UserPrizeExpressInput input)
        {
            var find = await Repository.FirstOrDefaultAsync(x => x.Id == input.Id
                                                                 && x.CreatorUserId == AbpSession.UserId.Value
            );

            if (find == null)
                throw new UserFriendlyException($"找不到记录");

            find.Address = input.Address;
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<UserPrizeDto>(find);
        }


        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<string> GetCheckQr(EntityDto<long> inputDto)
        {
            var userId = AbpSession.UserId!.Value;
            var find = await Repository.FirstOrDefaultAsync(x => x.Id == inputDto.Id
                                                                 && x.CreatorUserId == userId
            );

            if (find == null)
                throw new UserFriendlyException($"找不到记录");

            switch (find.State)
            {
                case 0:
                {
                    var app = await _mediator.Send(new QueryApp());
                    var appid = app.GetValue("appid");
                    var appSec = app.GetValue("appsec");
                    var qrStr = await _weixinManger.getwxacodeunlimit(appid, appSec, $"prizeCheck@{inputDto.Id}@{AbpSession.UserId}", "pages/activity/check", 430, false);

                    find.QrUrl = qrStr;
                    await CurrentUnitOfWork.SaveChangesAsync();

                    return qrStr;
                }
                default:
                    throw new UserFriendlyException($"未知错误");
            }
        }


        [AbpAuthorize]
        public async Task<string> ExportExcel(AppResultRequestDto input)
        {
            CheckGetAllPermission();
            input.MaxResultCount = int.MaxValue;

            var result = await GetAllAsync(input);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var sWebRootFolder = _hostEnvironment.ContentRootPath + "/wwwroot";
            var sFileName = @$"用户中奖记录导出_{DateTime.Now:yyMMdd_HHmmss}.xlsx";
            var URL = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext!.Request.Host}/{sFileName}";
            var file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }

            using var package = new ExcelPackage(file);
            // add a new worksheet to the empty workbook
            var ws = package.Workbook.Worksheets.Add("用户中奖记录");

            var titleArr = new[]
            {
                "ID", "活动名称", "奖品名称", "奖品数量", "中奖时间", "中奖手机", "状态", "领奖时间", "门店核销码", "核销人手机",
                "省", "市", "镇/区", "地址", "收货人姓名", "收货人手机", "快递公司", "快递单号"
            };
            int row = 1;

            ws.Cells["A1"].Value = "用户中奖记录导出";
            ws.Cells[$"A1:{(char)('A' + titleArr.Length - 1)}1"].Merge = true;
            ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(1).Style.Font.Size = 24;
            // ws.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.Red);
            //First add the headers
            row++;

            for (int i = 0; i < titleArr.Length; i++)
            {
                ws.Cells[2, i + 1].Value = titleArr[i];
            }

            row++;

            for (int i = 0; i < result.Items.Count; i++)
            {
                var j = 1;
                ws.Cells[row, j++].Value = result.Items[i].Id;
                ws.Cells[row, j++].Value = result.Items[i].LuckDrawTitle;
                ws.Cells[row, j++].Value = result.Items[i].Name;
                ws.Cells[row, j++].Value = result.Items[i].Count;
                ws.Cells[row, j++].Value = $"{result.Items[i].CreationTime:g}";
                ws.Cells[row, j++].Value = result.Items[i].PhoneNumber;
                ws.Cells[row, j++].Value = result.Items[i].State == 0 ? "未领取" : "已领取";
                ws.Cells[row, j++].Value = $"{result.Items[i].CheckTime:g}";
                ws.Cells[row, j++].Value = $"{result.Items[i].CheckCode}";
                ws.Cells[row, j++].Value = $"{result.Items[i].CheckPhoneNumber}";
                if (result.Items[i].Address != null)
                {
                    ws.Cells[row, j++].Value = $"{result.Items[i].Address?.ProvinceName}";
                    ws.Cells[row, j++].Value = $"{result.Items[i].Address?.CityName}";
                    ws.Cells[row, j++].Value = $"{result.Items[i].Address?.CountyName}";
                    ws.Cells[row, j++].Value = $"{result.Items[i].Address?.DetailInfo}";
                    ws.Cells[row, j++].Value = $"{result.Items[i].Address?.UserName}";
                    ws.Cells[row, j++].Value = $"{result.Items[i].Address?.TelNumber}";
                }

                row++;
            }

            await package.SaveAsync(); //Save the workbook.
            return URL;
        }


        [Obsolete]
        public override Task<UserPrizeDto> CreateAsync(UserPrizeDto input)
        {
            throw new Exception("notuse");
        }

        [Obsolete]
        public override Task<UserPrizeDto> UpdateAsync(UserPrizeDto input)
        {
            throw new Exception("notuse");
        }

        [Obsolete]
        public override Task DeleteAsync(EntityDto<long> input)
        {
            throw new Exception("notuse");
        }
    }

    public class UserPrizeCheckInputDto
    {
        public long Id { get; set; }

        public string Code { get; set; }
    }

    public class UserPrizeExpressInput
    {
        public long Id { get; set; }
        public AddressDetail Address { get; set; }
    }
}