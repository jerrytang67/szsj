using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Session;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.AuditManagement.Events;
using TTWork.Abp.AuditManagement.Events.Queries;
using TtWork.ProjectName.Entities.Organizations;
namespace TtWork.ProjectName.Events.AuditQueries
{
    public class OrganizationApplyAproveAuditQuery : IAuditQueryBase
    {
        public AuditUserLog Input { get; set; }
        public AuditFlow Flow { get; set; }
        public AuditNode Node { get; set; }
    }

    public class OrganizationApplyAproveAuditQueryHandler : AuditQueryHandlerBase<OrganizationApplyAproveAuditQuery, OrganizationApply, long>
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<ProjectNameOrganizationUnit, long> _organizationRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpSession _abpSession;

        public OrganizationApplyAproveAuditQueryHandler(
            IRepository<OrganizationApply, long> repository,
            OrganizationUnitManager organizationUnitManager,
            IRepository<ProjectNameOrganizationUnit, long> organizationRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IAbpSession abpSession,
            IIocManager iocManager) : base(repository, iocManager)
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationRepository = organizationRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _abpSession = abpSession;
        }

        /// <summary>
        /// 机构审核通过后执行
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override async Task Do(OrganizationApply entity, OrganizationApplyAproveAuditQuery request = null)
        {
            var organizationUnit = new ProjectNameOrganizationUnit(_abpSession.TenantId, entity.DisplayName, null) {Detail = entity.Detail};
            organizationUnit.Approve();

            await _organizationUnitManager.CreateAsync(organizationUnit);

            await _unitOfWorkManager.Current.SaveChangesAsync();

            entity.OrganizationUnitId = organizationUnit.Id;

            await Task.CompletedTask;
        }

        /// <summary>
        /// 机构资料编辑审核退回后执行
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override async Task Reject(OrganizationApply entity, OrganizationApplyAproveAuditQuery request = null)
        {
            if (request != null)
                entity.RefuseText = request.Input.Desc;
            await Task.CompletedTask;
        }
    }
}