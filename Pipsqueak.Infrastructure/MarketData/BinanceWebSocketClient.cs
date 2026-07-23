using System.Net.WebSockets;
using System.Text;
using Microsoft.Extensions.Logging;
using Pipsqueak.Domain.Models;
using Pipsqueak.Infrastructure.Interfaces;

namespace Pipsqueak.Infrastructure.MarketData;

public class BinanceWebSocketClient : IBinanceWebSocketClient
{
    private readonly ClientWebSocket _socket = new();
    private readonly ILogger<BinanceWebSocketClient> _logger;

    public BinanceWebSocketClient(ILogger<BinanceWebSocketClient> logger)
    {
        _logger = logger;
    }

    public async Task ConnectToWebSocketAsync(string url, CancellationToken ct)
    {
        _logger.LogInformation("Connecting to {Url}", url);

        await _socket.ConnectAsync(new Uri(url), ct);

        _logger.LogInformation("Connected to Binance WebSocket");
    }

    public async Task ListenAsync(Func<string, Task> onMessage, CancellationToken ct)
    {
        byte[] buffer = new byte[8192];

        while (!ct.IsCancellationRequested && _socket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await _socket.ReceiveAsync(buffer, ct);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                _logger.LogWarning("WebSocket closed by server");
                break;
            }
            
            
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Trade trade = new Trade(message);
            await onMessage(message);
        }
    }

    public async Task DisconnectAsync()
    {
        if (_socket.State == WebSocketState.Open)
        {
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }

        _socket.Dispose();
    }
}