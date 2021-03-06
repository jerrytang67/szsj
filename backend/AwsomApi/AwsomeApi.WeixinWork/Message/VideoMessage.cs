namespace AwsomeApi.WeixinWork.Message
{
    public class VideoMessage : MessageBase
    {
        public string msgtype { get; set; } = "video";

        public VideoContentWrap video { get; set; } = new();

        /// <summary>
        /// 表示是否是保密消息，0表示可对外分享，1表示不能分享且内容显示水印，默认为0
        /// </summary>
        public int safe { get; set; } = 0;
    }
}