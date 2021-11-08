using System.Threading.Tasks;

namespace TTWork.Abp.Core.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}