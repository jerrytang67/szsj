using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using TT.Extensions;
using TT.HttpClient.Weixin;
using TTWork.Abp.AuditManagement.Applications;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.LaborUnion.Applications.Dtos;
using TTWork.Abp.LaborUnion.Definitions;
using TTWork.Abp.LaborUnion.Domains;
using TTWork.Abp.LaborUnion.Events.Commands;

namespace TTWork.Abp.LaborUnion.Applications
{
    public class CraftsmanRecommendAppService : AuditAsyncCrudAppService<CraftsmanRecommend, CraftsmanRecommendDto, long, AppResultRequestDto, CraftsmanRecommendCreateOrUpdateDto,
        CraftsmanRecommendCreateOrUpdateDto>
    {
        private readonly UserManager _userManager;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IPayApi _payApi;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public CraftsmanRecommendAppService(
            IRepository<CraftsmanRecommend, long> repository,
            IocManager iocManager, UserManager userManager,
            IHostEnvironment hostEnvironment,
            IPayApi payApi,
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator
        ) : base(repository, iocManager)
        {
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _payApi = payApi;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
            base.GetPermissionName = AppPermissions.Pages.Default;
            base.CreatePermissionName = AppPermissions.Pages.Default;
            base.UpdatePermissionName = AppPermissions.Pages.Default;

            base.DeletePermissionName = LaborUnionPermissions.Admin;
            base.GetAllPermissionName = LaborUnionPermissions.Default;

            base.AuditName = LaborUnionAudit.CraftsmanRecommend;
            EnableGetEdit = true;
        }

        /// <summary>
        /// 检查是否有同名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task CheckSameName(CraftsmanRecommendCreateOrUpdateDto input)
        {
            if (await Repository.GetAll().AsNoTracking().AnyAsync(x => x.Detail.Realname == input.Detail.Realname &&
                                                                       x.CreatorUserId == AbpSession.UserId.Value &&
                                                                       x.Id != input.Id))
            {
                throw new UserFriendlyException($"你已经推荐了{input.Detail.Realname},请误重复推荐");
            }
        }

        private async Task CheckOneDay()
        {
            const int NUM = 3;
            if (await Repository.GetAll().AsNoTracking().CountAsync(x => x.CreatorUserId == AbpSession.UserId!.Value && x.CreationTime >= DateTime.Today) >= NUM)
            {
                throw new UserFriendlyException($"每人一天最多推荐{NUM}人");
            }
        }

        public override async Task<CraftsmanRecommendDto> GetAsync(EntityDto<long> input)
        {
            CheckGetPermission();
            var entity = await Repository.GetAll().Include(x => x.CreatorUser).AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.Id);

            return MapToEntityDto(entity);
        }

        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<CraftsmanRecommendDto> GetRedpacket(EntityDto<long> input)
        {
            var entity = await Repository.FirstOrDefaultAsync(x => x.Id == input.Id && x.CreatorUserId == AbpSession.UserId.Value);

            if (entity == null)
                throw new UserFriendlyException("找不到这条推荐记录");

            if (entity.State == RecomandState.推荐成功 && entity.RedpacketRecived == false)
            {
                var openid = await _userManager.GetUserLoginKey(entity.CreatorUserId!.Value, ClientTypeEnum.WeixinMini);

                var success = await _payApi.Transfers(
                    "wx3452d3067b75dc88",
                    "1486627732",
                    "9d46c43b90229ff44e9599674ad4d683",
                    entity.SecurityStamp!.Value.ToShortString().Replace("_", "").Replace("-", ""),
                    openid,
                    "",
                    Convert.ToInt32(entity.Redpacket!.Value * 100),
                    $"吴江“红色工匠”奖励红包",
                    false
                );
                if (success)
                {
                    entity.RedpacketRecived = true;
                    entity.RedpacketRecivedTime = DateTime.Now;
                }
                else
                {
                    throw new UserFriendlyException("红包发放失败,请查看微信帐号是否已绑定银行卡并且实名认证");
                }

                return MapToEntityDto(entity);
            }

            throw new UserFriendlyException("红包已领取或推荐未成功");
        }


        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<PagedResultDto<CraftsmanRecommendDto>> GetAllMyAsync(AppResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            query = query.Where(x => x.CreatorUserId == AbpSession.UserId.Value);

            var total = await query.CountAsync();

            query = ApplySorting(query, input);

            query = ApplyPaging(query, input);

            return new PagedResultDto<CraftsmanRecommendDto>(total, ObjectMapper.Map<List<CraftsmanRecommendDto>>(await query.ToListAsync()));
        }

