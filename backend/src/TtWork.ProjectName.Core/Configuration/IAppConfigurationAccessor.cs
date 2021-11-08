using Microsoft.Extensions.Configuration;

namespace TtWork.ProjectName.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }

    }
}