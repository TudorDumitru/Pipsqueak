using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Pipsqueak.Infrastructure.MarketData;

namespace Pipsqueak.App.MarketDataWorker;

public class MarketDataBackgroundWorker : BackgroundService
{
    private readonly ILogger<MarketDataBackgroundWorker> _logger;

    public MarketDataBackgroundWorker(ILogger<MarketDataBackgroundWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        BinanceWebSocketClient client = new BinanceWebSocketClient(_logger);

        const string url = "wss://stream.binance.com:9443/ws/btcusdt@trade";

        await client.ConnectAsync(url, stoppingToken);

        _logger.LogInformation("Listening for trades...");

        await client.ListenAsync(async message =>
        {
            _logger.LogInformation("RAW: {Message}", message);
            await Task.CompletedTask;
        }, stoppingToken);

        await client.DisconnectAsync();
    }
}