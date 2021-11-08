namespace TTWork.Abp.Core.Authorization.Users
{
    public interface IMayHaveFromUser
    {
        public long? FromUserId { get; set; }
    }
}