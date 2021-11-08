namespace AwsomeApi.WeixinWork.Message
{
    public class MessageBase : IMessage
    {
        /// <summary>
        /// 指定接收消息的成员，成员ID列表（多个接收者用‘|’分隔，最多支持1000个）。
        /// 特殊情况：指定为”@all”，则向该企业应用的全部成员发送
        /// </summary>
        public string touser { get; set; }

        /// <summary>
        /// 指定接收消息的部门，部门ID列表，多个接收者用‘|’分隔，最多支持100个。
        /// 当touser为”@all”时忽略本参数
        /// </summary>
        public string toparty { get; set; }

        /// <summary>
        /// 指定接收消息的标签，标签ID列表，多个接收者用‘|’分隔，最多支持100个。
        /// 当touser为”@all”时忽略本参数
        /// </summary>
        public string totag { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public virtual string msgtype { get; set; }

        /// <summary>
        /// 企业应用的id，整型。企业内部开发，可在应用的设置页面查看；第三方服务商，
        /// 可通过接口 获取企业授权信息 获取该参数值 <see cref="https://work.weixin.qq.com/api/doc/90000/90135/90236#10975/%E8%8E%B7%E5%8F%96%E4%BC%81%E4%B8%9A%E6%8E%88%E6%9D%83%E4%BF%A1%E6%81%AF"/>
        /// </summary>
        public int agentid { get; set; }

        /// <summary>
        /// 示是否开启重复消息检查，0表示否，1表示是，默认0
        /// </summary>
        public int enable_duplicate_check { get; set; } = 0;

        /// <summary>
        /// 表示是否重复消息检查的时间间隔，默认1800s，最大不超过4小时
        /// </summary>
        public int duplicate_check_interval { get; set; } = 1800;
    }
}