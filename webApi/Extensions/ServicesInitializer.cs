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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IFileHandlingService, AvatarService>();
        }
    }
}