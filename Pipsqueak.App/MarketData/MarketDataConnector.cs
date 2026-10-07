using System.Net.WebSockets;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Pipsqueak.App.MarketData;

public class MarketDataConnector : IMarketDataConnector, IDisposable
{
    private readonly ClientWebSocket _socket = new();
    private readonly ILogger<MarketDataConnector> _logger;
    
    public MarketDataConnector(ILogger<MarketDataConnector> logger)
    {
        _logger = logger;
    }
    
    public async Task ConnectAsync(string url, CancellationToken ct)
    {
        _logger.LogInformation("Connecting to {Url}", url);

        await _socket.ConnectAsync(new Uri(url), ct);

        _logger.LogInformation("Connected to Market WebSocket ({Url})", url);    
    }

    public async Task ListenAsync(
        Func<string, Task> onMessage,
        CancellationToken ct)
    {
        byte[] buffer = new byte[8192];

        while (!ct.IsCancellationRequested &&
               _socket.State == WebSocketState.Open)
        {
            using MemoryStream messageStream = new();

            WebSocketReceiveResult result;
            
            do
            {
                result = await _socket.ReceiveAsync(buffer, ct);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger.LogWarning("WebSocket closed by server");
                    return;
                }

                await messageStream.WriteAsync(
                    buffer.AsMemory(0, result.Count),
                    ct);

            } 
            while (!result.EndOfMessage);

            string message = Encoding.UTF8.GetString(messageStream.ToArray());

            await onMessage(message);
        }
    }

    public async Task DisconnectAsync()
    {
        if (_socket.State == WebSocketState.Open)
        {
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }
        Dispose();
    }

    public void Dispose()
    {
        _socket.Dispose();
        GC.SuppressFinalize(this);
    }
}