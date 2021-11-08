using System.Threading.Tasks;
using TtWork.ProjectName.Configuration.Dto;

namespace TtWork.ProjectName.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
