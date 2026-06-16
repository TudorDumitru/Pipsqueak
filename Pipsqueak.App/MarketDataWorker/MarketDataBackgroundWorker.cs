using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pipsqueak.Infrastructure.Interfaces;

namespace Pipsqueak.App.MarketDataWorker;

public class MarketDataBackgroundWorker : BackgroundService
{
    private const string Url = "wss://stream.binance.com:9443/ws/btcusdt@trade";
    private readonly ILogger<MarketDataBackgroundWorker> _logger;
    private readonly IBinanceWebSocketClient _binanceWebSocketClient;
    
    public MarketDataBackgroundWorker(ILogger<MarketDataBackgroundWorker> logger, IBinanceWebSocketClient binanceWebSocketClient)
    {
        _logger = logger;
        _binanceWebSocketClient = binanceWebSocketClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    { 
        await _binanceWebSocketClient.ConnectToWebSocketAsync(Url, stoppingToken);
        
        _logger.LogInformation("Listening for trades...");

        try
        {
            await ListenToTradesAsync(stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogError("Error listening to trades: {error}", e);
        }
        finally
        {
            await _binanceWebSocketClient.DisconnectAsync();
        }
    }

    private async Task ListenToTradesAsync(CancellationToken  stoppingToken)
    {
        await _binanceWebSocketClient.ListenAsync(async message =>
        {
            _logger.LogInformation("RAW: {Message}", message);
            await Task.CompletedTask;
        }, stoppingToken);
    }
}
