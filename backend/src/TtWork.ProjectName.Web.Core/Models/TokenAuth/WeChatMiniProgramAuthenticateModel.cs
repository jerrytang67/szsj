using System;

namespace TtWork.ProjectName.Models.TokenAuth
{
    public class WeChatMiniProgramAuthenticateModel
    {
        /// <summary>
        /// 用于换取微信的session_key
        /// </summary>
        [Obsolete("2021年4月小程序改登录后弃用")]
        public string code { get; set; }

        /// <summary>
        /// 解密Userinfo使用
        /// </summary>
        public string encryptedData { get; set; }

        public string iv { get; set; }

        public string session_key { get; set; }

        public string openid { get; set; }

        public string unionid { get; set; }

        public string appid { get; set; }
    }
}