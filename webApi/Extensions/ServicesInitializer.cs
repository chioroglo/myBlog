using Service;
using Service.Abstract;
using Service.Abstract.Auth;
using Service.Auth;

namespace API.Extensions
{
    public static class ServicesInitializer
    {
        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScopedAuthServices();

            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IFilePerUserHandlingService, AvatarService>();
            services.AddTransient<IPostReactionService, PostReactionService>();
            services.AddTransient<IUserService, UserService>();
        }

        private static void AddScopedAuthServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
        }
    }
}