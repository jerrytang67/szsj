using Xunit;

namespace TtWork.ProjectName.Tests.Modules.sms
{
    public class SmsSendTest
    {
        [Fact]
        public void Send()
        {
            SmsHelper.SendAcsCode("18012728118", "123456");
        }
    }
}