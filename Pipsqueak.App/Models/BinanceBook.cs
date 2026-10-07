using Newtonsoft.Json;

namespace Pipsqueak.App.Models;

public class BinanceBook
{
    
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;
    
    [JsonProperty("a")]
    public decimal Ask { get; set; }
    
    [JsonProperty("A")]
    public decimal AskSize { get; set; }
    
    [JsonProperty("b")] 
    public decimal Bid { get; set; }
    
    [JsonProperty("B")] 
    public decimal BidSize { get; set; }
    
    public override string ToString()
    {
        return $"Ask: {Ask}; AskSize: {AskSize}; Bid: {Bid}; BidSize: {BidSize}";
    }
}