        public override async Task<CraftsmanRecommendDto> CreateAsync(CraftsmanRecommendCreateOrUpdateDto input)
        {
            await CheckSameName(input);

            await CheckOneDay();

            var dto = await base.CreateAsync(input);

            if (dto.AuditFlowId.HasValue)
            {
                await base.StartAudit(dto);
            }

            var data = new SendWechatWorkAppDetail(new MarkdownMessage
                {
                    agentid = 1000004,
                    touser = "TangJiaWei",
                    markdown = new MessageContentWrap
                    {
                        content = $@"有新的`红色工匠`推荐 
> 被推荐人姓名：{dto.Detail.Realname}
> 手机：{dto.Detail.PhoneNumber}
> 性别：{dto.Detail.Sex}
> 年龄：{dto.Detail.Age}
> 政治面貌：{dto.Detail.PoliticsStatus}
> 工作单位：{dto.Detail.WorkUnit}
> 职务：{dto.Detail.WorkTitle}
> 推荐理由：{dto.Detail.Desc}
> 如需查看详细，请点击：[查看详细](https://szsj.wujiangapp.com/admin/index.html#/craftsman/CraftsmanRecommandList)"
                    }
                }
            );
            
            await _mediator.Publish(new MessageSendCommand(MessageType.WechatWorkApp, data));
            
            return dto;
        }

        [AbpAuthorize]
        public override async Task<CraftsmanRecommendDto> UpdateAsync(CraftsmanRecommendCreateOrUpdateDto input)
        {
            var entity = await GetEntityByIdAsync(input.Id);

            if (entity == null)
                throw new UserFriendlyException(L("notfind"));

            await CheckSameName(input);

            if (!await IsInRoleAsync(AbpSession.UserId!.Value, LaborUnionPermissions.Admin))
            {
                if (entity.CreatorUserId != AbpSession.UserId!.Value)
                {
                    throw new UserFriendlyException(L("OnlyCanEditSelfEntity"));
                }
            }
            
            if (entity.IsAudited)
                throw new UserFriendlyException("审核通过的不能再编辑");

            if (!entity.AuditFlowId.HasValue) // 如果编辑的时候没有原来的审核,就拉取
            {
                var auditName = GetAuditName(entity);

                if (!auditName.IsNullOrEmptyOrWhiteSpace())
                {
                    var auditFlowId = await AuditProvider.GetOrNullAsync(auditName);
                    input.AuditFlowId = auditFlowId;
                }
            }

            MapToEntity(input, entity);

            // 如果修改了审核流程,审核将重新开始
            entity.Audit = null;
            entity.AuditStatus = null;
            await CurrentUnitOfWork.SaveChangesAsync();
            await StartAudit(entity);
            return MapToEntityDto(entity);
        }
        
        
        [AbpAuthorize]
        public async Task<string> ExportExcel(AppResultRequestDto input)
        {
            CheckGetAllPermission();
            input.MaxResultCount = int.MaxValue;

            var result = await base.GetAllAsync(input);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var sWebRootFolder = _hostEnvironment.ContentRootPath + "/wwwroot";
            var sFileName = @$"工匠推荐导出_{DateTime.Now:yyMMdd_HHmmss}.xlsx";
            var URL = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext!.Request.Host}/{sFileName}";
            var file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }

