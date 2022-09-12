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
            await CreateHostBuilder().Build().RunAsync();
            return 0;
        }
    }
}
