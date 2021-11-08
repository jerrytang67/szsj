using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.Core.Organizations;

namespace TTWork.Abp.AuditManagement.Audits
{
    public class OrganizationUnitAuditValueProvider : AuditValueProvider
    {
        public ICurrentOrganization CurrentOrganization { get; }

        public const string ProviderName = "O";

        public override string Name => ProviderName;

        public OrganizationUnitAuditValueProvider(
            IRepository<AuditFlow, Guid> auditFlowRepository,
            ICurrentOrganization currentOrganization
        ) : base(auditFlowRepository)
        {
            CurrentOrganization = currentOrganization;
        }

        [UnitOfWork]
        public override async Task<Guid?> GetOrNullAsync(AuditDefinition audit)
        {
            if (!CurrentOrganization.Id.HasValue)
                return null;

            var dbEntity = await AuditFlowRepository
                .FirstOrDefaultAsync(
                    x => x.ProviderName == ProviderName
                         && x.AuditName == audit.Name
                         && x.Enable
                         && x.ProviderKey == CurrentOrganization.Id.ToString()
                );

            return dbEntity?.Id;
        }
    }
}