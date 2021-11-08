namespace AwsomeApi.WeixinWork.Message
{
    public class TextMessage : MessageBase
    {
        public string msgtype { get; set; } = "text";

        /// <summary>
        /// 消息内容，最长不超过2048个字节，超过将截断（支持id转译）
        /// </summary>
        public MessageContentWrap text { get; set; } = new();

        /// <summary>
        /// 表示是否是保密消息，0表示可对外分享，1表示不能分享且内容显示水印，默认为0
        /// </summary>
        public int safe { get; set; } = 0;

        /// <summary>
        /// 表示是否开启id转译，0表示否，1表示是，默认0。仅第三方应用需要用到，企业自建应用可以忽略。
        /// </summary>
        public int enable_id_trans { get; set; } = 0;
    }
}