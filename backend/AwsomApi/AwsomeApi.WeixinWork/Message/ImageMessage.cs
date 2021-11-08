namespace AwsomeApi.WeixinWork.Message
{
    public class ImageMessage : MessageBase
    {
        public string msgtype { get; set; } = "image";

        /// <summary>
        /// 图片媒体文件id，可以调用上传临时素材接口获取
        /// </summary>
        public MediaContentWrap image { get; set; } = new();

        /// <summary>
        /// 表示是否是保密消息，0表示可对外分享，1表示不能分享且内容显示水印，默认为0
        /// </summary>
        public int safe { get; set; } = 0;
    }
}