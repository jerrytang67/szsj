namespace TTWork.Abp.Activity.Applications
{
    public class GetPointRequestDto
    {
        public long ActivityId { get; set; }
        public string ShareFrom { get; set; }
    }

    public class LuckDrawRequestDto
    {
        public long Id { get; set; }
        public string ShareFrom { get; set; }
    }
}