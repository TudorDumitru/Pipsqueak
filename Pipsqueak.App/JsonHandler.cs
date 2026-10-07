using Newtonsoft.Json;

namespace Pipsqueak.App;

public class JsonHandler : IJsonHandler
{
    public T Deserialize<T>(string message)
        where T : BinanceBaseData
    {
        return JsonConvert.DeserializeObject<T>(message)
               ?? throw new InvalidOperationException(
                   $"Failed to deserialize Binance data into {typeof(T).Name}.");
    }
}