using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pipsqueak.App.MarketData;
using Pipsqueak.App.MarketDataWorker;
using Pipsqueak.App.Models;
using Serilog;

namespace Pipsqueak.App;

public static class Program
{
    private static HostApplicationBuilder _builder = new();
    
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

        try
        {
            Log.Information("Pipsqueak starting up");
            
            SetupBuilderServices(args);
            
            IHost app = _builder.Build();
            await app.RunAsync();
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static void SetupBuilderServices(string[] args)
    {
        _builder = Host.CreateApplicationBuilder(args);
        _builder.Services.AddSerilog((_, config) =>
        {
            config.ReadFrom.Configuration(_builder.Configuration);
        });
        _builder.Services.AddTransient<IMarketDataConnector, MarketDataConnector>();
        _builder.Services.AddHostedService<MarketDataFeeder<BinanceTrade>>(provider => new MarketDataFeeder<BinanceTrade>(provider.GetRequiredService<ILogger<MarketDataFeeder<BinanceTrade>>>(), provider.GetRequiredService<IMarketDataConnector>(), "wss://stream.binance.com:9443/ws/btcusdt@trade"));
        _builder.Services.AddHostedService<MarketDataFeeder<BinanceBook>>(provider => new MarketDataFeeder<BinanceBook>(provider.GetRequiredService<ILogger<MarketDataFeeder<BinanceBook>>>(), provider.GetRequiredService<IMarketDataConnector>(), "wss://stream.binance.com:9443/ws/btcusdt@bookTicker"));
    }
}