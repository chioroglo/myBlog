using LanguageDetection;
using Microsoft.Extensions.Options;
using MyBlog.Common.Options;
using MyBlog.Service;
using MyBlog.Service.Abstract;
using MyBlog.Service.Abstract.Auth;
using MyBlog.Service.Abstract.Auth.Passkeys;
using MyBlog.Service.Abstract.Messaging;
using MyBlog.Service.Abstract.Statistics;
using MyBlog.Service.Auth;
using MyBlog.Service.Auth.Passkeys;
using MyBlog.Service.Messaging;
using MyBlog.Service.Statistics;
using ProfanityFilter.Interfaces;

namespace MyBlog.API.Extensions
{
    public static class ServicesInitializer
    {
        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IMessageBus, MassTransitMessageBus>();
            services.AddScoped<IPasswordAuthService, PasswordAuthService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IAvatarService, AvatarService>();
            services.AddScoped<IPostReactionService, PostReactionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserPunishmentsService, UserPunishmentsService>();
            services.AddScoped<IContentStatisticsService, ContentStatisticsService>();
            
            // Passkey
            services.AddScoped<IPasskeyAuthService, PasskeyAuthService>();
            services.AddScoped<IPasskeyCryptographyService, PasskeyCryptographyService>();
            services.AddScoped<IPasskeySessionsService, PasskeySessionsService>();

            // Semantic analysis
            services.AddScoped<IProfanityFilter, ProfanityFilter.ProfanityFilter>();
            services.AddScoped<ISemanticAnalysisService, SemanticAnalysisService>(serviceProvider =>
            {
                var langDetector = new LanguageDetector();
                langDetector.AddAllLanguages();
                var profanityFilter = serviceProvider.GetRequiredService<IProfanityFilter>();
                var options = serviceProvider.GetRequiredService<IOptions<SemanticAnalysisOptions>>();
                return new SemanticAnalysisService(langDetector, profanityFilter, options);
            });
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