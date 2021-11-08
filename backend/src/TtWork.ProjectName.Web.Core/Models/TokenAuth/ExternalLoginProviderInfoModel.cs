using Abp.AutoMapper;
using TtWork.ProjectName.Authentication.External;

namespace TtWork.ProjectName.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
