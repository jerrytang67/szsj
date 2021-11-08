namespace AwsomeApi.WeixinWork.WeixinResult
{
    public class TokenResult : BaseWeixinWorkResult
    {
        public string access_token { get; set; }
        
        public int expires_in { get; set; }
    }
}