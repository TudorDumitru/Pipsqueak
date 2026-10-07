using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pipsqueak.App.MarketData;
using Pipsqueak.App.Models;
using Serilog;

namespace Pipsqueak.App.Services;

public static class BuilderService
{
    private static HostApplicationBuilder _builder = new();
    private static string _tradeUrl = string.Empty;
    private static string _bookUrl = string.Empty;

    public static void SetupBuilderServices(string[] args)
    {
        _builder = Host.CreateApplicationBuilder(args);
        ConfigureLogger();
        SetUrls();
        ConfigureWebsockets();
    }

    private static void ConfigureLogger()
    {
        _builder.Services.AddSerilog((_, config) =>
        {
            config.ReadFrom.Configuration(_builder.Configuration);
        });
    }

    private static void SetUrls()
    {
        _tradeUrl = _builder.Configuration["MarketData:TradeUrl"]
                          ?? throw new InvalidOperationException("MarketData:TradeUrl is missing.");

        _bookUrl = _builder.Configuration["MarketData:BookUrl"]
                         ?? throw new InvalidOperationException("MarketData:BookUrl is missing.");
    }

    private static void ConfigureWebsockets()
    {
        _builder.Services.AddTransient<IMarketDataConnector, MarketDataConnector>();

        _builder.Services.AddHostedService<MarketDataFeeder<BinanceTrade>>(
            provider => ActivatorUtilities.CreateInstance<MarketDataFeeder<BinanceTrade>>(
                provider, _tradeUrl));

        _builder.Services.AddHostedService<MarketDataFeeder<BinanceBook>>(
            provider => ActivatorUtilities.CreateInstance<MarketDataFeeder<BinanceBook>>(
                provider, _bookUrl));
    }
    

    public static IHost Build()
    {
        return _builder.Build();
    }
}