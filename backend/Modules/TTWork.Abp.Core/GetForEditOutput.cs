using Newtonsoft.Json.Linq;

namespace TTWork.Abp.Core
{
    public class GetForEditOutput<T> : IHaveSchema
    {
        public GetForEditOutput(T data, JToken schema = null)
        {
            Data = data;
            Schema = schema ?? JToken.FromObject(new { });
        }

        public T Data { get; }
        public JToken Schema { get; set; }
    }

    public interface IHaveSchema
    {
        JToken Schema { get; set; }
    }

    public interface ICanSchema
    {
    }
}