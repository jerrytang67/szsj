using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using JetBrains.Annotations;
using TTWork.Abp.AuditManagement.Domain;

namespace TTWork.Abp.AuditManagement.Audits
{
    public interface IAuditValueProvider
    {
        string Name { get; }

        Task<Guid?> GetOrNullAsync([NotNull] AuditDefinition audit);
    }

    public abstract class AuditValueProvider : IAuditValueProvider, ITransientDependency
    {
        protected IRepository<AuditFlow, Guid> AuditFlowRepository { get; }
        public abstract string Name { get; }

        protected AuditValueProvider(IRepository<AuditFlow, Guid> auditFlowRepository)
        {
            AuditFlowRepository = auditFlowRepository;
        }

        public abstract Task<Guid?> GetOrNullAsync(AuditDefinition audit);
    }
}