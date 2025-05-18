using Destructurama;
using Serilog;

namespace MyBlog.API
{
    public static class WebHostFactory
    {
        public static IHostBuilder CreateHostBuilderUsingStartupAndLogging()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
                .UseSerilog((context,loggingConfiguration) =>
                {
                    loggingConfiguration.ReadFrom.Configuration(context.Configuration);
                    loggingConfiguration.Destructure.UsingAttributes();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}