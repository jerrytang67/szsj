using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TTWork.Abp.AuditManagement.Audits;

namespace TTWork.Abp.AuditManagement.Events.Queries
{
    public class QueryAuditDefinition : IRequest<List<AuditDefinition>>
    {
        public QueryAuditDefinition()
        {
        }

        public class QueryAuditDefinitionHandle : IRequestHandler<QueryAuditDefinition, List<AuditDefinition>>
        {
            private readonly IAuditDefinitionManager _auditDefinitionManager;

            public QueryAuditDefinitionHandle(IAuditDefinitionManager auditDefinitionManager)
            {
                _auditDefinitionManager = auditDefinitionManager;
            }

            public Task<List<AuditDefinition>> Handle(QueryAuditDefinition request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_auditDefinitionManager.GetAll().ToList());
            }
        }
    }
}