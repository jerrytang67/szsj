using System.Collections.Generic;

namespace TTWork.Abp.AuditManagement.Audits
{
    public interface IAuditValueProviderManager
    {
        List<IAuditValueProvider> Providers { get; }
    }
}