using Common.Options;

namespace API.Extensions
{
    public static class OptionsInitializer
    {
        public static void InitializeOptions(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<JsonWebTokenOptions>(
                configuration.GetSection("Jwt"));
        }
    }
}