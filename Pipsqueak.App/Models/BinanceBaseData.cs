using Newtonsoft.Json;

namespace Pipsqueak.App;

public class BinanceBaseData
{
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;
}