using API.Extensions;

namespace API
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {   
            var host = await WebHostFactory.CreateHostBuilderUsingStartupAndLogging().Build().SeedData();
            
            await host.RunAsync();
        }
    }
}
