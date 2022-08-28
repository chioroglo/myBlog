using DAL;
using Microsoft.EntityFrameworkCore;

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

        public static int Main(string[] args)
        {
                CreateHostBuilder().Build().Run();

            return 0;
        }
    }
}
