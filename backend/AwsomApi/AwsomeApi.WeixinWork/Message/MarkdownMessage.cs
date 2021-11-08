namespace AwsomeApi.WeixinWork.Message
{
    public class MarkdownMessage : MessageBase
    {
        public string msgtype { get; set; } = "markdown";

        public MessageContentWrap markdown { get; set; } = new();
    }
}