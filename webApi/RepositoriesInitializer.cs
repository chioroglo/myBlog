using DAL.Repositories;
using DAL.Repositories.Abstract;
using Service.Abstract.Auth;
using Service.Auth;

namespace webApi
{
    public static class RepositoriesInitializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            //services.AddScoped<ICommentRepository, CommentRepository>();
        }
    }
}
