namespace Pipsqueak.App.MarketData;

public interface IMarketDataConnector
{
    public Task ConnectAsync(string url, CancellationToken ct);
    public Task ListenAsync(Func<string, Task> onMessage, CancellationToken ct);
    public Task DisconnectAsync();
}