using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.AuditManagement.Events;
using TTWork.Abp.AuditManagement.Events.Queries;
using TtWork.ProjectName.Entities.Organizations;

namespace TtWork.ProjectName.Events.AuditQueries
{
    public class OrganizationEditAproveAuditQuery : IAuditQueryBase
    {
        public AuditUserLog Input { get; set; }
        public AuditFlow Flow { get; set; }
        public AuditNode Node { get; set; }
    }

    public class OrganizationEditAproveAuditQueryHandler : AuditQueryHandlerBase<OrganizationEditAproveAuditQuery, OrganizationApply, long>
    {

        public OrganizationEditAproveAuditQueryHandler(
            IRepository<OrganizationApply, long> repository,
            IIocManager iocManager) : base(repository, iocManager)
        {
        }

        /// <summary>
        /// 机构资料编辑审核通过后执行
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override async Task Do(OrganizationApply entity, OrganizationEditAproveAuditQuery request = null)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 机构资料编辑审核退回后执行
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override async Task Reject(OrganizationApply entity, OrganizationEditAproveAuditQuery request = null)
        {
            await Task.CompletedTask;
        }
    }
}