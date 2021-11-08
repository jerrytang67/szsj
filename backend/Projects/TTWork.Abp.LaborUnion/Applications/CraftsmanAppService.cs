using System;
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
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SixLabors.ImageSharp;
using TT.Extensions;
using TT.HttpClient.Weixin;
using TTWork.Abp.AuditManagement.Applications;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.Core.Events.Queries;
using TTWork.Abp.LaborUnion.Applications.Dtos;
using TTWork.Abp.LaborUnion.Definitions;
using TTWork.Abp.LaborUnion.Domains;
using TTWork.Abp.LaborUnion.Events.Commands;

namespace TTWork.Abp.LaborUnion.Applications
{
    public class CraftsmanAppService : AuditAsyncCrudAppService<Craftsman, CraftsmanDto, long, AppResultRequestDto, CraftsmanCreateOrUpdateDto, CraftsmanCreateOrUpdateDto>
    {
        private readonly IMediator _mediator;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPayApi _payApi;

        public CraftsmanAppService(
            IMediator mediator,
            IHostEnvironment hostEnvironment,
            IRepository<Craftsman, long> repository,
            IHttpContextAccessor httpContextAccessor,
            IocManager iocManager, IPayApi payApi) : base(repository, iocManager)
        {
            _mediator = mediator;
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _payApi = payApi;
            base.GetPermissionName = AppPermissions.Pages.Default;
            base.CreatePermissionName = AppPermissions.Pages.Default;
            base.UpdatePermissionName = AppPermissions.Pages.Default;

            base.DeletePermissionName = LaborUnionPermissions.Admin;

            base.GetAllPermissionName = LaborUnionPermissions.Default;


            base.AuditName = LaborUnionAudit.Craftsman;
            EnableGetEdit = true;
        }


        public override async Task<GetForEditOutput<CraftsmanCreateOrUpdateDto>> GetForEdit(EntityDto<long> input)
        {
            Craftsman entity = null;

            if (AbpSession.UserId.HasValue && await IsInRoleAsync(AbpSession.UserId!.Value, "admin"))
            {
                entity = await Repository.FirstOrDefaultAsync(z => z.Id.Equals(input.Id));
            }
            else if (AbpSession.UserId.HasValue)
            {
                entity = await Repository.FirstOrDefaultAsync(z => z.CreatorUserId == AbpSession.UserId!.Value);
            }

            var schema = JToken.FromObject(new { });

            return new GetForEditOutput<CraftsmanCreateOrUpdateDto>(
                entity != null
                    ? ObjectMapper.Map<CraftsmanCreateOrUpdateDto>(entity)
                    : Activator.CreateInstance<CraftsmanCreateOrUpdateDto>(),
                schema);
        }

        protected override async Task AfterUpdateAsync(Craftsman entity, CraftsmanCreateOrUpdateDto input = default)
        {
            await StartAudit(entity);
        }

