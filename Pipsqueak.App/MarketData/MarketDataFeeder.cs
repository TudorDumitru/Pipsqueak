using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pipsqueak.App.Models;

namespace Pipsqueak.App.MarketData;

public class MarketDataFeeder<T> : BackgroundService
    where T : BinanceBaseData
{
    private readonly string _url;
    private readonly ILogger<MarketDataFeeder<T>> _logger;
    private readonly IMarketDataConnector _marketDataConnector;
    private readonly IJsonHandler _jsonHandler = new JsonHandler();
    
    public MarketDataFeeder(ILogger<MarketDataFeeder<T>> logger, IMarketDataConnector marketDataConnector, string url)
    {
        _logger = logger;
        _marketDataConnector = marketDataConnector;
        _url = url;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    { 
        await _marketDataConnector.ConnectAsync(_url, stoppingToken);
        
        _logger.LogInformation("Listening for trades...");

        try
        {
            await ListenToTradesAsync(stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error listening to trades:");
        }
        finally
        {
            await _marketDataConnector.DisconnectAsync();
        }
    }

    private async Task ListenToTradesAsync(CancellationToken  stoppingToken)
    {
        await _marketDataConnector.ListenAsync(async message =>
        {
            T data = _jsonHandler.Deserialize<T>(message);
            _logger.LogInformation("{Message}", data.ToString());
            await Task.CompletedTask;
        }, stoppingToken);
    }
}