            using var package = new ExcelPackage(file);
            // add a new worksheet to the empty workbook
            var ws = package.Workbook.Worksheets.Add("自荐信息");

            var titleArr = new[]
            {
                "ID", "被推荐人姓名", "被推荐人手机", "性别", "政治面貌", "年龄", "所属区域", "工作单位", "职务", "推荐理由", "创建时间", "审核状态", "推荐人手机"
            };


            int row = 1;
            ws.Cells["A1"].Value = "吴江“红色工匠”推荐信息";
            ws.Cells[$"A1:{(char) ('A' + titleArr.Length - 1)}1"].Merge = true;
            ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(1).Style.Font.Size = 24;
            // ws.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.Red);
            row++;
            for (int i = 0; i < titleArr.Length; i++)

                ws.Cells[2, i + 1].Value = titleArr[i];

            row++;

            for (int i = 0; i < result.Items.Count; i++)
            {
                int j = 1;
                ws.Cells[row, j++].Value = result.Items[i].Id;
                ws.Cells[row, j++].Value = result.Items[i].Detail.Realname;
                ws.Cells[row, j++].Value = result.Items[i].Detail.PhoneNumber;
                ws.Cells[row, j++].Value = result.Items[i].Detail.Sex;
                ws.Cells[row, j++].Value = result.Items[i].Detail.PoliticsStatus;
                ws.Cells[row, j++].Value = result.Items[i].Detail.Age;
                ws.Cells[row, j++].Value = result.Items[i].Detail.Address;
                ws.Cells[row, j++].Value = result.Items[i].Detail.WorkUnit;
                ws.Cells[row, j++].Value = result.Items[i].Detail.WorkTitle;
                ws.Cells[row, j++].Value = result.Items[i].Detail.Desc;
                ws.Cells[row, j++].Value = $"{result.Items[i].CreationTime:g}";
                ws.Cells[row, j++].Value = $"{result.Items[i].State.ToString()}";
                ws.Cells[row, j++].Value = result.Items[i].CreatorUser?.PhoneNumber ?? "";

                row++;
            }

            await package.SaveAsync(); //Save the workbook.
            return URL;
        }


        protected override async Task BeforeStartAuditDo(CraftsmanRecommend entity)
        {
            entity.State = RecomandState.审核中;
            await Task.CompletedTask;
        }

        protected override async Task AfterStartAuditDo(CraftsmanRecommend entity)
        {
            entity.State = RecomandState.审核中;
            await Task.CompletedTask;
        }

        protected override IQueryable<CraftsmanRecommend> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                    .Include(x => x.CreatorUser)
                    .WhereIf(input.Status is 1, x => x.State == RecomandState.审核中)
                    .WhereIf(input.Status is 3, x => x.State == RecomandState.推荐成功)
                    .WhereIf(input.Status is 4, x => x.State == RecomandState.推荐失败)
                    .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), x => x.Detail.PhoneNumber.Contains(input.Keyword)
                                                                              || x.Detail.Realname.Contains(input.Keyword)
                                                                              || x.Detail.WorkUnit.Contains(input.Keyword)
                    )
                ;
        }

        #region Dashboard

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages.Dashboard.Default)]
        public async Task<object> DateAnlayse(AppResultRequestDto input)
        {
            var query = await Repository.GetAll()
                .WhereIf(input.From.HasValue, x => x.CreationTime >= input.From)
                .WhereIf(input.To.HasValue, x => x.CreationTime <= input.From)
                .GroupBy(row => new
                {
                    row.CreationTime.Year,
                    row.CreationTime.Month,
                    row.CreationTime.Date
                }).Select(grp => new
                {
                    Label = $"{grp.Key.Date.Month}月{grp.Key.Date.Day}日",
                    grp.Key.Year,
                    grp.Key.Month,
                    grp.Key.Date,
                    Count = grp.Count(),
                }).ToListAsync();
            return query.OrderBy(x => x.Year).ThenBy(x => x.Month).ThenBy(x => x.Date);
        }

        #endregion
    }
}