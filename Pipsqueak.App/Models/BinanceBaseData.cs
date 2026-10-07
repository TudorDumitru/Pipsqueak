using Newtonsoft.Json;

namespace Pipsqueak.App.Models;

public class BinanceBaseData
{
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;
}