        [AbpAuthorize(AppPermissions.Pages.Default)]
        public override async Task<CraftsmanDto> CreateAsync(CraftsmanCreateOrUpdateDto input)
        {
            if (await Repository.GetAll().AsNoTracking().AnyAsync(x =>
                x.CreatorUserId == AbpSession.UserId!.Value))
            {
                throw new UserFriendlyException("你已提交过自荐");
            }

            var dto = await base.CreateAsync(input);

            if (dto.AuditFlowId.HasValue)
            {
                await base.StartAudit(dto);
            }

            var data = new SendWechatWorkAppDetail(new MarkdownMessage
                {
                    agentid = 1000004,
                    touser = "TangJiaWei|18913712580",
                    markdown = new MessageContentWrap
                    {
                        content = $@"有新的`红色工匠`自荐 
> **自荐信息** 
> 姓名：{dto.Detail.Realname}
> 手机：{dto.Detail.PhoneNumber}
> 性别：{dto.Detail.Sex}
> 出生年月：{dto.Detail.Birthday}
> 政治面貌：{dto.Detail.PoliticsStatus}
> 工作单位：{dto.Detail.WorkUnit}
> 职务：{dto.Detail.WorkTitle}
> 如需查看详细，请点击：[查看详细](https://szsj.wujiangapp.com/admin/index.html#/craftsman/CraftsmanList)"
                    }
                }
            );

            await _mediator.Publish(new MessageSendCommand(MessageType.WechatWorkApp, data));

            return dto;
        }


        [AbpAuthorize(AppPermissions.Pages.Default)]
        public async Task<CraftsmanDto> GetRedpacket(EntityDto<long> input)
        {
            var entity = await Repository.FirstOrDefaultAsync(x => x.Id == input.Id && x.CreatorUserId == AbpSession.UserId.Value);

            if (entity == null)
                throw new UserFriendlyException("找不到这条自荐记录");

            if (entity.State == RecomandState.推荐成功 && entity.RedpacketRecived == false)
            {
                var openid = await _mediator.Send(new UserLoginKeyQuery(entity.CreatorUserId!.Value, TTWorkConsts.LoginProvider.WeChatMiniProgram));

                var success = await _payApi.Transfers(
                    "wx3452d3067b75dc88",
                    "1486627732",
                    "9d46c43b90229ff44e9599674ad4d683",
                    entity.SecurityStamp!.Value.ToShortString().Replace("_", "").Replace("-", ""),
                    openid,
                    "",
                    Convert.ToInt32(entity.Redpacket!.Value * 100),
                    $"吴江“红色工匠”自荐奖励红包",
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

        [AbpAuthorize]
        public async Task<string> ExportExcel(AppResultRequestDto input)
        {
            CheckGetAllPermission();
            input.MaxResultCount = int.MaxValue;

            var result = await base.GetAllAsync(input);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var sWebRootFolder = _hostEnvironment.ContentRootPath + "/wwwroot";
            var sFileName = @$"工匠自荐导出_{DateTime.Now:yyMMdd_HHmmss}.xlsx";
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
                "ID", "姓名", "性别", "籍贯", "学历", "政治面貌", "出生年月", "所属区域", "工作单位", "职务", "手机号码", "个人简历", "主要成果、获奖情况", "主要事迹", "创建时间", "审核状态"
            };
            int row = 1;

            ws.Cells["A1"].Value = "吴江“红色工匠”自荐信息";
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
                ws.Cells[row, j++].Value = result.Items[i].Detail.Realname;
                ws.Cells[row, j++].Value = result.Items[i].Detail.Sex;
                ws.Cells[row, j++].Value = result.Items[i].Detail.NativePlace;
                ws.Cells[row, j++].Value = result.Items[i].Detail.EducationBackground;
                ws.Cells[row, j++].Value = result.Items[i].Detail.PoliticsStatus;
                ws.Cells[row, j++].Value = result.Items[i].Detail.Birthday;
                ws.Cells[row, j++].Value = result.Items[i].Detail.Address;
                ws.Cells[row, j++].Value = result.Items[i].Detail.WorkUnit;
                ws.Cells[row, j++].Value = result.Items[i].Detail.WorkTitle;
                ws.Cells[row, j++].Value = result.Items[i].Detail.PhoneNumber;
                ws.Cells[row, j++].Value = result.Items[i].Detail.PersonalResume;
                ws.Cells[row, j++].Value = result.Items[i].Detail.MainAchievement;
                ws.Cells[row, j++].Value = result.Items[i].Detail.MainEvent;
                ws.Cells[row, j++].Value = $"{result.Items[i].CreationTime:g}";
                ws.Cells[row, j].Value = $"{result.Items[i].State.ToString()}";
                row++;
            }

            await package.SaveAsync(); //Save the workbook.
            return URL;
        }

        protected override async Task BeforeStartAuditDo(Craftsman entity)
        {
            entity.State = RecomandState.审核中;
            await Task.CompletedTask;
        }

        protected override async Task AfterStartAuditDo(Craftsman entity)
        {
            entity.State = RecomandState.审核中;
            await Task.CompletedTask;
        }

        protected override IQueryable<Craftsman> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
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