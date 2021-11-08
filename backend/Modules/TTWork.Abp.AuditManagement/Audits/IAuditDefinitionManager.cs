using System.Collections.Generic;
using JetBrains.Annotations;

namespace TTWork.Abp.AuditManagement.Audits
{
    public interface IAuditDefinitionManager
    {
        AuditDefinition Get([NotNull] string name);

        IReadOnlyList<AuditDefinition> GetAll();

        AuditDefinition GetOrNull(string name);
    }
}