using System.Threading.Tasks;
using Abp.Dependency;
using Castle.Core.Logging;
using TTWork.Abp.Core.Net.Sms;

namespace TtWork.ProjectName.Net.Sms
{
    public class SmsSender : ISmsSender, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public SmsSender()
        {
            Logger = NullLogger.Instance;
        }

        public Task SendAsync(string number, string message)
        {
            Logger.Warn("Sending SMS is not implemented! Message content:");
            Logger.Warn("Number  : " + number);
            Logger.Warn("Message : " + message);

            return Task.FromResult(0);
        }
    }
}