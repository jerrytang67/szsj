using System.Collections.Generic;

namespace TtWork.ProjectName.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
