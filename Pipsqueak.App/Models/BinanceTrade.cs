using Newtonsoft.Json;

namespace Pipsqueak.App.Models;

public class BinanceTrade : BinanceBaseData
{
    [JsonProperty("e")]
    public string EventType { get; set; } = string.Empty;

    [JsonProperty("E")]
    public long EventTime { get; set; }

    [JsonProperty("t")]
    public long TradeId { get; set; }

    [JsonProperty("p")]
    public decimal Price { get; set; }

    [JsonProperty("q")]
    public decimal Quantity { get; set; }

    [JsonProperty("T")]
    public long TradeTime { get; set; }

    [JsonProperty("m")]
    public bool IsBuyerMarketMaker { get; set; }

    [JsonProperty("M")]
    public bool Ignore { get; set; }

    public override string ToString()
    {
        return $"BinanceTrade: Price={Price}, Quantity={Quantity}";
    }
}