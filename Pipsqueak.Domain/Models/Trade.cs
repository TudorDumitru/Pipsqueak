using Newtonsoft.Json;
namespace Pipsqueak.Domain.Models;

public class Trade
{
    
    private record struct TradeModel
    {
        public string EventType;
        public long EventTime;
        public string Symbol;
        public long TradeId;
        public double Price;
        public double Quantity;
        public long TradeTime;
        public bool IsBuyerMarketMaker;
        public bool Ignore;
    }

    public Trade(string message)
    {
        dynamic json = DeserializeJson(message);
        TradeModel tradeModel = new()
        {
            EventType = json["e"],
            EventTime = json["E"],
            Symbol = json["s"],
            TradeId = json["t"],
            Price = Convert.ToDouble(json["p"]),
            Quantity = Convert.ToDouble(json["q"]),
            TradeTime = json["T"],
            IsBuyerMarketMaker = json["m"],
            Ignore = json["M"]
        };
    }

    private object? DeserializeJson(string message)
    {
        return JsonConvert.DeserializeObject(message);
    }
}