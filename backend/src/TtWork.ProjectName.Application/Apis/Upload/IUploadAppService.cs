using System.Threading.Tasks;
using Abp.Application.Services;
using Microsoft.AspNetCore.Http;

namespace TtWork.ProjectName.Upload
{
    public interface IUploadAppService : IApplicationService
    {
        Task<string> Upload(int? id, IFormFile file);
    }
}