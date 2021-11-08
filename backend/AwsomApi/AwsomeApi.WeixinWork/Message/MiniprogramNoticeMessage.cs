namespace AwsomeApi.WeixinWork.Message
{
    public class MiniprogramNoticeMessage : MessageBase
    {
        public string msgtype { get; set; } = "miniprogram_notice";

        public MiniprogramNoticeWrap miniprogram_notice { get; set; } = new();
    }
}