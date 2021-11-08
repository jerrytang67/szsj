namespace AwsomeApi.WeixinWork.Message
{
    public interface IMessage
    {
        public string touser { get; set; }
        public string toparty { get; set; }
        public string totag { get; set; }
        public string msgtype { get; set; }

        public int agentid { get; set; }

        public int enable_duplicate_check { get; set; }
        public int duplicate_check_interval { get; set; }
    }
}