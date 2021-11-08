using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using TTWork.Abp.AuditManagement.Domain;

namespace TTWork.Abp.AuditManagement.Audits
{
    public class TenantAuditValueProvider : AuditValueProvider
    {
        private readonly IAbpSession _abpSession;
        public const string ProviderName = "T";

        public override string Name => ProviderName;

        public TenantAuditValueProvider(
            IRepository<AuditFlow, Guid> auditFlowRepository,
            IAbpSession abpSession
        ) : base(auditFlowRepository)
        {
            _abpSession = abpSession;
        }

        [UnitOfWork]
        public override async Task<Guid?> GetOrNullAsync(AuditDefinition audit)
        {
            if (!_abpSession.TenantId.HasValue)
                return null;

            var tenantId = _abpSession.TenantId.ToString();

            var dbEntity = await AuditFlowRepository
                .FirstOrDefaultAsync(
                    x => x.ProviderName == ProviderName
                         && x.AuditName == audit.Name
                         && x.Enable
                         && x.ProviderKey == tenantId
                );

            return dbEntity?.Id;
        }
    }
}