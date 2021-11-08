using System;

namespace TtWork.ProjectName
{
    public class AppConsts
    {
        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "gsKxGZ012HLL3MI5";
        public const int MaxPageSize = 1000;
        public const int DefaultPageSize = 10;
        
        public static TimeSpan AccessTokenExpiration = TimeSpan.FromDays(15);
        public static TimeSpan RefreshTokenExpiration = TimeSpan.FromDays(365);
                
        public const string TokenValidityKey = "token_validity_key";
        public const string SecurityStampKey = "AspNet.Identity.SecurityStamp";
        
        public const string TokenType = "token_type";
        public static string UserIdentifier = "user_identifier";
        
    }
}
