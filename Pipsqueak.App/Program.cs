using Microsoft.Extensions.Hosting;
using Pipsqueak.App.Services;
using Serilog;

namespace Pipsqueak.App;

public static class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            Log.Information("Pipsqueak starting up");
            
            // Build app
            BuilderService.SetupBuilderServices(args);
            IHost app = BuilderService.Build();
            
            await app.RunAsync();
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}