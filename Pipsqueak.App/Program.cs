using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pipsqueak.App.MarketDataWorker;
using Pipsqueak.Infrastructure.Interfaces;
using Pipsqueak.Infrastructure.MarketData;
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
        _builder.Services.AddSingleton<IBinanceWebSocketClient, BinanceWebSocketClient>();
        _builder.Services.AddHostedService<MarketDataBackgroundWorker>();
    }
}