using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using TtWork.ProjectName.Apis.Dtos;
using TtWork.ProjectName.Entities.Organizations;
using TtWork.ProjectName.Entities.Pay;
using TtWork.ProjectName.Managers;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.Core.Domains.Weixin;
using TtWork.ProjectName.Apis.Pays.Dtos;

namespace TtWork.ProjectName.Apis.Pays
{
    [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
    public class PayOrderAppService : AbpAppServiceBase
    {
        private readonly IRepository<TenPayNotify> _tenPayNotifyRepository;
        private readonly IRepository<PayOrder, long> _payorderRepository;
        private readonly IRepository<RefundLog, long> _refundordeRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<WechatUserinfo, string> _wechatUserRepository;
        private readonly IRepository<ProjectNameOrganizationUnit, long> _organizationRepository;

        private readonly PayManager _payaManager;

        public PayOrderAppService(IRepository<TenPayNotify> tenPayNotifyRepository,
            IRepository<PayOrder, long> payorderRepository,
            IRepository<RefundLog, long> refundordeRepository,
            IRepository<User, long> userRepository,
            IRepository<WechatUserinfo, string> wechatUserRepository,
            IRepository<ProjectNameOrganizationUnit, long> organizationRepository)
        {
            _tenPayNotifyRepository = tenPayNotifyRepository;
            _payorderRepository = payorderRepository;
            _refundordeRepository = refundordeRepository;
            _userRepository = userRepository;
            _wechatUserRepository = wechatUserRepository;
            _wechatUserRepository = wechatUserRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task<PagedResultDto<PayOrderDto>> GetAllAsync(AppResultRequestDto input)
        {
            var query = _payorderRepository.GetAll()
                .Include(x => x.FromUser);
            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(z => z.Id).PageBy(input).ToListAsync();

            return new PagedResultDto<PayOrderDto>(totalCount, ObjectMapper.Map<List<PayOrderDto>>(items));
        }

        public async Task<PagedResultDto<RefundLog>> GetAllRefundOrders(AppResultRequestDto input)
        {
            var query = _refundordeRepository.GetAll();
            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(z => z.Id).PageBy(input).ToListAsync();

            return new PagedResultDto<RefundLog>(totalCount, items);
        }

        public async Task<List<RefundDetailDto>> GetRefundDetail(string billNo)
        {
            var payorder = from z in _payorderRepository.GetAll().Where(z => z.BillNo == billNo)
                // join zz in _applyRepository.GetAll() on z.OrderId equals zz.Id
                // join activity in _activityRepository.GetAll() on zz.ActivityId equals activity.Id
                join xx in _userRepository.GetAll() on z.CreatorUserId equals xx.Id
                join wechat in _wechatUserRepository.GetAll() on z.OpenId equals wechat.openid
                join ou in _organizationRepository.GetAll() on z.OrganizationUnitId equals ou.Id
                select new RefundDetail
                {
                    PayOrder = z,
                    // ActivityApply = zz,  Activity = activity, 
                    User = xx, WechatUserinfo = wechat,

                    OrganizationUnit = ou
                };

            return ObjectMapper.Map<List<RefundDetailDto>>(await payorder.ToListAsync());
        }
    }
}