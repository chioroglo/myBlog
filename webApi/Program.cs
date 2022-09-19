using API.Extensions;

namespace webApi
{
    public static class Program
    {
        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
        }

        public static async Task<int> Main(string[] args)
        {
            var host = await CreateHostBuilder().Build().SeedData();
            await host.RunAsync();
            return 0;
        }
    }
}
