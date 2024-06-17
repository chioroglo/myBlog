using Common.Options;

namespace API.Extensions
{
    public static class OptionsInitializer
    {
        public static void InitializeOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JsonWebTokenOptions>(
                configuration.GetSection(JsonWebTokenOptions.Config));

            services.Configure<CorsPolicyOptions>(
                configuration.GetSection(CorsPolicyOptions.Config));

            services.Configure<PasskeyRelyingPartyOptions>(
                configuration.GetSection(PasskeyRelyingPartyOptions.Config));

            services.Configure<CacheOptions>(
                configuration.GetSection(CacheOptions.Config));
        }
    }
}