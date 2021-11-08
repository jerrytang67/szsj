using System;
using System.Threading.Tasks;

namespace TTWork.Abp.AuditManagement.Audits
{
    public interface IAuditProvider
    {
        Task<Guid?> GetOrNullAsync(string name);
    }
}