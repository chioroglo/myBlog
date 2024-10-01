using Common.Options;
using Microsoft.Extensions.Options;
using Service;
using Service.Abstract;
using Service.Abstract.Auth;
using Service.Abstract.Auth.Passkeys;
using Service.Auth;
using Service.Auth.Passkeys;

namespace API.Extensions
{
    public static class ServicesInitializer
    {
        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IFilePerUserHandlingService, AvatarService>();
            services.AddTransient<IPostReactionService, PostReactionService>();
            services.AddTransient<IUserService, UserService>();
            services.AddScoped<ICacheService, RedisDistributedCacheService>();
            // Passkey
            services.AddScoped<IPasskeyAuthService, PasskeyAuthService>();
            services.AddScoped<IPasskeyCryptographyService, PasskeyCryptographyService>();
            services.AddScoped<IPasskeySessionsService, PasskeySessionsService>();
        }

        public static void InitializePasskeyFido2CryptoLibrary(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptions<PasskeyOptions>>().Value.RelyingParty;

            services.AddFido2(config =>
            {
                config.ServerDomain = options.DomainName;
                config.ServerName = options.DisplayName;
                config.ServerIcon = options.Icon;
                config.Origins = options.Origins.ToHashSet();
            });
        }
    }
}