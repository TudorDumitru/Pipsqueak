namespace Pipsqueak.Infrastructure.Interfaces;

public interface IBinanceWebSocketClient
{
    public Task ConnectToWebSocketAsync(string url, CancellationToken ct);
    public Task ListenAsync(Func<string, Task> onMessage, CancellationToken ct);
    public Task DisconnectAsync();
}