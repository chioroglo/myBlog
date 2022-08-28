using Service;
using Service.Abstract;
using Service.Abstract.Auth;
using Service.Auth;

namespace webApi
{
    public static class ServicesInitializer
    {
        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostsService, PostService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<ICommentService, CommentService>();
        }
    }
}