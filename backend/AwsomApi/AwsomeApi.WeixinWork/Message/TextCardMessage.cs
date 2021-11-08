namespace AwsomeApi.WeixinWork.Message
{
    public class TextCardMessage : MessageBase
    {
        public string msgtype { get; set; } = "textcard";

        public TextCardContentWrap textcard { get; set; } = new();

        /// <summary>
        /// 表示是否开启id转译，0表示否，1表示是，默认0
        /// </summary>
        public int enable_id_trans { get; set; } = 0;
    }
}