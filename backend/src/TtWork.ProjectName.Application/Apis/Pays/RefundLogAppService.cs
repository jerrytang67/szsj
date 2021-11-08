using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using TtWork.ProjectName.Apis.Pays.Dtos;
using TtWork.ProjectName.Definitions;
using TtWork.ProjectName.Entities.Pay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.AuditManagement.Applications;
using TTWork.Abp.AuditManagement.Applications.Dtos;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.Core.Domains.Weixin;

namespace TtWork.ProjectName.Apis.Pays
{
    public interface IRefundLogsAppService : IAsyncCrudAppService<RefundLogDto, long, RefundLogResultRequestDto, RefundLogDto, RefundLogDto>
    {
        Task<RefundDetailDto> GetRefundDetail(string billNo);
    }

    /// <summary>
    /// 退款记录
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
    public class RefundLogAppService :
        AuditAsyncCrudAppService<RefundLog, RefundLogDto, long, RefundLogResultRequestDto, RefundLogDto, RefundLogDto>,
        IRefundLogsAppService
    {
        private readonly IRepository<RefundLog, long> _repository;
        private readonly IRepository<PayOrder, long> _payOrdeRepository;
        private readonly IRepository<WechatUserinfo, string> _wechatUserInfoRepository;

        public RefundLogAppService(
            IRepository<RefundLog, long> repository,
            IRepository<PayOrder, long> payOrdeRepository,
            IocManager iocManager,
            IRepository<WechatUserinfo, string> wechatUserInfoRepository
        ) : base(repository, iocManager)
        {
            _repository = repository;
            _payOrdeRepository = payOrdeRepository;
            _wechatUserInfoRepository = wechatUserInfoRepository;

            // base.CreatePermissionName = AppPermissions.Pages.Administration.Default;

            base.AuditName = RefundAudit.Default;
        }

        public override async Task<PagedResultDto<RefundLogDto>> GetAllAsync(RefundLogResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(z => z.Id).PageBy(input).ToListAsync();

            var result = new PagedResultDto<RefundLogDto>(totalCount, ObjectMapper.Map<List<RefundLogDto>>(items));

            #region 审核重要过程

            foreach (var dto in result.Items)
            {
                if (!dto.IsAudited)
                {
                    var nextStatus = dto.AuditStatus.HasValue ? dto.AuditStatus + 1 : 0;

                    var nodesDto = ObjectMapper.Map<List<AuditNodeDto>>(await AuditNodeRepository.GetAll()
                        .Where(x => x.AuditFlowId == dto.AuditFlowId && x.Index == nextStatus)
                        .ToListAsync());

                    if (AbpSession.UserId.HasValue)
                    {
                        var userId = AbpSession.UserId.Value;
                        foreach (var node in nodesDto)
                        {
                            node.ICanAudit = node.CanIAudit(userId);
                        }

                        dto.CurrentAuditNodes = nodesDto;
                    }
                }
            }

            #endregion

            return result;
        }


        public async Task<RefundDetailDto> GetRefundDetail(string billNo)
        {
            var payorder = from z in _payOrdeRepository.GetAll().Where(z => z.BillNo == billNo)
                join xx in UserManager.GetAll() on z.CreatorUserId equals xx.Id
                join wechat in _wechatUserInfoRepository.GetAll() on z.OpenId equals wechat.openid
                join r in _repository.GetAll() on z.BillNo equals r.BillNo
                select new RefundDetail
                    {PayOrder = z, User = xx, WechatUserinfo = wechat, RefundLog = r};

            return ObjectMapper.Map<RefundDetailDto>(await payorder.FirstOrDefaultAsync());
        }


        protected override IQueryable<RefundLog> CreateFilteredQuery(RefundLogResultRequestDto input)
        {
            var start = new DateTime();
            var end = new DateTime();
            if (input.CreationTime != null)
            {
                var year = input.CreationTime?.Year ?? System.DateTime.Now.Year;
                var month = input.CreationTime?.Month ?? System.DateTime.Now.Month;
                var day = input.CreationTime?.Day ?? System.DateTime.Now.Day;
                start = new DateTime(year, month, day);
                end = start.AddDays(1);
            }

            var query = Repository.GetAll()
                .WhereIf(input.Price.HasValue, s => s.Price == (input.Price * 100))
                .WhereIf(input.AuditStatus.HasValue, s => s.AuditStatus == input.AuditStatus)
                .WhereIf(input.IsSuccess.HasValue, s => s.IsSuccess == input.IsSuccess)
                .WhereIf(input.CreationTime.HasValue, s => s.CreationTime >= start && s.CreationTime < end)
                .WhereIf(!string.IsNullOrEmpty(input.BillNo), s => s.BillNo.Contains(input.BillNo));

            return query;
        }
    }
}