using System;

namespace AwsomeApi.WeixinWork.WeixinResult
{
    [Serializable]
    public class BaseWeixinWorkResult
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }
    }
}