using API.Extensions;
using NLog.Web;

namespace API
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            AddLoggingFolderInGlobalDiagnosticsContext(logPath);

            
            var host = await CreateHostBuilder().Build().SeedData();
            await host.RunAsync();
            return 0;
        }

        public static void AddLoggingFolderInGlobalDiagnosticsContext(string logPath)
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            NLog.GlobalDiagnosticsContext.Set("LogDirectory", logPath);
        }

        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .UseNLog();
          
        }
    }
}
