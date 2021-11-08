using System.Collections.Generic;

namespace AwsomeApi.WeixinWork.Message
{
    public class MiniprogramNoticeWrap
    {
        /// <summary>
        /// 小程序appid，必须是与当前应用关联的小程序
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 点击消息卡片后的小程序页面，仅限本小程序内的页面。该字段不填则消息点击后不跳转。
        /// </summary>
        public string page { get; set; }

        /// <summary>
        /// 消息标题，长度限制4-12个汉字（支持id转译）
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 消息描述，长度限制4-12个汉字（支持id转译）
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 是否放大第一个content_item
        /// </summary>
        public bool emphasis_first_item { get; set; } = true;

        /// <summary>
        /// 消息内容键值对，最多允许10个item
        /// </summary>
        public List<KeyValueItem> content_item { get; set; } = new();
    }
}