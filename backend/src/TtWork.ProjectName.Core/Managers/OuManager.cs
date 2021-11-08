using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using TTWork.Abp.Core;
using TtWork.ProjectName.Entities.Organizations;

namespace TtWork.ProjectName.Managers
{
    public class OuManager : AppDomainServicebase
    {
        private readonly IRepository<ProjectNameOrganizationUnit, long> _repository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public OuManager(
            IRepository<ProjectNameOrganizationUnit, long> repository,
            IUnitOfWorkManager unitOfWorkManager,
            IIocManager iocManager) : base(iocManager)
        {
            _repository = repository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public virtual async Task<ProjectNameOrganizationUnit> GetAsync(long id, int? tenantId = null)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
            {
                return await _repository.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);
            }
        }
    }
}