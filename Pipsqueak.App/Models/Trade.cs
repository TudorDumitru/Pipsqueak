namespace Pipsqueak.App.Models;

public sealed class Trade
{
    private readonly string _eventType;
    private readonly DateTime _eventTime;
    private readonly string _symbol;
    private readonly decimal _price;
    private readonly decimal _quantity;
    private readonly bool _isBuyerMarketMaker;
    private readonly bool _ignore;

    public Trade(BinanceTrade trade)
    {
        _eventType = trade.EventType;
        _eventTime = Time.GetEventTimeAsDateTime(trade.EventTime);
        _symbol = trade.Symbol;
        _price = trade.Price;
        _quantity = trade.Quantity;
        _isBuyerMarketMaker = trade.IsBuyerMarketMaker;
        _ignore = trade.Ignore;
    }
    
    public override string ToString()
    {
        return $"EventType: {_eventType}; EventTime: {_eventTime}; Symbol: {_symbol}; Price: {_price}; Quantity: {_quantity}; IsBuyerMarketMaker: {_isBuyerMarketMaker}; Ignore: {_ignore}";
    }
}