using MediatR;
using TTWork.Abp.AuditManagement.Domain;

namespace TTWork.Abp.AuditManagement.Events.Queries
{
    public interface IAuditQueryBase : IRequest<AuditResult>
    {
        AuditUserLog Input { get; set; }
        AuditFlow Flow { get; set; }
        AuditNode Node { get; set; }
    }
}