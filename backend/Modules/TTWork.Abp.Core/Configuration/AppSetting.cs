namespace TtWork.ProjectName.Configuration
{
    public static class AppSettings
    {
        public const string UiTheme = "App.UiTheme";

        public static class ClientManagement
        {
            public const string WechatMini = "App.AdminManagement.WechatMini";
        }

        public static class NotifyManagement
        {
            public const string Phone = "App.AdminManagement.RecivePhone";
            public const string NewOrderSendStatus = "App.AdminManagement.NewOrderSendStatus";
            public const string RefundSendStatus = "App.AdminManagement.RefundSendStatus";
            public const string AdminOpenid = "App.AdminManagement.AdminOpenid";
        }
        
        public static class WeixinManagement
        {
            public const string AppId = "App.WeixinManagement.AppId";
            public const string AppSecret = "App.WeixinManagement.AppSecret";
            public const string MiniAppId = "App.WeixinManagement.Mini_AppId";
            public const string MiniAppSecret = "App.WeixinManagement.Mini_AppSecret";
            public const string PayMchId = "App.WeixinManagement.TenPay_MchId";
            public const string PayKey = "App.WeixinManagement.TenPay_Key";
            public const string PayNotify = "App.WeixinManagement.TenPay_Notify";
            public const string TenPay_AppId = "App.WeixinManagement.TenPay_AppId";
            public const string TenPay_AppSecret = "App.WeixinManagement.TenPay_AppSecret";
            
            public const string TenPay_RefundAccount = "App.WeixinManagement.TenPay_RefundAccount";

        }
        public static class OssManagement
        {
            public const string BucketName = "App.OssManagement.BucketName";
            public const string UserName = "App.OssManagement.UserName";
            public const string Password = "App.OssManagement.Password";
            public const string DomainHost = "App.OssManagement.DomainHost";
        }

        
        
        public static class UserManagement
        {
            public static class TwoFactorLogin
            {
                public const string IsGoogleAuthenticatorEnabled = "App.UserManagement.TwoFactorLogin.IsGoogleAuthenticatorEnabled";
            }

            public static class SessionTimeOut
            {
                public const string IsEnabled = "App.UserManagement.SessionTimeOut.IsEnabled";
                public const string TimeOutSecond = "App.UserManagement.SessionTimeOut.TimeOutSecond";
                public const string ShowTimeOutNotificationSecond = "App.UserManagement.SessionTimeOut.ShowTimeOutNotificationSecond";
                public const string ShowLockScreenWhenTimedOut = "App.UserManagement.SessionTimeOut.ShowLockScreenWhenTimedOut";
            }

            public const string AllowSelfRegistration = "App.UserManagement.AllowSelfRegistration";
            public const string IsNewRegisteredUserActiveByDefault = "App.UserManagement.IsNewRegisteredUserActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.UserManagement.UseCaptchaOnRegistration";
            public const string UseCaptchaOnLogin = "App.UserManagement.UseCaptchaOnLogin";
            public const string SmsVerificationEnabled = "App.UserManagement.SmsVerificationEnabled";
            public const string IsCookieConsentEnabled = "App.UserManagement.IsCookieConsentEnabled";
            public const string IsQuickThemeSelectEnabled = "App.UserManagement.IsQuickThemeSelectEnabled";
            public const string AllowOneConcurrentLoginPerUser = "App.UserManagement.AllowOneConcurrentLoginPerUser";
        }
    }
}