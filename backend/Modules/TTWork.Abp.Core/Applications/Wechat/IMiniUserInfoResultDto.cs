namespace TTWork.Abp.Core.Applications.Wechat
{
    public class IMiniUserInfoResultDto
    {
        public string openid { get; set; }
        public string nickName { get; set; }
        public int gender { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }

        public string avatarUrl { get; set; }
        public string unionid { get; set; }
    }
}