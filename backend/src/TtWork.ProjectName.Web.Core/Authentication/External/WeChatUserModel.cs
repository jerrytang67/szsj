namespace TtWork.ProjectName.Authentication.External
{
    public class WeChatUserLoginModel
    {
        public string openid { get; set; }
        public string unionid { get; set; }
        public string headimgurl { get; set; }
        public string nickname { get; set; }
        public string session_key { get; set; }
        
        public string phoneNumber { get; set; }
    }
}