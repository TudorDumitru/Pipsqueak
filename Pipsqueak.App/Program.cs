using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pipsqueak.App.MarketDataWorker;
using Serilog;

namespace Pipsqueak.App;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Pipsqueak starting up");

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSerilog((services, config) =>
            {
                config.ReadFrom.Configuration(builder.Configuration);
            });

            builder.Services.AddHostedService<MarketDataBackgroundWorker>();

            IHost app = builder.Build();

            await app.RunAsync();
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}